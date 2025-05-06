using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneOf;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.ExecutionResults.Interfaces;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.MasstransitWorkflow;
using WorkflowProcessor.MasstransitWorkflow.Models;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Services;

namespace WorkflowProcessor.Core
{
    public class WorkflowExecutor
    {
        private readonly ILogger<WorkflowExecutor> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowStorage _workflowStorage;
        private readonly WorkflowSender _sender;

        public WorkflowExecutor(ILogger<WorkflowExecutor> logger, IServiceProvider serviceProvider, WorkflowContext dbContext, WorkflowStorage workflowStorage, WorkflowSender sender)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _workflowStorage = workflowStorage;
            _sender = sender;
        }

        /// <summary>
        /// Start process
        /// </summary>
        /// <typeparam name="T">Process type</typeparam>
        /// <typeparam name="TContext">Process context type</typeparam>
        /// <param name="contextData">Initial context data</param>
        /// <param name="initiator">Initiator (user id)</param>
        /// <param name="parent">Parent of new workflow instance</param>
        /// <returns></returns>
        public async Task<WorkflowInstance> StartProcessAsync<T, TContext>(TContext? contextData, long? initiator = null, IWorkflowInstance? parent = null)
            where T : WorkflowBuilder<TContext>, new()
            where TContext : IContextData, new()
        {
            var process = new T().Build();
            if (contextData is not null)
            {
                process.InitialContext.DataObject = contextData;
            }
            return await StartProcessAsync(process, initiator, parent);
        }

        /// <summary>
        /// Start process
        /// </summary>
        /// <typeparam name="T">Process type</typeparam>
        /// <param name="initiator">Initiator (user id)</param>
        /// <param name="parent">Parent of new workflow instance</param>
        /// <returns></returns>
        public async Task<WorkflowInstance> StartProcessAsync<T>(long? initiator = null, IWorkflowInstance? parent = null) where T : WorkflowBuilder, new()
        {
            return await StartProcessAsync(new T().Build(), null, parent);
        }

        /// <summary>
        /// Start process
        /// </summary>
        /// <param name="workflow">Workflow</param>
        /// <param name="initiator">Initiator (user id)</param>
        /// <param name="parent">Parent of new workflow instance</param>
        /// <returns></returns>
        public async Task<WorkflowInstance> StartProcessAsync(Workflow workflow, long? initiator = null, IWorkflowInstance? parent = null)
        {
            // Создаем Instance
            var workflowInstance = new WorkflowInstance();
            workflowInstance.ParentId = parent?.Id;
            workflowInstance.Initiator = initiator;
            workflowInstance.WorkflowInfo = new WorkflowInfo() { Name = workflow.Name, Version = workflow.Version };
            workflowInstance.Context = workflow.InitialContext;
            workflowInstance.Context.WorkflowInstance = workflowInstance;
            workflowInstance.Name = workflow.Name;
            workflowInstance.Status = WorkflowInstanceStatus.Executing;

            var startStep = workflow.Scheme.Start;
            // Создаем точку исполнения
            var executionPoint = new WorkflowExecutionPoint()
            {
                Status = WorkflowExecutionStepStatus.Executing,
                StepId = startStep.StepId,
                ActivityTypeName = startStep.ActivityTypeName
            };
            workflowInstance.WorkflowExecutionPoints.Add(executionPoint);
            workflowInstance.Context.Save();
            //
            _dbContext.WorkflowInstances.Add(workflowInstance);
            await _dbContext.SaveChangesAsync();
            //
            await ExecuteActivityAsync(workflow, workflowInstance, new WorkflowExecuteStep() { StepId = startStep.StepId, WorkflowInstanceId = workflowInstance.Id }, executionPoint);

            return workflowInstance;
        }

