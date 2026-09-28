namespace ChapterMD.Core;

public static class TemplateHandler
{
    public static void WriteChapterFileFromTemplate(
        WriterOptions options,
        string fullPath,
        TextWriter log)
    {
        var chapters = TemplateParser.Parse(options.templateFile);

        using var writer = new StreamWriter(fullPath);
        var formatter = new ChapterLineFormatter(options);

        foreach (var (chapterNumber, properties) in chapters)
        {
            string mark = Utils.GetMarkedString(properties.IsMarked);

            formatter.WriteChapter(writer, chapterNumber, mark);

            if (properties.Parts <= 0)
                continue;

            for (int j = 0; j < properties.Parts; j++)
            {
                int subChapterNumber = j + properties.StartFrom;

                // Original template handler used the same mark for all sub-chapters.
                formatter.WriteSubChapter(
                    writer,
                    chapterNumber,
                    subChapterNumber,
                    mark);
            }
        }

        log.WriteLine(ConsoleWritingUtils.Style(
            $"Generated {options.FileName} at {fullPath}.",
            ConsoleWritingUtils.MessageType.INFO));
    }
}