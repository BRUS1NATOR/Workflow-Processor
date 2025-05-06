namespace WorkflowProcessor.Activities
{
    public enum BaseAcitivityType
    {
        StartActivity = 1, EndActivity = 2,
        Gateway = 10, ParallelGatewayStart = 11, ParallelGatewayEnd = 12,
        BlockingActivity = 20, SleepActivity = 21, LogActivity = 22, CodeActivity = 23, UserActivity = 24,
        SubprocessActivity = 30, Unknown = 99
    }
}
