namespace ChapterMD.Core;

internal static class ChapterMarkdownGenerator
{
    public static void Generate(
        WriterOptions options,
        string finalPath,
        bool append,
        int startFrom,
        int chaptersAmount,
        TextWriter consoleWriter)
    {
        using var writer = new StreamWriter(finalPath, append: append);
        var formatter = new ChapterLineFormatter(options);

        for (int i = 0; i < chaptersAmount; i++)
        {
            int chapterNumber = i + startFrom;
            string chapterMark = Utils.GetMarkedString(
                i,
                options.MarkedFrom,
                options.Marked);

            formatter.WriteChapter(writer, chapterNumber, chapterMark);

            if (options.SubChaptersAmount <= 0)
                continue;

            for (int j = 0; j < options.SubChaptersAmount; j++)
            {
                int subChapterNumber = j + options.StartSubFrom;

                string subChapterMark = Utils.GetMarkedString(
                    i,
                    options.SubMarkedFrom,
                    options.SubMarked);

                formatter.WriteSubChapter(
                    writer,
                    chapterNumber,
                    subChapterNumber,
                    subChapterMark);
            }
        }

        consoleWriter.WriteLine(ConsoleWritingUtils.Style(
            $"Generated {options.FileName} at {finalPath}.",
            ConsoleWritingUtils.MessageType.INFO));
    }
}