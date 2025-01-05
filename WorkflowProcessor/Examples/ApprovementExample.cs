using WorkflowProcessor.Activities;
using WorkflowProcessor.Activities.Basic;
using WorkflowProcessor.Core;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Persistance.Context;
namespace MassTransitExample.Examples
{
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
            Name = "Approvement";
            Version = 1;
        }

        public override Workflow Build()
        {
            var start = Step<StartActivity>();
            //
            var userEditDocument = Step<CodeActivity<ApprovementData>>((activity) =>
            {
                activity.Code(x =>
                {
                    x.DocumentId = x.DocumentId == null ? new Random().Next(0, 100) : x.DocumentId;
                });
            });

            var isGroup1Approved = Step<If<ApprovementData>>((activity) => { activity.SetCondition(x => x.IsGroup1Approved); });
            var isGroup2Approved = Step<If<ApprovementData>>((activity) => { activity.SetCondition(x => x.IsGroup2Approved); });

            var approvement1 = Step<UserActivity>(x =>
            {
                //x.SetName("Согласование документа групой 1");
                x.SetUserId(9250);
            }).WithId("approvementTask1");

            var approvement2 = Step<UserActivity>(x =>
            {
                //x.SetName("Согласование документа групой 2");
                x.SetUserId(9250);
            }).WithId("approvementTask2");

            var endActivity = Step<EndActivity>(x => { Console.WriteLine("Документ согласован!"); });

            Connections = new List<Connection>()
                {
                    new Connection(start, userEditDocument),
                    new Connection(userEditDocument, isGroup1Approved),
                    //
                        new ConditionalConnection<ApprovementData, bool>(isGroup1Approved, isGroup2Approved, true),
                        new ConditionalConnection<ApprovementData, bool>(isGroup1Approved, approvement1, false),
                            new ConditionalConnection<string>(approvement1, isGroup2Approved, "approved"),
                            new ConditionalConnection<string>(approvement1, userEditDocument, "disapproved"),
                            new ConditionalConnection<string>(approvement1, endActivity, "cancel"),
                    //
                        new ConditionalConnection<ApprovementData, bool>(isGroup2Approved, endActivity, true),
                        new ConditionalConnection<ApprovementData, bool>(isGroup2Approved, approvement2, false),
                            new ConditionalConnection<string>(approvement2, endActivity, "approved"),
                            new ConditionalConnection<string>(approvement2, userEditDocument, "disapproved"),
                };

            return base.Build();
        }
    }
}