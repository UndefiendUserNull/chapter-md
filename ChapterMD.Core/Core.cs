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
        int startSubFrom)
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
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            using var writer = new StreamWriter(finalPath, append: false);

            for (int i = startFrom; i <= chaptersAmount; i++)
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
}
