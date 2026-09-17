using CommandLine;

namespace ChapterMD.Cli;

public static class Program
{
    class Options
    {
        [Option('m', "name", Required = true, Default = ".", HelpText = "File name.")]
        public string FileName { get; set; } = string.Empty;

        [Option(
          Default = false,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Value(5, MetaName = "amount", HelpText = "The amount of chapters inside the file.")]
        public int ChaptersAmount { get; set; }
    }

    static int Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
          .WithParsed(RunOptions)
          .WithNotParsed(HandleParseError);

        return 0;
    }
    static void RunOptions(Options opts)
    {
        if (opts.FileName != string.Empty && opts.ChaptersAmount > 0)
        {
            Core.ChapterFileWriter.WriteChapterFile(opts.FileName, opts.ChaptersAmount);
        }
    }
    static void HandleParseError(IEnumerable<Error> errs)
    {
        foreach (var item in errs)
        {
            throw new Exception(item.ToString());
        }
    }

}