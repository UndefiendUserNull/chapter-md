namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(string path, int chaptersAmount, int subChaptersAmount, string title, string subTitle)
    {
        using var writer = new StreamWriter(path, append: false);
        for (int i = 1; i <= chaptersAmount; i++)
        {
            writer.WriteLine($"- [ ] {title} {i}");
            if (subChaptersAmount > 0)
            {
                for (int j = 1; j < subChaptersAmount; j++)
                {
                    writer.WriteLine($"\t- [ ] {subTitle} {i}.{j}");
                }
            }
        }
    }
}