        public async Task<WorkflowExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance, WorkflowExecuteStep executeNext)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowInstance.WorkflowInfo);

            var previousExecutionPoint = _dbContext.WorkflowExecutionPoints.First(x => x.Id == executeNext.PreviousExecutionPointId);
            previousExecutionPoint.Status = WorkflowExecutionStepStatus.Finished;
            await _dbContext.SaveChangesAsync();

            var nextStep = workflow.Scheme.Elements.First(x => x.StepId == executeNext.StepId);

            if(nextStep.BaseActivityType == Activities.BaseAcitivityType.ParallelGatewayEnd)
            //if (typeof(IParallelExclusiveGatewayEnd).IsAssignableFrom(nextStep.ActivityType))
            { 
                var closeGatewayActivity = (ParallelExclusiveGatewayEnd)ActivatorUtilities.CreateInstance(_serviceProvider, nextStep.ActivityType);
                nextStep.Setup(closeGatewayActivity, workflowInstance.Context);

                var startGatewayStep = workflow.Scheme.Elements.First(x => x.StepId == closeGatewayActivity.GatewayStartStepId);
                //
                var incomingSteps = workflow.Scheme.GetIncomingSteps(nextStep);
                // ids of activities that must be finished
                var activitiesToFinishIds = incomingSteps.Select(x => x.StepId);

                // Execution point of start parallel gateway
                var parallelExecutionPoint = _dbContext.WorkflowExecutionPoints
                    .Where(x => x.WorkflowInstanceId == workflowInstance.Id && x.ActivityTypeName == startGatewayStep.ActivityTypeName)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                var execPoins = _dbContext.WorkflowExecutionPoints.Where(x => x.WorkflowInstanceId == workflowInstance.Id 
                    && activitiesToFinishIds.Contains(x.StepId) 
                    && x.Status == WorkflowExecutionStepStatus.Finished);

                if (execPoins.Count() != (parallelExecutionPoint is null ? 0 : parallelExecutionPoint.ActivatedStepsId.Count()))
                {
                    return new WorkflowExecutionResult(true);
                }
            }
            var executionPoint = new WorkflowExecutionPoint()
            {
                Status = WorkflowExecutionStepStatus.Executing,
                StepId = executeNext.StepId,
                ActivityTypeName = nextStep.ActivityTypeName
            };
            workflowInstance.WorkflowExecutionPoints.Add(executionPoint);
            await _dbContext.SaveChangesAsync();

            return await ExecuteActivityAsync(workflow, workflowInstance, executeNext, executionPoint);
        }

        private async Task<WorkflowExecutionResult> ExecuteActivityAsync(Workflow workflow, IWorkflowInstance workflowInstance, WorkflowExecuteStep step, WorkflowExecutionPoint executionPoint)
        {
            var currentStep = workflow.Scheme.Elements.First(x => x.StepId == step.StepId);

            if (currentStep.Metadata is not null)
            {
                _logger.LogInformation(currentStep.Metadata.ElementDisplayName);
            }
            var executionResult = await ExecuteStepAsync(workflowInstance, currentStep);

            workflowInstance.Context.Save();
            await _dbContext.SaveChangesAsync();

            return await ProcessExecutionResultAsync(workflow, workflowInstance, executionPoint, executionResult);
        }

        private async Task<ActivityExecutionResult> ExecuteStepAsync(IWorkflowInstance workflowInstance, WorkflowStep step)
        {
            var workflowElement = (IWorkflowElement)ActivatorUtilities.CreateInstance(_serviceProvider, step.ActivityType);
            step.Setup(workflowElement, workflowInstance.Context);

            return await workflowElement.ExecuteAsync(workflowInstance);
        }

        private async Task<WorkflowExecutionResult> ProcessExecutionResultAsync(IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, ActivityExecutionResult executionResult)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowInstance);
            if (workflow is null)
            {
                return new WorkflowExecutionResult(false, "WorkflowNotFound");
            }
            return await ProcessExecutionResultAsync(workflow, workflowInstance, executionPoint, executionResult);
        }

        private async Task<WorkflowExecutionResult> ProcessExecutionResultAsync(Workflow workflow, IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, ActivityExecutionResult executionResult)
        {
            if (executionResult.StatusCode == ExecutionResultStatusCode.Finish)
            {
                workflowInstance.Status = WorkflowInstanceStatus.Finished;
                executionPoint.Status = WorkflowExecutionStepStatus.Finished;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Process {workflowInstance.Name} completed");
                // Send bookmark complete to parent
                await WorkflowInstanceFinishedAsync(workflowInstance);
                //
                return new WorkflowExecutionResult(true, "Process completed");
            }
            if (executionResult is IActivityExecutionParallelResult parallelResult)
            {
                return await ProcessParallelResult(workflow, workflowInstance, executionPoint, parallelResult);
            }
            //
            if (executionResult is IActivityExecutionBlockingResult blockingResult)
            {
                return await ProcessBlockingResult(executionPoint, executionResult, blockingResult);
            }

            // Defining the next step
            var nextStepOrError = GetNextStep(workflow, workflowInstance, executionPoint, executionResult);

            WorkflowStep nextStep;
            string errorMessage;
            if (nextStepOrError.TryPickT1(out errorMessage, out nextStep))
            {
                return new WorkflowExecutionResult(false, errorMessage);
            }
            executionPoint.ActivatedStepsId = [nextStep.StepId];
            await _dbContext.SaveChangesAsync();
            await ProceedAsync(workflowInstance, executionPoint, nextStep);
            return new WorkflowExecutionResult(true);
        }

        private async Task<WorkflowExecutionResult> ProcessBlockingResult(WorkflowExecutionPoint executionPoint, ActivityExecutionResult executionResult, IActivityExecutionBlockingResult blockingResult)
        {
            // Create bookmark
            _logger.LogDebug($"Bookmark created for {executionPoint.StepId}");
            executionPoint.Status = WorkflowExecutionStepStatus.Suspended;

            var userResult = executionResult as IActivityExecutionUserResult;
            var bookmark = new WorkflowBookmark()
            {
                Status = WorkflowBookmarkStatus.Active,
                Type = userResult is null ? WorkflowBookmarkType.Default : WorkflowBookmarkType.UserTask,
                UserTasks = userResult is null ? new List<UserTask>() : userResult.Users.Select(x => new UserTask() { UserId = x }).ToList(),
                WorkflowExecutionPoint = executionPoint,
                Name = blockingResult.BookmarkName,
                WorkflowChildId = blockingResult.ChildWorkflowInstanceId
            };

            _dbContext.WorkflowBookmarks.Add(bookmark);
            await _dbContext.SaveChangesAsync();
            return new WorkflowExecutionResult(true, "Bookmark created");
        }

        private async Task<WorkflowExecutionResult> ProcessParallelResult(Workflow workflow, IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, IActivityExecutionParallelResult parallelResult)
        {
            var nextSteps = GetNextSteps(workflow, workflowInstance, executionPoint, parallelResult);
            foreach (var step in nextSteps)
            {
                await ProceedAsync(workflowInstance, executionPoint, step);
            }
            executionPoint.ActivatedStepsId = nextSteps.Select(x => x.StepId).ToArray();
            await _dbContext.SaveChangesAsync();
            return new WorkflowExecutionResult(true);
        }

        private async Task ProceedAsync(IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, WorkflowStep nextStep)
        {
            //
            await _sender.SendExecuteNext(new WorkflowExecuteStep()
            {
                StepId = nextStep.StepId,
                PreviousExecutionPointId = executionPoint.Id,
                WorkflowInstanceId = workflowInstance.Id
            });
        }

        private async Task WorkflowInstanceFinishedAsync(IWorkflowInstance workflowInstance)
        {
            await _sender.SendFinish(new WorkflowInstanceFinishMessage()
            {
                WorkflowInstanceId = workflowInstance.Id
            });
        }

        private OneOf<WorkflowStep, string> GetNextStep(Workflow workflow, IWorkflowInstance workflowInstance, WorkflowExecutionPoint point, ActivityExecutionResult executionResult)
        {
            var outConnetctions = workflow.Scheme.GetOutgoingConnections(point.StepId);
            if (!outConnetctions.Any())
            {
                return "No outgoing connections found";
            }

            if (executionResult is IActivityExecutionResultWithValue executionResultWithValue)
            {
                foreach (var connection in outConnetctions)
                {
                    if (connection is IConditionalConnection conditionalConnection)
                    {
                        if (conditionalConnection.Compare(executionResultWithValue.Value))
                        {
                            return conditionalConnection.Target;
                        }
                    }
                    else if (connection is IUserConnection userConnection)
                    {
                        if (userConnection.Connector == executionResultWithValue.Value?.ToString())
                        {
                            if (userConnection.Compare(executionResultWithValue.Value?.ToString(), workflowInstance.Context.DataObject!))
                            {
                                return userConnection.Target;
                            }
                            return "Connection conditions not met";
                        }
                    }
                    else
                    {
                        return connection.Target;
                    }
                }
                return $"Connection with name \"{executionResultWithValue.Value}\" not found";
            }

            var simpleConnection = outConnetctions.FirstOrDefault(x => !(x is IConditionalConnection));
            if (simpleConnection is null)
            {
                return "Next step not found!";
            }
            return simpleConnection.Target;
        }

        private List<WorkflowStep> GetNextSteps(Workflow workflow, IWorkflowInstance workflowInstance, WorkflowExecutionPoint point, IActivityExecutionParallelResult executionResult)
        {
            var outConnetctions = workflow.Scheme.GetOutgoingConnections(point.StepId);
            var result = new List<WorkflowStep>();

            if (executionResult is IActivityExecutionResultWithValue executionResultWithValue)
            {
                foreach (var conditionalConnection in outConnetctions.Where(x => x is IConditionalConnection))
                {
                    if (((IConditionalConnection)conditionalConnection).Compare(executionResultWithValue.Value))
                    {
                        result.Add(conditionalConnection.Target);
                    }
                }
            }

            var simpleConnections = outConnetctions.Where(x => !(x is IConditionalConnection)).Select(x => x.Target);
            result.AddRange(simpleConnections);
            return result;
        }

        public async Task<WorkflowExecutionResult> CompleteBookmark(WorkflowBookmark bookmark, ActivityExecutionResult executionResult)
        {
            return await ProcessExecutionResultAsync(bookmark.WorkflowExecutionPoint.WorkflowInstance, bookmark.WorkflowExecutionPoint, executionResult);
        }
    }
}
