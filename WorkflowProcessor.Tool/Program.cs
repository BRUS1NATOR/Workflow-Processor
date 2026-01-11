using CommandLine;
using Microsoft.Build.Locator;
using WorkflowProcessor.Core;
using WorkflowProcessor.Tool;
using WorkflowProcessor.Tool.Options;
using WorkflowProcessor.Tools;

namespace WorkflowProcessor;

internal class Program
{
    static void Main(string[] args)
    {
        // args = ["--project", "S:\\Workflow-Processor\\WorkflowProcessor.Tests"];
        var result = Parser.Default.ParseArguments<JsonOptions>(args)
           .MapResult(
             (JsonOptions opts) => CreateJsonFiles(opts),
             errs => 1);
    }

    private static int CreateJsonFiles(JsonOptions opts)
    {
        var projectDirectory = opts.Project;
        var outputDirectory = opts.Output;

        string? csprojFile;
        //
        if (projectDirectory is null)
        {
            projectDirectory = Directory.GetCurrentDirectory();
        }
        if (outputDirectory is null)
        {
            outputDirectory = Path.Combine(projectDirectory, "Schemes");
            if (!Directory.Exists(outputDirectory)) 
            { 
                Directory.CreateDirectory(outputDirectory);
            }
        }
        csprojFile = Directory
            .GetFiles(projectDirectory, "*.csproj", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();

        if (csprojFile is null)
            throw new Exception($"No .csproj found in {projectDirectory} directory");


        var instance = MSBuildLocator.RegisterDefaults();
        Console.WriteLine($"Using MSBuild at: {instance.MSBuildPath}");

        ProjectCompiler.BuildProject(csprojFile);

        var assembly = ProjectCompiler.GetAssembly(csprojFile);

        foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(WorkflowBuilder))))
        {
            var wfBuilderInstance = (WorkflowBuilder)Activator.CreateInstance(type)!;
            FileCreator.CreateFile(wfBuilderInstance, Path.Combine(outputDirectory, $"{wfBuilderInstance.Name}.{wfBuilderInstance.Version}.json"));
        }

        return 1;
    }
}