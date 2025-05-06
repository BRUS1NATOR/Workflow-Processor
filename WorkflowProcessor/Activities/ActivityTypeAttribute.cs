namespace WorkflowProcessor.Activities
{
    public class ActivityTypeAttribute : Attribute
    {
        public BaseAcitivityType ActivityType { get; set; }
        public ActivityTypeAttribute(BaseAcitivityType type) { ActivityType = type; }
    }
}
