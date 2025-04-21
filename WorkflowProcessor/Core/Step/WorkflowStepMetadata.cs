namespace WorkflowProcessor.Core.Step
{
    public class WorkflowStepMetadata
    {
        public string ElementDisplayName { get; set; }
    }

    //public class WorkflowElementMetadataBuilder
    //{
    //    public string ElementDisplayName { get; set; }
    //    public WorkflowStepMetadata GetMetadata()
    //    {
    //        return new WorkflowStepMetadata()
    //        {
    //            ElementDisplayName = ElementDisplayName
    //        };
    //    }

    //    public WorkflowElementMetadataBuilder WithName(string name)
    //    {
    //        ElementDisplayName = name;
    //        return this;
    //    }
    //}
}
