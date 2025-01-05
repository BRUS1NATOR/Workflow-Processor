namespace WorkflowProcessor.Core.WorkflowElement
{
    public class WorkflowElementMetadata
    {
        public string ElementDisplayName { get; set; }
    }

    public class WorkflowElementMetadataBuilder
    {
        public string ElementDisplayName { get; set; }
        public WorkflowElementMetadata GetMetadata()
        {
            return new WorkflowElementMetadata()
            {
                ElementDisplayName = ElementDisplayName
            };
        }

        public WorkflowElementMetadataBuilder WithName(string name)
        {
            ElementDisplayName = name;
            return this;
        }
    }
}
