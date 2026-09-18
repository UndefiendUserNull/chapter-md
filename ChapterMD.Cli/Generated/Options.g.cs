using CommandLine;
namespace ChapterMD.Cli;

public partial class Options
{
	[Option("command-name", Default = "Default", HelpText = "Help Text")]
	public string VariableName {get; set;} = "Default";

}