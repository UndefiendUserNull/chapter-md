namespace ChapterMD.Core;

internal static class TemplateParser
{
    public static SortedDictionary<int, ChapterProperties> Parse(string templateFile)
    {
        if (string.IsNullOrWhiteSpace(templateFile))
            throw new ArgumentException(
                "Template file path is required.",
                nameof(templateFile));

        var result = new SortedDictionary<int, ChapterProperties>();
        string[] lines = File.ReadAllLines(templateFile);

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex].Trim();

            if (line.Length == 0 ||
                line.StartsWith("#", StringComparison.Ordinal) ||
                line.StartsWith("//", StringComparison.Ordinal))
            {
                continue;
            }

            var parsed = ParseTemplateLine(line, lineIndex + 1);

            foreach (int chapterIndex in parsed.Indices)
            {
                if (chapterIndex < 0)
                {
                    throw new FormatException(
                        $"Line {lineIndex + 1}: chapter index cannot be negative: '{line}'");
                }

                result[chapterIndex] = parsed.Properties;
            }
        }

        return result;
    }

    private static (IReadOnlyList<int> Indices, ChapterProperties Properties)
        ParseTemplateLine(string line, int lineNumber)
    {
        int colonIndex = line.IndexOf(':');

        if (colonIndex < 0)
            throw new FormatException(
                $"Line {lineNumber}: no colon was found in '{line}'.");

        string indexPart = line[..colonIndex].Trim();
        string propertiesPart = line[(colonIndex + 1)..].Trim();

        IReadOnlyList<int> indices = ParseIndices(
            indexPart,
            line,
            lineNumber);

        ChapterProperties properties = ParseProperties(
            propertiesPart,
            line,
            lineNumber);

        return (indices, properties);
    }

    private static IReadOnlyList<int> ParseIndices(
        string indexPart,
        string line,
        int lineNumber)
    {
        if (indexPart.StartsWith("[", StringComparison.Ordinal))
        {
            var range = Utils.ParseRange(line);

            if (range.Max < range.Min)
                throw new FormatException(
                    $"Line {lineNumber}: invalid range in '{line}'.");

            return Enumerable
                .Range(range.Min, range.Max - range.Min + 1)
                .ToArray();
        }

        string[] indexTokens = indexPart.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (indexTokens.Length == 0)
            throw new FormatException(
                $"Line {lineNumber}: no chapter index found in '{line}'.");

        var indices = new List<int>(indexTokens.Length);

        foreach (string token in indexTokens)
        {
            if (!int.TryParse(token, out int index))
            {
                throw new FormatException(
                    $"Line {lineNumber}: invalid chapter index '{token}' in '{line}'.");
            }

            indices.Add(index);
        }

        return indices;
    }

    private static ChapterProperties ParseProperties(
        string propertiesPart,
        string line,
        int lineNumber)
    {
        int parts = 1;
        int startFrom = 1;
        bool isMarked = false;

        string[] tokens = propertiesPart.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        for (int i = 0; i < tokens.Length; i++)
        {
            switch (tokens[i])
            {
                case "marked":
                    isMarked = true;
                    break;

                case "from":
                    if (i + 1 >= tokens.Length ||
                        !int.TryParse(tokens[++i], out startFrom))
                    {
                        throw new FormatException(
                            $"Line {lineNumber}: expected a number after 'from' in '{line}'.");
                    }

                    if (startFrom < 0)
                    {
                        throw new FormatException(
                            $"Line {lineNumber}: 'from' cannot be negative in '{line}'.");
                    }

                    break;

                case "parts":
                    // Supports both "parts 3" and legacy "3 parts".
                    if (i + 1 < tokens.Length &&
                        int.TryParse(tokens[i + 1], out parts))
                    {
                        i++;
                    }
                    else if (i > 0 &&
                             int.TryParse(tokens[i - 1], out parts))
                    {
                        // Already read as the previous token.
                    }
                    else
                    {
                        throw new FormatException(
                            $"Line {lineNumber}: expected a number before or after 'parts' in '{line}'.");
                    }

                    if (parts < 0)
                    {
                        throw new FormatException(
                            $"Line {lineNumber}: 'parts' cannot be negative in '{line}'.");
                    }

                    break;

                default:
                    // Legacy syntax: "3 parts"
                    if (i + 1 < tokens.Length &&
                        tokens[i + 1] == "parts" &&
                        int.TryParse(tokens[i], out int legacyParts))
                    {
                        parts = legacyParts;
                        i++; // skip "parts"
                    }

                    // Unknown tokens are ignored to preserve old behavior.
                    break;
            }
        }

        return new ChapterProperties(parts, startFrom, isMarked, "Chapter");
    }
}