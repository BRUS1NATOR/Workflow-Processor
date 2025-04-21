using System.Diagnostics;
using System;
using System.Xml.Linq;
using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core.Step;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;
using static System.Collections.Specialized.BitVector32;

namespace WorkflowProcessor.Core
{
    /// <summary>
    /// Process description
    /// </summary>
    public abstract class WorkflowBuilder : WorkflowInfo
    {
        public WorkflowScheme Scheme { get; set; } = new();

        public virtual Workflow Build()
        {
            return new Workflow()
            {
                Scheme = Scheme.Init(),
                InitialContext = new Context(""),
                Name = Name,
                Version = Version,
                IsAllowedToRunFromWeb = IsAllowedToRunFromWeb
            };
        }


        public WorkflowStep<TElement> Step<TElement>(Action<TElement>? action = null) where TElement : IWorkflowElement
        {
            WorkflowStep<TElement> workflowStep = action is null ? new WorkflowStep<TElement>() : new WorkflowStep<TElement>(action);
            Scheme.Elements.Add(workflowStep);
            return workflowStep;
        }



        public WorkflowStep<StartActivity> StepStart(Action<StartActivity>? action = null)
        {
            return Step(action);
        }

        public WorkflowStep<EndActivity> StepEnd(Action<EndActivity>? action = null)
        {
            return Step(action);
        }

        public WorkflowStep<If> StepIf(Func<IWorkflowInstance, bool> condition)
        {
            return Step<If>(x => x.SetCondition(condition));
        }

        public WorkflowStep<LogActivity> StepLog(Action<LogActivity> action)
        {
            return Step(action);
        }

        public WorkflowStep<ParallelExclusiveGatewayEnd> StepParallelGatewayClose(WorkflowStep gatewayStart)
        {
            WorkflowStep<ParallelExclusiveGatewayEnd> workflowStep = new WorkflowStep<ParallelExclusiveGatewayEnd>(x =>
            {
                x.GatewayStartStepId = gatewayStart.StepId;
            });
            Scheme.Elements.Add(workflowStep);
            return workflowStep;
        }
    }

    /// <summary>
    /// Process description with context
    /// </summary>
    public abstract partial class WorkflowBuilder<TContextData> : WorkflowBuilder
        where TContextData : IContextData, new()
    {
        public override Workflow Build()
        {
            return new Workflow()
            {
                Scheme = Scheme.Init(),
                InitialContext = new Context<TContextData>(new TContextData()),
                Name = Name,
                Version = Version,
                IsAllowedToRunFromWeb = IsAllowedToRunFromWeb
            };
        }

        public WorkflowStep<StartActivity<TContextData>> StepStart(Action<StartActivity<TContextData>>? action = null)
        {
            return Step(action);
        }

        public WorkflowStep<LogActivity<TContextData>> StepLog(Action<LogActivity<TContextData>> action = null)
        {
            return Step(action);
        }

        public WorkflowStep<CodeActivity<TContextData>> StepCode(Action<CodeActivity<TContextData>> action)
        {
            return Step(action);
        }

        public WorkflowStep<If<TContextData>> StepIf(Func<Context<TContextData>, bool> condition)
        {
            return Step<If<TContextData>>(x => x.SetCondition(condition));
        }

        public WorkflowStep<ParallelExclusiveGateway<TContextData, T>> StepParallelGateway<T>(Func<Context<TContextData>, T> condition)
        {
            return Step<ParallelExclusiveGateway<TContextData, T>>(x => x.SetCondition(condition));
        }

        public WorkflowStep<UserActivity<TContextData>> StepUserTask(Action<UserActivity<TContextData>> action)
        {
            return Step(action);
        }

        public WorkflowStep<SubprocessActivity<TContextData, TProcess>> StepSubprocess<TProcess>(Action<SubprocessActivity<TContextData, TProcess>> action)
        where TProcess : WorkflowBuilder, new()
        {
            return Step(action);
        }

        public WorkflowStep<SubprocessActivity<TContextData, TProcess, TProcessContext>> StepSubprocess<TProcess, TProcessContext>(Action<SubprocessActivity<TContextData, TProcess, TProcessContext>> action)
            where TProcess : WorkflowBuilder<TProcessContext>, new()
            where TProcessContext : IContextData, new()
        {
            WorkflowStep<SubprocessActivity<TContextData, TProcess, TProcessContext>> workflowStep =
                new WorkflowStep<SubprocessActivity<TContextData, TProcess, TProcessContext>>(action);
            Scheme.Elements.Add(workflowStep);
            return workflowStep;
        }
    }
}