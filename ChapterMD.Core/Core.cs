namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(string path, int chaptersAmount)
    {
        using var writer = new StreamWriter(path, append: false);
        for (int i = 1; i <= chaptersAmount; i++)
        {
            writer.WriteLine($"- [ ] Chapter {i}");
        }
    }
}
