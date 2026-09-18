using CommandLine;

namespace ChapterMD.Cli;

public class Options
{
    [Option('m', "name", Default = "Chapters.md", HelpText = "File name.")]
    public string FileName { get; set; } = "Chapters.md";

    [Option('v', "verbose",
      Default = false,
      HelpText = "Prints all messages to standard output.")]
    public bool Verbose { get; set; } = false;

    [Option('c', "amount", Default = 5, HelpText = "The amount of chapters inside the file..")]
    public int ChaptersAmount { get; set; } = 5;

    [Option('s', "sub-amount", Default = 5, HelpText = "The amount of sub-chapters inside each chapter.")]
    public int SubChaptersAmount { get; set; } = 5;
    [Option('t', "title", Default = "Chapter", HelpText = "Custom title name instead of Chapter X")]
    public string Title { get; set; } = "Chapter";

    [Option("sub-title", Default = "Part", HelpText = "Custom sub-title name instead of Part X.Y")]
    public string SubTitle { get; set; } = "Part";
    [Option('f', "start-from", Default = 1)]
    public int StartFrom { get; set; } = 1;
    [Option('g', "start-sub-from", Default = 1)]
    public int StartSubFrom { get; set; } = 1;
    [Option('a', "append", Default = true, HelpText = "Adds new lines to existing chapters file continuing from the last chapter.")]
    public bool Append { get; set; } = false;
    [Option('r', "fresh-append", Default = false, HelpText = "Adds new lines to existing chapters file starting from (StartFrom).")]
    public bool FreshAppend { get; set; } = false;

    [Option('o', "output", Default = ".", HelpText = "Where the file will be saved.")]
    public string Path { get; set; } = ".";
}
