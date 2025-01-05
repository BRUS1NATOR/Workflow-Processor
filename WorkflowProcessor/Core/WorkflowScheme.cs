using WorkflowProcessor.Core.Connections;

namespace WorkflowProcessor.Core
{
    public class WorkflowScheme
    {
        public List<WorkflowStep> Elements = new();
        public List<Connection> Connections = new();

        public WorkflowStep Start => Elements.FirstOrDefault(x => x.ActivityType == "StartActivity")!;
        public WorkflowStep End => Elements.FirstOrDefault(x => x.ActivityType == "EndActivity")!;

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
                if(string.IsNullOrEmpty(element.ActivityId))
                {
                    element.Id = $"{element.ActivityType}{i++.ToString().PadLeft(2,'0')}";
                    continue;
                }
                element.Id = element.ActivityId;
            }
            return this;
        }

        public WorkflowStep GetStartStep()
        {
            return Elements.First(x => x.ActivityType == "StartActivity");
        }
        public WorkflowStep? GetNextStep(WorkflowStep currentElement)
        {
            return Connections.FirstOrDefault(x => x.Source == currentElement)?.Target;
        }

        public IEnumerable<Connection> GetOutgoingConnections(string stepId)
        {
            return GetOutgoingConnections(Elements.First(x => x.Id == stepId));
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
                throw new Exception($"{ValidationError}: Найдены дублирующие ID у переменных");
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
                return $"Начальное событие не найдено!";
            }
            var outputConnections = Connections.Count(x => x.Source == Start);
            if (outputConnections == 0)
            {
                return $"Начальное событие не имеет исходящих переходов!";
            }
            if (outputConnections > 1)
            {
                return $"Начальное событие не имеет исходящих переходов!";
            }
            return string.Empty;
        }

        private string ValidateEndStep()
        {
            if (End is null)
            {
                return $"Конечное событие не найдено!";
            }
            var inputConnection = Connections.Count(x => x.Target == End);
            if (inputConnection == 0)
            {
                return $"Конечное событие не имеет входящих переходов!";
            }
            return string.Empty;
        }

        #endregion Validation
    }
}