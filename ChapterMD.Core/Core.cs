namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(
        string fileName,
        string path,
        int chaptersAmount,
        int subChaptersAmount,
        string title,
        string subTitle,
        int startFrom,
        int startSubFrom,
        bool append,
        bool freshAppend)
    {
        if (startFrom > chaptersAmount) throw new Exception("The start from index is bigger than the chapters amount.");

        if (startSubFrom > subChaptersAmount) throw new Exception("The sub start from index is bigger than the sub chapters amount.");

        if (startFrom < 0 || startSubFrom < 0 || subChaptersAmount < 0 || chaptersAmount < 0)
        {
            throw new IndexOutOfRangeException("Negative number found in (" +
            $"startFrom: {startFrom}, startSubFrom: {startSubFrom}, subChaptersAmount: {subChaptersAmount}, chaptersAmount: {chaptersAmount}).");
        }

        try
        {
            var finalPath = Path.Combine(path, fileName);
            var dir = Path.GetDirectoryName(finalPath);
            int finalStartFrom = startFrom;
            int finalChaptersAmount = chaptersAmount;

            if (!string.IsNullOrEmpty(dir))
            {
                Console.WriteLine($"Creating {dir}");
                Directory.CreateDirectory(dir);
            }

            if (Path.Exists(finalPath) && append)
            {
                AppendExistingFile(finalPath, ref finalStartFrom, ref finalChaptersAmount, startFrom);
            }

            using var writer = new StreamWriter(finalPath, append: append);

            for (int i = finalStartFrom; i <= finalChaptersAmount; i++)
            {
                writer.WriteLine($"- [ ] {title} {i}");
                if (subChaptersAmount > 0)
                {
                    for (int j = startSubFrom; j <= subChaptersAmount; j++)
                    {
                        writer.WriteLine($"\t- [ ] {subTitle} {i}.{j}");
                    }
                }
            }
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
