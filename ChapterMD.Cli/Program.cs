using ChapterMD.Core;
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
        errs => { ReportParseResult(errs); return 1; });
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
    static int ReportParseResult(IEnumerable<Error> errs)
    {
        var errors = errs.ToList();

        foreach (var e in errors)
        {
            switch (e)
            {
                case HelpRequestedError:
                case VersionRequestedError:
                    return 0;

                case HelpVerbRequestedError hve when hve.Matched:
                    return 0;

                case HelpVerbRequestedError hve:
                    System.Console.Error.WriteLine($"Unknown help topic: '{hve.Verb}'.");
                    return 2;

                case BadFormatConversionError badFmt:
                    System.Console.Error.WriteLine(
                        $"Option '{badFmt.NameInfo.NameText}' has an invalid value.");
                    break;

                case UnknownOptionError unknown:
                    System.Console.Error.WriteLine($"'{unknown.Token}' is not a recognized option.");
                    break;

                case MissingRequiredOptionError req:
                    System.Console.Error.WriteLine($"Required option '{req.NameInfo.NameText}' is missing.");
                    break;

                case MissingValueOptionError noVal:
                    System.Console.Error.WriteLine($"Option '{noVal.NameInfo.NameText}' requires a value.");
                    break;

                case RepeatedOptionError dup:
                    System.Console.Error.WriteLine($"Option '{dup.NameInfo.NameText}' was specified more than once.");
                    break;

                case SetValueExceptionError setEx:
                    System.Console.Error.WriteLine($"Option '{setEx.NameInfo.NameText}' failed: {setEx.Exception.Message}");
                    break;

                default:
                    System.Console.Error.WriteLine($"Error: {e.Tag}");
                    break;
            }
        }

        System.Console.Error.WriteLine("Try '--help' for usage.");
        return 2;
    }
}