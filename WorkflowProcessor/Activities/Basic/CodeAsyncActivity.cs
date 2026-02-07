using Microsoft.Extensions.DependencyInjection;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.ExecutionResults;
using WorkflowProcessor.Core.WorkflowElement;
using WorkflowProcessor.Persistance.Context;

namespace WorkflowProcessor.Activities.Basic
{
    [ActivityType(BaseAcitivityType.CodeActivity)]
    public class CodeAsyncActivity<TContextData> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
    {
        protected Func<Context<TContextData>, Task>? ExecuteCodeAsync;

        public void CodeAsync(Func<Context<TContextData>, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            if (ExecuteCodeAsync is not null)
            {
                await ExecuteCodeAsync.Invoke(data);
            }

            return await base.ExecuteAsync(workflowInstance);
        }
    }

    [ActivityType(BaseAcitivityType.CodeActivity)]
    public class CodeAsyncActivity<TContextData, T1Service> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
        where T1Service : class
    {
        protected Func<Context<TContextData>, T1Service, Task>? ExecuteCodeAsync;
        private IServiceProvider _serviceProvider;

        public CodeAsyncActivity(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void CodeAsync(Func<Context<TContextData>, T1Service, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<T1Service>();
                if (ExecuteCodeAsync is not null)
                {
                    await ExecuteCodeAsync.Invoke(data, service);
                }
            }
            return await base.ExecuteAsync(workflowInstance);
        }
    }

    [ActivityType(BaseAcitivityType.CodeActivity)]
    public class CodeAsyncActivity<TContextData, T1Service, T2Service> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
        where T1Service : class
        where T2Service : class
    {
        protected Func<Context<TContextData>, T1Service, T2Service, Task>? ExecuteCodeAsync;
        private IServiceProvider _serviceProvider;

        public CodeAsyncActivity(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void CodeAsync(Func<Context<TContextData>, T1Service, T2Service, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<T1Service>();
                var service2 = scope.ServiceProvider.GetRequiredService<T2Service>();
                if (ExecuteCodeAsync is not null)
                {
                    await ExecuteCodeAsync.Invoke(data, service1, service2);
                }
            }
            return await base.ExecuteAsync(workflowInstance);
        }
    }
    

    [ActivityType(BaseAcitivityType.CodeActivity)]
    public class CodeAsyncActivity<TContextData, T1Service, T2Service, T3Service> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
        where T1Service : class
        where T2Service : class
        where T3Service : class
    {
        protected Func<Context<TContextData>, T1Service, T2Service, T3Service, Task>? ExecuteCodeAsync;
        private IServiceProvider _serviceProvider;

        public CodeAsyncActivity(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void CodeAsync(Func<Context<TContextData>, T1Service, T2Service, T3Service, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<T1Service>();
                var service2 = scope.ServiceProvider.GetRequiredService<T2Service>();
                var service3 = scope.ServiceProvider.GetRequiredService<T3Service>();
                if (ExecuteCodeAsync is not null)
                {
                    await ExecuteCodeAsync.Invoke(data, service1, service2, service3);
                }
            }
            return await base.ExecuteAsync(workflowInstance);
        }
    }


    [ActivityType(BaseAcitivityType.CodeActivity)]
    public class CodeAsyncActivity<TContextData, T1Service, T2Service, T3Service, T4Service> : WorkflowElement<TContextData>
        where TContextData : IContextData, new()
        where T1Service : class
        where T2Service : class
        where T3Service : class
        where T4Service : class
    {
        protected Func<Context<TContextData>, T1Service, T2Service, T3Service, T4Service, Task>? ExecuteCodeAsync;
        private IServiceProvider _serviceProvider;

        public CodeAsyncActivity(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public void CodeAsync(Func<Context<TContextData>, T1Service, T2Service, T3Service, T4Service, Task> ExecuteCodeAsync)
        {
            this.ExecuteCodeAsync = ExecuteCodeAsync;
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(IWorkflowInstance workflowInstance)
        {
            var data = Data(workflowInstance);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<T1Service>();
                var service2 = scope.ServiceProvider.GetRequiredService<T2Service>();
                var service3 = scope.ServiceProvider.GetRequiredService<T3Service>();
                var service4 = scope.ServiceProvider.GetRequiredService<T4Service>();
                if (ExecuteCodeAsync is not null)
                {
                    await ExecuteCodeAsync.Invoke(data, service1, service2, service3, service4);
                }
            }
            return await base.ExecuteAsync(workflowInstance);
        }
    }
}