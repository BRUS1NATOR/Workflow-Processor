using CommandLine;

namespace WorkflowProcessor.Tool.Options
{
    [Verb("json", HelpText = "Generate json schemes for workflows.")]
    class JsonOptions
    {
        [Option(
          Default = false,
          HelpText = "Project path.",
          Required = true)]
        public string Project { get; set; }

        [Option("output",
          HelpText = "Output directory for json files.",
          Required = false)]
        public string? Output { get; set; }
    }


    //[Verb("html", HelpText = "Generate html from json workflows.")]
    //class HtmlOptions
    //{
    //    [Option(
    //      Default = false,
    //      HelpText = "Folder path.",
    //      Required = true)]
    //    public string Folder { get; set; }

    //    [Option("output",
    //      HelpText = "Output directory for html files.",
    //      Required = false)]
    //    public string? Output { get; set; }
    //}
}
