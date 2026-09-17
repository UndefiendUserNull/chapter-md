namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(
        string path,
        int chaptersAmount,
        int subChaptersAmount,
        string title,
        string subTitle,
        int startFrom,
        int startSubFrom)
    {
        if (startFrom > chaptersAmount)
        {
            throw new Exception("The start from index is bigger than the chapters amount.");
        }

        if (startSubFrom > subChaptersAmount)
        {
            throw new Exception("The sub start from index is bigger than the sub chapters amount.");
        }

        try
        {
            using var writer = new StreamWriter(path, append: false);
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
