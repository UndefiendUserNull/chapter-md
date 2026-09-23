using Spectre.Console;
using Spectre.Console.Cli;

namespace ChapterMD.Cli;

public static class Program
{
    static int Main(string[] args)
    {
        var app = new CommandApp<RunCommand>();
        app.Configure(c => c.SetApplicationName("chaptermd"));

        try
        {
            return app.Run(args);
        }
        catch (CommandRuntimeException ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] {ex.Message}");
            return 1;
        }

    }

    private sealed class RunCommand : Command<Options>
    {
        protected override int Execute(CommandContext context, Options settings, CancellationToken cancellationToken)
        {
            return RunOptions(settings);
        }
    }

    private static int RunOptions(Options opts)
    {
        var ogOut = Console.Out;
        var bridge = new UConsoleTextWriter(ogOut);

        if (opts.FileName != string.Empty && opts.ChaptersAmount > 0)
        {
            //Console.WriteVerboseLine($"Writing {opts.ChaptersAmount} chapters in {opts.FileName} and {opts.SubChaptersAmount} Sub-Chapters ...");
            Core.ChapterFileWriter.WriteChapterFile(Utils.ToWriterOptions(opts), bridge);
        }

        return 0;
    }

}