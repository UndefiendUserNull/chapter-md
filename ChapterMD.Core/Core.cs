namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(WriterOptions options, TextWriter writer)
    {
        bool append = !options.Overwrite;
        var oldOut = Console.Out;

        Console.SetOut(writer);

        HandleConflicts(options);

        var finalPath = Path.Combine(options.Output, options.FileName);
        var dir = Path.GetDirectoryName(finalPath);
        int finalStartFrom = options.StartFrom;
        int finalChaptersAmount = options.ChaptersAmount;

        try
        {
            try
            {
                if (options.templateFile != string.Empty)
                {
                    TemplateHandler.WriteChapterFileFromTemplate(options, finalPath);
                    return;
                }

                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (Path.Exists(finalPath) && append && !options.FreshAppend)
                {
                    Utils.UpdateWritingValuesForAppend(finalPath, ref finalStartFrom, ref finalChaptersAmount, options.StartFrom);
                }

                if (options.Unmark)
                {
                    Unmark(finalPath);
                    return;
                }

                Write(finalPath, append, finalChaptersAmount, finalStartFrom, options);

            }
            catch (Exception)
            {
                // TODO: Handle common IO errors
                throw;
            }
        }
        finally
        {
            Console.SetOut(oldOut);
        }
    }



    private static void Write(string finalPath, bool append, int finalChaptersAmount, int finalStartFrom, WriterOptions options)
    {
        using var writer = new StreamWriter(finalPath, append: append || options.FreshAppend);

        for (int i = 0; i < finalChaptersAmount; i++)
        {
            string styledChapterNumber = Utils.ConvertDecimalToNumberType(i + finalStartFrom, options.NumberingStyle);

            writer.WriteLine($"- [{Utils.GetMarkedString(i, options.MarkedFrom, options.Marked)}] {options.Title} {styledChapterNumber}");

            if (options.SubChaptersAmount > 0)
            {
                for (int j = 0; j < options.SubChaptersAmount; j++)
                {
                    string styledSubChapterNumber = Utils.ConvertDecimalToNumberType(j + options.StartSubFrom, options.NumberingStyle);
                    string subChapterEnding = string.Empty;

                    switch (options.SubChapterStyleType)
                    {
                        case SubChapterStyleType.XAndY:
                            subChapterEnding = $"{styledChapterNumber}.{styledSubChapterNumber}";
                            break;
                        case SubChapterStyleType.XOnly:
                            subChapterEnding = $"{styledChapterNumber}";
                            break;
                        case SubChapterStyleType.YOnly:
                            subChapterEnding = $"{styledSubChapterNumber}";
                            break;
                        case SubChapterStyleType.None:
                            subChapterEnding = string.Empty;
                            break;
                    }
                    writer.WriteLine($"\t- [{Utils.GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle} {subChapterEnding}");
                }
            }
        }

        Console.WriteLine(ConsoleWritingUtils.Style($"Generated {options.FileName} at {finalPath}.", ConsoleWritingUtils.MessageType.INFO));

    }

    private static void Unmark(string finalPath)
    {
        string[] lines = [];

        {
            using StreamReader sr = new(finalPath);

            lines = sr.ReadToEnd().Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains($"[X]"))
                {
                    lines[i] = lines[i].Replace("[X]", "[ ]");
                }
            }
        }

        using var wr = new StreamWriter(finalPath);


        foreach (var line in lines)
        {
            wr.Write(line);
        }

        return;
    }

    private static void HandleConflicts(WriterOptions options)
    {
        if (options.StartFrom > options.ChaptersAmount)
            throw new Exception("The start from index is bigger than the chapters amount.");

        if (options.StartSubFrom > options.SubChaptersAmount)
            throw new Exception("The sub start from index is bigger than the sub chapters amount.");

        if (options.StartFrom < 0 || options.StartSubFrom < 0 ||
            options.SubChaptersAmount < 0 || options.ChaptersAmount < 0)
        {
            throw new IndexOutOfRangeException("Negative number found in (" +
                $"startFrom: {options.StartFrom}, startSubFrom: {options.StartSubFrom}, " +
                $"subChaptersAmount: {options.SubChaptersAmount}, chaptersAmount: {options.ChaptersAmount}).");
        }

        if (!options.FileName.EndsWith(".md")) options.FileName += ".md";
    }
}
