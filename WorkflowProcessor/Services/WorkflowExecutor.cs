using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.Enums;
using WorkflowProcessor.Core.Results;
using WorkflowProcessor.Core.Results.Interfaces;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Services;

namespace MassTransitExample.Services
{
    public class WorkflowExecutor
    {
        private readonly ILogger<WorkflowExecutor> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowStorage _workflowStorage;
        private readonly WorkflowSender _sender;
        private readonly WorkflowInstanceFactory _workflowInstanceFactory;

        public WorkflowExecutor(ILogger<WorkflowExecutor> logger, IServiceProvider serviceProvider, WorkflowContext dbContext, WorkflowStorage workflowStorage, WorkflowSender sender, WorkflowInstanceFactory workflowInstanceFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
            _workflowStorage = workflowStorage;
            _sender = sender;
            _workflowInstanceFactory = workflowInstanceFactory;
        }

        public async Task StartProcessAsync<T>() where T : WorkflowBuilder, new()
        {
            await StartProcessAsync(new T().Build());
        }

        public async Task StartProcessAsync(Workflow workflow)
        {
            // Создаем Instance
            var workflowInstance = new WorkflowInstance();
            workflowInstance.Workflow = workflow;
            workflowInstance.Context = workflow.ContextData;
            workflowInstance.Status = WorkflowInstanceStatus.Executing;

            var startStep = workflowInstance.Workflow.Scheme.GetStartStep();
            // Создаем точку исполнения
            var executionPoint = new WorkflowExecutionPoint()
            {
                Status = WorkflowExecutionStepStatus.Executing,
                StepId = startStep.Id,
                ActivityType = startStep.ActivityType
            };
            workflowInstance.WorkflowExecutionPoints.Add(executionPoint);
            //
            _dbContext.WorkflowInstances.Add(workflowInstance);
            await _dbContext.SaveChangesAsync();
            //
            await ExecuteActivity(workflowInstance, new WorkflowExecuteStep() { StepId = startStep.Id, WorkflowInstanceId = workflowInstance.Id }, executionPoint);
        }


        public async Task ExecuteAsync(IWorkflowInstance workflowInstance, WorkflowExecuteStep executeNext)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowInstance.Workflow);

            var previousExecutionPoint = _dbContext.WorkflowExecutionPoints.First(x => x.Id == executeNext.PreviousExecutionPointId);
            previousExecutionPoint.Status = WorkflowExecutionStepStatus.Finished;

            var executionPoint = new WorkflowExecutionPoint()
            {
                Status = WorkflowExecutionStepStatus.Executing,
                StepId = executeNext.StepId,
                ActivityType = workflowInstance.Workflow.Scheme.Elements.First(x => x.Id == executeNext.StepId).ActivityType
            };
            workflowInstance.WorkflowExecutionPoints.Add(executionPoint);
            await _dbContext.SaveChangesAsync();

            await ExecuteActivity(workflowInstance, executeNext, executionPoint);
        }

        private async Task ExecuteActivity(IWorkflowInstance workflowInstance, WorkflowExecuteStep executeStep, WorkflowExecutionPoint executionPoint)
        {
            var currentStep = workflowInstance.Workflow.Scheme.Elements.First(x => x.Id == executeStep.StepId);
            var meta = await GetMetadataAsync(workflowInstance, currentStep);
            if (meta is not null)
            {
                _logger.LogInformation(meta.ElementDisplayName);
            }
            var executionResult = await ExecuteStep(workflowInstance, currentStep);

            workflowInstance.JsonData = workflowInstance.Context.GetJsonContextValue();
            await _dbContext.SaveChangesAsync();

            await ProcessExecutionResultAsync(workflowInstance, executionPoint, executionResult);
        }

        public async Task<bool> ProcessExecutionResultAsync(IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, WorkflowResult executionResult)
        {
            if (executionResult is IWorkflowBlockingResult blockingResult)
            {
                if (blockingResult.CreateBookmark)
                {
                    _logger.LogInformation($"Создан bookmark для {executionPoint.StepId}");
                    executionPoint.Status = WorkflowExecutionStepStatus.Suspended;
                    var bookmark = new WorkflowBookmark()
                    {
                        Status = WorkflowBookmarkStatus.Active,
                        WorkflowExecutionPoint = executionPoint
                    };

                    _dbContext.WorkflowBookmarks.Add(bookmark);
                    await _dbContext.SaveChangesAsync();
                    if (executionResult is IWorkflowUserResult userResult)
                    {
                        _dbContext.UserTasks.Add(new UserTask()
                        {
                            DisplayName = userResult.TaskName,
                            Metadata = new UserTaskMetadata()
                            {
                                //UserId = userResult.UserId
                            },
                            UserId = userResult.UserId,
                            WorkflowBookmark = bookmark
                        });
                        await _dbContext.SaveChangesAsync();
                    }
                    return true;
                }
            }

            // Определяем следующий шаг
            WorkflowStep? nextStep = GetNextStep(workflowInstance, executionPoint, executionResult);
            if (nextStep is null)
            {
                _logger.LogInformation("Следующий шаг не найден");
                return false;
            }
            if (nextStep == workflowInstance.Workflow.Scheme.End)
            {
                workflowInstance.Status = WorkflowInstanceStatus.Finished;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Процесс завершен");
                return true;
            }

            await ProceedAsync(workflowInstance, executionPoint, nextStep);
            return true;
        }

        private async Task<WorkflowResult> ExecuteStep(IWorkflowInstance workflowInstance, WorkflowStep step)
        {
            var wf = ActivatorUtilities.CreateInstance(_serviceProvider, step.GetWorkflowElementType());
            step.SetupWorkflowInstance(wf, workflowInstance.Context.Data);

            IWorkflowElement workflowElement = (IWorkflowElement)wf;

            return await workflowElement.ExecuteAsync(workflowInstance);
        }

        private async Task<WorkflowElementMetadata?> GetMetadataAsync(IWorkflowInstance workflowInstance, WorkflowStep step)
        {
            var wf = ActivatorUtilities.CreateInstance(_serviceProvider, step.GetWorkflowElementType());
            step.SetupWorkflowInstance(wf, workflowInstance.Context.Data);

            IWorkflowElement workflowElement = (IWorkflowElement)wf;

            return await Task.FromResult(workflowElement.Metadata);
        }

        public async Task ProceedAsync(IWorkflowInstance workflowInstance, WorkflowExecutionPoint executionPoint, WorkflowStep nextStep)
        {
            await _sender.SendExecuteNext(new WorkflowExecuteStep()
            {
                StepId = nextStep.Id,
                PreviousExecutionPointId = executionPoint.Id,
                WorkflowInstanceId = workflowInstance.Id
            });
        }

        private WorkflowStep? GetNextStep(IWorkflowInstance workflowInstance, WorkflowExecutionPoint point, WorkflowResult executionResult)
        {
            var outConnetctions = workflowInstance.Workflow.Scheme.GetOutgoingConnections(point.StepId);
            if (!outConnetctions.Any())
            {
                _logger.LogInformation("Не найдено исходящих переходов");
                return null;
            }

            if (executionResult is IWorkflowResultWithValue executionResultWithValue)
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
                    else
                    {
                        return connection.Target;
                    }
                }
            }
            else
            {
                foreach (var connection in outConnetctions)
                {
                    if (connection is IConditionalConnection)
                    {
                        continue;
                    }
                    else
                    {
                        return connection.Target;
                    }
                }
            }

            return null;
        }
    }
}
