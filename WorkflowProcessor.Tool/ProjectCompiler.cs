using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using System.Reflection;

namespace WorkflowProcessor.Tool
{
    internal class ProjectCompiler
    {

        public static void BuildProject(string csprojFile)
        {
            var project = new Project(csprojFile);
            var result = project.Build();

            if (!result)
            {
                throw new Exception("Build failed.");
            }
        }

        public static Assembly GetAssembly(string csprojFile)
        {
            var project = ProjectRootElement.Open(csprojFile);

            var targetFramework = project.Properties.First(x => x.Name == "TargetFramework");
            var assemblyPath = Path.Combine(project.DirectoryPath, "bin", "Debug", targetFramework.Value, Path.GetFileNameWithoutExtension(csprojFile) + ".dll");
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
