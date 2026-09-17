using CommandLine;

namespace ChapterMD.Cli;

public static class Program
{
    class Options
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
        [Option('o', "outpur", Default = ".", HelpText = "Where the file will be saved.")]
        public string Path { get; set; } = ".";
    }

    private readonly static UConsole Console = new();

    static int Main(string[] args)
    {
        return Parser.Default.ParseArguments<Options>(args)
        .MapResult(
        opts => { RunOptions(opts); return 0; },
        errs => { HandleParseError(errs); return 1; });
    }
    static void RunOptions(Options opts)
    {
        Console.SetVerbose(opts.Verbose);
        if (opts.FileName != string.Empty && opts.ChaptersAmount > 0)
        {
            Console.WriteVerboseLine($"Writing {opts.ChaptersAmount} chapters in {opts.FileName} and {opts.SubChaptersAmount} Sub-Chapters ...");
            Core.ChapterFileWriter.WriteChapterFile(
                opts.FileName,
                opts.Path,
                opts.ChaptersAmount,
                opts.SubChaptersAmount,
                opts.Title,
                opts.SubTitle,
                opts.StartFrom,
                opts.StartSubFrom
                );
        }

    }
    static void HandleParseError(IEnumerable<Error> errs)
    {
        Console.WriteLine("You didn't provide enough arguments.");
        using var writer = new StreamWriter("chmd_errors.log", append: false);
        foreach (var item in errs)
        {
            writer.WriteLine(item);
        }

    }

}