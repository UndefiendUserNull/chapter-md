namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(WriterOptions options)
    {
        bool append = !options.Overwrite;

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

        try
        {
            var finalPath = Path.Combine(options.Output, options.FileName);
            var dir = Path.GetDirectoryName(finalPath);
            int finalStartFrom = options.StartFrom;
            int finalChaptersAmount = options.ChaptersAmount;
            string markedStr = options.Marked ? "X" : " ";

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

                    lines = sr.ReadToEnd().Split("\r\t");

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
                    wr.WriteLine(line);
                }

                return;
            }

            using var writer = new StreamWriter(finalPath, append: append || options.FreshAppend);

            for (int i = finalStartFrom; i <= finalChaptersAmount; i++)
            {
                string styledChapterNumber = Utils.ConvertDecimalToNumberType(i, options.NumberingType);

                if (options.Marked && i >= options.MarkedFrom)
                    writer.WriteLine($"- [{markedStr}] {options.Title} {styledChapterNumber}");
                else
                    writer.WriteLine($"- [ ] {options.Title} {styledChapterNumber}");

                if (options.SubChaptersAmount > 0)
                {
                    for (int j = options.StartSubFrom; j <= options.SubChaptersAmount; j++)
                    {
                        string styledSubChapterNumber = Utils.ConvertDecimalToNumberType(j, options.NumberingType);

                        if (options.SubMarked && j >= options.SubMarkedFrom)
                            writer.WriteLine($"\t- [{markedStr}] {options.SubTitle} {styledSubChapterNumber}");
                        else
                            writer.WriteLine($"\t- [ ] {options.SubTitle} {styledSubChapterNumber}");
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
        Console.WriteLine($"File at {finalPath} already exists and option append is used.");
        string[] data = File.ReadAllLines(finalPath);

        if (data.Length == 0)
        {
            Console.WriteLine("File found was empty.");
            return;
        }

        var lastChapter = data.Last(x => !x.StartsWith('\t')).Split(' ');

        if (int.TryParse(lastChapter[lastChapter.Length - 1], out int parsed))
        {
            finalStartFrom = parsed + 1;
            finalChaptersAmount += finalChaptersAmount;
            Console.WriteLine($"Last chapter found = {finalStartFrom}");
        }
        else
        {
            finalStartFrom = startFrom;
            Console.WriteLine($"Start from didn't change ({startFrom})");
        }
        Console.WriteLine($"Start from = {finalStartFrom}");
    }
}
