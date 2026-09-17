namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(string path, int chaptersAmount, int subChaptersAmount)
    {
        using var writer = new StreamWriter(path, append: false);
        for (int i = 1; i <= chaptersAmount; i++)
        {
            writer.WriteLine($"- [ ] Chapter {i}");
            if (subChaptersAmount > 0)
            {
                for (int j = 1; j < subChaptersAmount; j++)
                {
                    writer.WriteLine($"\t- [ ] Part {i}.{j}");
                }
            }
        }
    }
}
