using System.Text.RegularExpressions;

namespace ChapterMD.Core;



public static class ChapterFileWriter
{
    public static void WriteChapterFile(WriterOptions options, TextWriter writer)
    {
        bool append = !options.Overwrite;
        var oldOut = Console.Out;

        Console.SetOut(writer);

        if (options.templateFile != string.Empty)
        {
            ParseTemplate(options.templateFile);
        }

        HandleConflicts(options);

        var finalPath = Path.Combine(options.Output, options.FileName);
        var dir = Path.GetDirectoryName(finalPath);
        int finalStartFrom = options.StartFrom;
        int finalChaptersAmount = options.ChaptersAmount;

        try
        {
            try
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (Path.Exists(finalPath) && append && !options.FreshAppend)
                {
                    UpdateWritingValuesForAppend(finalPath, ref finalStartFrom, ref finalChaptersAmount, options.StartFrom);
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

    private static void ParseTemplate(string templateFile)
    {
        string[] data = File.ReadAllLines(templateFile);
        Dictionary<int, ChapterProperties> theThing = [];

        foreach (var block in data)
        {
            // Range rules
            if (block.StartsWith('['))
            {
                var rangeFound = Utils.ParseRange(block);
                for (int i = rangeFound.Min; i <= rangeFound.Max; i++)
                {
                    theThing[i] = ParseLine(block);
                }
            }

            // Individual
            if (int.TryParse(block[0].ToString(), out var chapterIndex))
            {
                if (chapterIndex < 0)
                    throw new Exception($"Start from cannot be negative '{block}'");

                theThing[chapterIndex] = ParseLine(block);
            }

            // Individual Groups
            {
                var safeToParseBlock = block[..block.IndexOf(':')].Trim().Replace(" ", string.Empty);
                if (Regex.IsMatch(safeToParseBlock, @"^\d,"))
                {
                    try
                    {
                        var group = safeToParseBlock.Split(',').Select(x => int.Parse(x));
                        foreach (var i in group)
                        {
                            theThing[i] = ParseLine(block);
                        }
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidCastException($"Error while casting block '{safeToParseBlock}' containing NaN.");
                    }
                }
            }

        }

        foreach (var item in theThing)
        {

            Console.WriteLine(item);
        }
    }

    private static ChapterProperties ParseLine(string line)
    {
        string[] props = (line[(line.IndexOf(':') + 2)..]).Split(' ');
        ChapterProperties result = new(0, 0, false);

        for (int i = 0; i < props.Length; i++)
        {
            if (props[i] == "marked")
            {
                result.IsMarked = true;
            }
            else if (props[i] == "from")
            {
                if (int.TryParse(props[i + 1], out var parsedStartFrom))
                {
                    if (parsedStartFrom < 0)
                        throw new Exception($"Start from cannot be negative '{line}'"); // TODO: Highlight syntax error (line.replace(error))
                    result.StartFrom = parsedStartFrom;
                }
                else
                {
                    throw new Exception($"Invalid string after 'from' {line}");
                }
            }
            else if (props[i] == "parts")
            {
                if (int.TryParse(props[i - 1], out var partsCount))
                {
                    if (partsCount < 0)
                        throw new Exception($"Parts count cannot be negative '{line}'"); // TODO: Highlight syntax error (line.replace(error))

                    result.Parts = partsCount;
                }
                else
                {
                    throw new Exception($"Invalid string before 'parts' {line}");
                }
            }
        }

        return result;
    }

    private static void Write(string finalPath, bool append, int finalChaptersAmount, int finalStartFrom, WriterOptions options)
    {
        using var writer = new StreamWriter(finalPath, append: append || options.FreshAppend);

        for (int i = 0; i < finalChaptersAmount; i++)
        {
            string styledChapterNumber = Utils.ConvertDecimalToNumberType(i + finalStartFrom, options.NumberingStyle);

            writer.WriteLine($"- [{GetMarkedString(i, options.MarkedFrom, options.Marked)}] {options.Title} {styledChapterNumber}");

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
                    writer.WriteLine($"\t- [{GetMarkedString(i, options.SubMarkedFrom, options.SubMarked)}] {options.SubTitle} {subChapterEnding}");
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

    public static void UpdateWritingValuesForAppend(string finalPath, ref int finalStartFrom, ref int finalChaptersAmount, int startFrom)
    {
        Console.WriteLine(ConsoleWritingUtils.Style($"File at {finalPath} already exists and option append is used.", ConsoleWritingUtils.MessageType.WARNING));
        string[] data = File.ReadAllLines(finalPath);

        if (data.Length == 0)
        {
            Console.WriteLine(ConsoleWritingUtils.Style("File found was empty.", ConsoleWritingUtils.MessageType.ERROR));
            return;
        }

        var lastChapter = data.Last(x => !x.StartsWith('\t')).Split(' ');


        try
        {
            int parsed = Utils.RevertNumberTypeToDecimal(lastChapter[lastChapter.Length - 1]);

            finalStartFrom = parsed + 1;
            Console.WriteLine(ConsoleWritingUtils.Style($"Last chapter found = {finalStartFrom}", ConsoleWritingUtils.MessageType.INFO));
        }
        catch
        {
            Console.WriteLine(ConsoleWritingUtils.Style($"Couldn't find the last chapter in {finalPath}, using fresh append instead.", ConsoleWritingUtils.MessageType.ERROR));
            finalStartFrom = startFrom;
        }
        Console.WriteLine(ConsoleWritingUtils.Style($"Start from = {finalStartFrom}", ConsoleWritingUtils.MessageType.INFO));
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

    private static string GetMarkedString(int i, int markedFrom, bool useMarked)
    {
        if (!useMarked) return " ";

        return i >= markedFrom ? "X" : " ";
    }
}
