using CommandLine;

namespace ChapterMD.Cli;

public static class Program
{
    private readonly static UConsole Console = new();

    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No args provided, generating default template.");
        }
        return Parser.Default.ParseArguments<Options>(args)
        .MapResult(
        opts => { RunOptions(opts); return 0; },
        errs => { HandleParseError(errs); return 1; });
    }
    static void RunOptions(Options opts)
    {
        Console.Verbose = opts.Verbose;
        Console.SkipPressToContinue = opts.SkipConfirm;

        if (opts.FileName != string.Empty && opts.ChaptersAmount > 0)
        {
            Console.WriteVerboseLine($"Writing {opts.ChaptersAmount} chapters in {opts.FileName} and {opts.SubChaptersAmount} Sub-Chapters ...");
            Core.ChapterFileWriter.WriteChapterFile(Utils.ToWriterOptions(opts));
        }

        Console.PressToContinue();
    }
    static void HandleParseError(IEnumerable<Error> errs)
    {
        using var writer = new StreamWriter("chmd_errors.log", append: false);
        foreach (var item in errs)
        {
            writer.WriteLine(item);
        }

    }

}