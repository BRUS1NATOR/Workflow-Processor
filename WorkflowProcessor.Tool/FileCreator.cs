using System.Text.Json;
using WorkflowProcessor.Core;

namespace WorkflowProcessor.Tools
{
    internal class FileCreator
    {
        public static string CreateJson(WorkflowBuilder workflowBuilder)
        {
            return JsonSerializer.Serialize(workflowBuilder.Build(), new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
        }


        public static void CreateFile(WorkflowBuilder workflowBuilder, string filePath, string? fileName = null)
        {
            var json = CreateJson(workflowBuilder);

            using (var stream = File.CreateText(filePath))
            {
                stream.Write(json);
            }
        }
    }
}
