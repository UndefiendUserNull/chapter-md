namespace ChapterMD.Core;

public static class TemplateHandler
{
    private static Dictionary<int, ChapterProperties> ParseTemplate(string templateFile)
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
            if (int.TryParse(block.Split(' ')[0], out var chapterIndex))
            {
                if (chapterIndex < 0)
                    throw new Exception($"Start from cannot be negative '{block}'");

                theThing[chapterIndex] = ParseLine(block);
            }

            // Individual Groups
            {
                var indexOfColon = block.IndexOf(':') < 0 ? throw new Exception($"No colon was found '{block}'") : block.IndexOf(':');
                var safeToParseBlock = block[..indexOfColon].Trim().Replace(" ", string.Empty);
                if (Utils.StartsWithDigit().IsMatch(safeToParseBlock))
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

        return theThing;
    }

    public static void WriteChapterFileFromTemplate(WriterOptions options, string fullPath)
    {
        var theThing = ParseTemplate(options.templateFile);

        using var writer = new StreamWriter(fullPath);

        foreach (var chapter in theThing)
        {
            string styledChapterNumber = Utils.ConvertDecimalToNumberType(chapter.Key, options.NumberingStyle);

            writer.WriteLine($"- [{Utils.GetMarkedString(chapter.Value.IsMarked)}] {options.Title} {styledChapterNumber}");

            if (chapter.Value.Parts > 0)
            {
                for (int j = 0; j < chapter.Value.Parts; j++)
                {
                    string styledSubChapterNumber = Utils.ConvertDecimalToNumberType(j + chapter.Value.StartFrom, options.NumberingStyle);
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
                    writer.WriteLine($"\t- [{Utils.GetMarkedString(chapter.Value.IsMarked)}] {options.SubTitle} {subChapterEnding}");
                }
            }
        }

        Console.WriteLine(ConsoleWritingUtils.Style($"Generated {options.FileName} at {"."}.", ConsoleWritingUtils.MessageType.INFO));


    }

    private static ChapterProperties ParseLine(string line)
    {
        bool partsFound = false, startFromFound = false, markedFound = false;

        var indexOfColon = line.Replace(" ", string.Empty).IndexOf(':') < 0 ? throw new Exception($"No colon was found '{line}'") : line.Replace(" ", string.Empty).IndexOf(':');
        string[] props = line[(indexOfColon + 1)..].Split(' ');

        ChapterProperties result = new(1, 1, false);

        for (int i = 0; i < props.Length; i++)
        {
            if (props[i] == "marked")
            {
                result.IsMarked = true;
                markedFound = true;
            }

            else if (props[i] == "from")
            {
                if (int.TryParse(props[i + 1], out var parsedStartFrom))
                {
                    if (parsedStartFrom < 0)
                        throw new Exception($"Start from cannot be negative '{line}'"); // TODO: Highlight syntax error (line.replace(error))

                    result.StartFrom = parsedStartFrom;
                    startFromFound = true;
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
                    partsFound = true;
                }
                else
                {
                    throw new Exception($"Invalid string before 'parts' {line}");
                }
            }
            else
            {
                if (!markedFound)
                    result.IsMarked = false;
                if (!partsFound)
                    result.Parts = 1;
                if (!startFromFound)
                    result.StartFrom = 1;
            }


        }

        return result;
    }
}
