namespace ChapterMD.Core;

internal static class ChapterUnmarker
{
    public static void Unmark(string finalPath)
    {
        if (!File.Exists(finalPath))
            return;

        string text = File.ReadAllText(finalPath);

        File.WriteAllText(
            finalPath,
            text.Replace("[X]", "[ ]", StringComparison.Ordinal));
    }
}