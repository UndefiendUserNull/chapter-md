namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    private static readonly UConsole Console = new();
    public static void WriteChapterFile(WriterOptions options)
    {
        Console.Verbose = options.Verbose;
        Console.SkipPressToContinue = options.SkipConfirm;

        bool append = !options.Overwrite;

        HandleConflicts(options);


        try
        {
            var finalPath = Path.Combine(options.Output, options.FileName);
            var dir = Path.GetDirectoryName(finalPath);
            int finalStartFrom = options.StartFrom;
            int finalChaptersAmount = options.ChaptersAmount;

            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (Path.Exists(finalPath) && append && !options.FreshAppend)
            {
                AppendExistingFile(finalPath, ref finalStartFrom, ref finalChaptersAmount, options.StartFrom);
            }

            if (options.Unmark)
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

            using var writer = new StreamWriter(finalPath, append: append || options.FreshAppend);

            for (int i = 0; i < finalChaptersAmount; i++)
            {
                string styledChapterNumber = Utils.ConvertDecimalToNumberType(i + finalStartFrom, options.NumberingType);

                writer.WriteLine($"- [{GetMarkedString(i, options.MarkedFrom, options.Marked)}] {options.Title} {styledChapterNumber}");

                if (options.SubChaptersAmount > 0)
                {
                    for (int j = options.StartSubFrom; j < options.SubChaptersAmount; j++)
                    {
                        string styledSubChapterNumber = Utils.ConvertDecimalToNumberType(j, options.NumberingType);
                        switch (options.SubChapterStyleType)
                        {
                            case SubChapterStyleType.XAndY:
                                writer.WriteLine($"\t- [{GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle} {styledChapterNumber}.{styledSubChapterNumber}");
                                break;
                            case SubChapterStyleType.XOnly:
                                writer.WriteLine($"\t- [{GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle} {styledChapterNumber}");
                                break;
                            case SubChapterStyleType.YOnly:
                                writer.WriteLine($"\t- [{GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle} {styledSubChapterNumber}");
                                break;
                            case SubChapterStyleType.None:
                                writer.WriteLine($"\t- [{GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle}");
                                break;
                        }
                    }
                }
            }

            Console.WriteLine($"Generated {options.FileName} at {finalPath}.");
        }
        catch (Exception)
        {
            // TODO: Handle common IO errors
            throw;
        }
    }
    public static void AppendExistingFile(string finalPath, ref int finalStartFrom, ref int finalChaptersAmount, int startFrom)
    {
        Console.WriteVerboseLine($"File at {finalPath} already exists and option append is used.");
        string[] data = File.ReadAllLines(finalPath);

        if (data.Length == 0)
        {
            Console.WriteVerboseLine("File found was empty.");
            return;
        }

        var lastChapter = data.Last(x => !x.StartsWith('\t')).Split(' ');


        try
        {
            int parsed = Utils.RevertNumberTypeToDecimal(lastChapter[lastChapter.Length - 1]);

            finalStartFrom = parsed + 1;
            Console.WriteVerboseLine($"Last chapter found = {finalStartFrom}");
        }
        catch
        {
            Console.WriteLine($"Couldn't find the last chapter in {finalPath}, using fresh append instead.");
            finalStartFrom = startFrom;
            Console.WriteVerboseLine($"Start from didn't change ({startFrom})");
        }
        Console.WriteVerboseLine($"Start from = {finalStartFrom}");
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

    private static string? GetMarkedString(int i, int markedFrom, bool useMarked)
    {
        if (!useMarked) return null;

        return i >= markedFrom ? "X" : " ";
    }
}
