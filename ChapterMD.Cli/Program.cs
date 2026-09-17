using CommandLine;

namespace ChapterMD.Cli;

public static class Program
{
    class Options
    {
        [Option('m', "name", Required = true, Default = ".", HelpText = "File name.")]
        public string FileName { get; set; } = string.Empty;

        [Option('v', "verbose",
          Default = false,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('c', "amount", HelpText = "The amount of chapters inside the file.")]
        public int ChaptersAmount { get; set; }
    }

    private readonly static UConsole Console = new();

    static int Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions)
          .WithNotParsed(HandleParseError);

        return 0;
    }
    static void RunOptions(Options opts)
    {
        Console.SetVerbose(opts.Verbose);
        if (opts.FileName != string.Empty && opts.ChaptersAmount > 0)
        {
            Console.WriteVerboseLine($"Writing {opts.ChaptersAmount} chapters in {opts.FileName} ...");
            Core.ChapterFileWriter.WriteChapterFile(opts.FileName, opts.ChaptersAmount);
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