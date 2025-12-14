using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.Connections.Metadata;
using WorkflowProcessor.Persistance.Context;
using WorkflowProcessor.Persistance.Context.Json;

namespace WorkflowProcessor.Console.Examples
{
    [PolymorphicContext(typeof(ApprovementData), "ApprovementData")]
    public class ApprovementData : IContextData
    {
        public int? DocumentId { get; set; }
        public bool IsGroup1Approved { get; set; } = false;
        public bool IsGroup2Approved { get; set; } = false;
    }

    public class ApprovementExample : WorkflowBuilder<ApprovementData>
    {
        public ApprovementExample()
        {
            Name = "Example_Approvement";
            Version = 1;
            IsAllowedToRunFromWeb = true;
        }

        public override Workflow Build()
        {
            var start = StepStart((x) =>
            {
                x.SetWorkflowInstanceName("Approvement process");
            });
            //
            var userEditDocument = StepCode((activity) =>
            {
                activity.Code(x =>
                {
                    x.Data.DocumentId = x.Data.DocumentId == null ? new Random().Next(0, 100) : x.Data.DocumentId;
                });
            });

            var isGroup1Approved = Step<If<ApprovementData>>((activity) => { activity.SetCondition(x => x.Data.IsGroup1Approved); });
            var isGroup2Approved = Step<If<ApprovementData>>((activity) => { activity.SetCondition(x => x.Data.IsGroup2Approved); });

            var approvement1 = Step<UserActivity<ApprovementData>>(x =>
            {
                x.SetBookmarkName(c => c.Data.DocumentId + " test");
                x.AddUser(9250);
                x.AddUser(9251);
            }).WithId("approvementTask1");

            var approvement2 = Step<UserActivity<ApprovementData>>(x =>
            {
                x.SetBookmarkName("Approvement by user group 2");
                x.AddUser(1);
                x.AddUser(2);
            }).WithId("approvementTask2");

            var endActivity = StepEnd(x => { System.Console.WriteLine("Document has been approved!"); });

            Scheme.Connections = new List<Connection>()
                {
                    new Connection(start, userEditDocument),
                    new Connection(userEditDocument, isGroup1Approved),
                    //
                        new ConditionalConnection<ApprovementData, bool>(isGroup1Approved, isGroup2Approved, true),
                        new ConditionalConnection<ApprovementData, bool>(isGroup1Approved, approvement1, false),
                            new UserConnection<ApprovementData>(approvement1, isGroup2Approved, "approved", new ConnectionMetadata("Согласовать", "approved")),
                            new UserConnection<ApprovementData>(approvement1, userEditDocument, "disapproved", new ConnectionMetadata("Отказать", "disapproved")),
                            new UserConnection<ApprovementData>(approvement1, endActivity, "cancel", new ConnectionMetadata("Прервать", "cancel")),
                    //
                        new ConditionalConnection<ApprovementData, bool>(isGroup2Approved, endActivity, true),
                        new ConditionalConnection<ApprovementData, bool>(isGroup2Approved, approvement2, false),
                            new UserConnection<ApprovementData>(approvement2, endActivity, "approved"),
                            new UserConnection<ApprovementData>(approvement2, userEditDocument, "disapproved"),
                };

            return base.Build();
        }
    }
}