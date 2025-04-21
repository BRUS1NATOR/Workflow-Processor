using System.Text.Json.Serialization;
using WorkflowProcessor.Activities.Gateways;
using WorkflowProcessor.Core.Connections;
using WorkflowProcessor.Core.Step;

namespace WorkflowProcessor.Core
{
    public class WorkflowScheme
    {
        /// <summary>
        /// Elements (steps)
        /// </summary>
        [JsonPropertyName("elements")]
        public List<WorkflowStep> Elements { get; set; } = new();

        [JsonPropertyName("connection")]
        public List<Connection> Connections { get; set; } = new();

        [JsonPropertyName("start")]
        public WorkflowStep Start => Elements.FirstOrDefault(x => x.ActivityTypeName == "StartActivity" || x.ActivityTypeName == "StartActivityGeneric")!;

        [JsonPropertyName("end")]
        public WorkflowStep End => Elements.FirstOrDefault(x => x.ActivityTypeName == "EndActivity" || x.ActivityTypeName == "EndActivityGeneric")!;

        public WorkflowScheme()
        {
        }

        public WorkflowScheme(List<WorkflowStep> elements, List<Connection> conntections)
        {
            Elements = elements;
            Connections = conntections;
        }

        public WorkflowScheme Init()
        {
            Validate();

            int i = 0;
            foreach (var element in Elements)
            {
                if (string.IsNullOrEmpty(element.StepId))
                {
                    element.StepId = $"{element.ActivityTypeName}_{i++.ToString().PadLeft(2, '0')}";
                    continue;
                }
            }
            return this;
        }

        public WorkflowStep? GetNextStep(WorkflowStep currentElement)
        {
            return Connections.FirstOrDefault(x => x.Source == currentElement)?.Target;
        }

        public IEnumerable<Connection> GetOutgoingConnections(string stepId)
        {
            return GetOutgoingConnections(Elements.First(x => x.StepId == stepId));
        }

        public IEnumerable<Connection> GetOutgoingConnections(WorkflowStep? currentElement)
        {
            if (currentElement is null)
            {
                return Enumerable.Empty<Connection>();
            }
            return Connections.Where(x => x.Source == currentElement);
        }

        #region Validation

        const string ValidationError = "Не удалось инициализировать процесс";
        private void Validate()
        {
            var message = ValidateStartStep();
            if (!string.IsNullOrEmpty(message))
            {
                throw new Exception($"{ValidationError}: {message}");
            }
            message = ValidateEndStep();
            if (!string.IsNullOrEmpty(message))
            {
                throw new Exception($"{ValidationError}: Найдены дублирующие ID у переменных");
            }
            //message = ValidateConnections();
            //if (!string.IsNullOrEmpty(message))
            //{
            //    throw new Exception($"{ValidationError}: Найдены дублирующие ID у переменных");
            //}
        }

        private string ValidateStartStep()
        {
            if (Start is null)
            {
                return "Start activity not found!";
            }
            var outputConnections = Connections.Count(x => x.Source == Start);
            if (outputConnections == 0)
            {
                return "Start activity doesnt have any output connections!";
            }
            if (outputConnections > 1)
            {
                return "Start activity has more than one output connection";
            }
            return string.Empty;
        }

        private string ValidateEndStep()
        {
            if (End is null)
            {
                return "End activity not found!";
            }
            var inputConnection = Connections.Count(x => x.Target == End);
            if (inputConnection == 0)
            {
                return "Start activity doesnt have any input connections!";
            }
            return string.Empty;
        }

        #endregion Validation


        public List<WorkflowStep> GetIncomingSteps(WorkflowStep step)
        {
            return Connections.Where(x => x.Target.StepId == step.StepId).Select(x => x.Source).ToList();
        }

        #region Parallel

        public List<WorkflowStep> GetActivitiesInParallelBlock(string closeParallelActivityId)
        {
            var startParallelStep = Elements.FirstOrDefault(x => x.StepId == closeParallelActivityId);
            var steps = new List<WorkflowStep>() { startParallelStep };

            Loop(steps[0], steps);

            return steps;
        }

        private List<WorkflowStep> Loop(WorkflowStep currentStep, List<WorkflowStep> result)
        {
            for (var i = 0; i < result.Count; i++)
            {
                var prevSteps = Connections.Where(x => x.Target.StepId == result[i].StepId).Select(x => x.Source).ToList();
                for (var j = 0; j < prevSteps.Count; j++)
                {
                    var prevStep = prevSteps[j];
                    //TODO
                    if (result.Contains(prevStep) || prevStep.StepId.StartsWith("ParallelExclusiveGateway"))
                    {
                        continue;
                    }
                    result.Add(prevStep);
                    Loop(prevStep, result);
                }
            }

            return result;
        }
        #endregion
    }
}