using System.Text.Json;
using WorkflowProcessor.Core;

namespace WorkflowProcessor.Tools
{
    internal class FileCreator
    {
        public static void CreateJson(WorkflowBuilder workflowBuilder)
        {
            string jsonString = JsonSerializer.Serialize(workflowBuilder.Build(), new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
            Console.WriteLine(jsonString);
        }
    }
}
