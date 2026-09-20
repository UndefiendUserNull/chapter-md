
using ChapterMD.Core;

namespace ChapterMD.Cli;

public static partial class Utils
{
    public static WriterOptions ToWriterOptions(Options options)
    {
        return new WriterOptions
        {
			FileName = options.FileName,
			Verbose = options.Verbose,
			SubChaptersAmount = options.SubChaptersAmount,
			ChaptersAmount = options.ChaptersAmount,
			Title = options.Title,
			SubTitle = options.SubTitle,
			StartFrom = options.StartFrom,
			StartSubFrom = options.StartSubFrom,
			Append = options.Append,
			FreshAppend = options.FreshAppend,
			Output = options.Output,
			Overwrite = options.Overwrite,
			SkipConfirm = options.SkipConfirm,
			Marked = options.Marked,
			MarkedFrom = options.MarkedFrom,
			SubMarked = options.SubMarked,
			SubMarkedFrom = options.SubMarkedFrom,
			Unmark = options.Unmark,
			TabSize = options.TabSize,
			NumberingType = options.NumberingType,

            };
        }
    }

    