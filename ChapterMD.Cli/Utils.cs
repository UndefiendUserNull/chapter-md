using ChapterMD.Core;

namespace ChapterMD.Cli;

public static class Utils
{
    public static WriterOptions ToWriterOptions(Options options)
    {
        return new WriterOptions
        {
            FileName = options.FileName,
            Path = options.Path,
            ChaptersAmount = options.ChaptersAmount,
            SubChaptersAmount = options.SubChaptersAmount,
            Title = options.Title,
            SubTitle = options.SubTitle,
            StartFrom = options.StartFrom,
            StartSubFrom = options.StartSubFrom,
            Append = options.Append,
            FreshAppend = options.FreshAppend,
        };
    }
}
