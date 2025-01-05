using Microsoft.EntityFrameworkCore;
using WorkflowProcessor.Core;

namespace WorkflowProcessor.Services
{
    public class WorkflowInstanceFactory
    {
        private readonly WorkflowContext _dbContext;
        private readonly WorkflowStorage _workflowStorage;

        public WorkflowInstanceFactory(WorkflowContext dbContext, WorkflowStorage workflowStorage)
        {
            _dbContext = dbContext;
            _workflowStorage = workflowStorage;
        }

        public async Task<IWorkflowInstance> GetWorkflowInstance(int id)
        {
            var workflowInstance = await _dbContext.WorkflowInstances.Include(x => x.Workflow).FirstOrDefaultAsync(x => x.Id == id);
            InitializeWorkflow(workflowInstance);

            return workflowInstance;
        }

        public WorkflowInstance? GetWorkflowInstance(WorkflowBookmark bookmark)
        {
            if(bookmark == null || bookmark.WorkflowExecutionPoint == null || bookmark.WorkflowExecutionPoint.WorkflowInstance == null)
            {
                return null;
            }
            var workflowInstance = bookmark.WorkflowExecutionPoint.WorkflowInstance;

            InitializeWorkflow(workflowInstance);

            return workflowInstance;
        }

        private void InitializeWorkflow(IWorkflowInstance workflowInstance)
        {
            var workflow = _workflowStorage.GetWorkflow(workflowInstance);

            workflowInstance.Workflow = workflow;
            workflowInstance.Context = workflow.ContextData;
            if (workflowInstance is null)
            {
                throw new Exception("Workflow not found!");
            }
            workflowInstance.Context.SetContextValueFromJson(workflowInstance.JsonData);
        }
    }
}
