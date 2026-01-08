using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Locator;
using System.Reflection;
using WorkflowProcessor.Core;
using WorkflowProcessor.Tools;

namespace WorkflowProcessor;

internal class Program
{
    static void Main(string[] args)
    {
        var projectDirectory = GetProjectPath(args);
        string? csprojFile;
        //test projectDirectory = "S:\\Workflow-Processor\\WorkflowProcessor.Tests";
        //
        if (projectDirectory is null)
        {
            projectDirectory = Directory.GetCurrentDirectory();
        }
        csprojFile = Directory
            .GetFiles(projectDirectory, "*.csproj", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();

        if (csprojFile is null)
            throw new Exception($"No .csproj found in {projectDirectory} directory");


        var instance = MSBuildLocator.RegisterDefaults();
        Console.WriteLine($"Using MSBuild at: {instance.MSBuildPath}");

        BuildProject(csprojFile);

        var assembly = GetAssembly(csprojFile);

        foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(WorkflowBuilder))))
        {
            var wfBuilderInstance = (WorkflowBuilder)Activator.CreateInstance(type)!;
            FileCreator.CreateJson(wfBuilderInstance);
        }
    }

    private static Assembly GetAssembly(string csprojFile)
    {
        var project = ProjectRootElement.Open(csprojFile);
        
        var targetFramework = project.Properties.First(x => x.Name == "TargetFramework");
        var assemblyPath = Path.Combine(project.DirectoryPath, "bin", "Release", targetFramework.Value, Path.GetFileNameWithoutExtension(csprojFile) + ".dll");
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly;
    }

    private static void BuildProject(string csprojFile)
    {
        var project = new Project(csprojFile);
        var result = project.Build();

        if (!result)
        {
            throw new Exception("Build failed.");
        }
    }

    private static string? GetProjectPath(string[] args)
    {
        return args
            .SkipWhile(a => a != "--project")
            .Skip(1)
            .FirstOrDefault();
    }
}