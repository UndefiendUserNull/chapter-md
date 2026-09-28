namespace ChapterMD.Core;

public static class ChapterFileWriter
{
    public static void WriteChapterFile(WriterOptions options, TextWriter? writer = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        ChapterWriteValidator.ValidateAndNormalize(options);

        string finalPath = Path.Combine(options.Output, options.FileName);
        EnsureOutputDirectory(finalPath);

        if (options.templateFile != string.Empty)
        {
            TemplateHandler.WriteChapterFileFromTemplate(options, finalPath, writer!);
            return;
        }

        if (options.Unmark)
        {
            ChapterUnmarker.Unmark(finalPath);
            return;
        }

        bool append = !options.Overwrite;
        int finalStartFrom = options.StartFrom;
        int finalChaptersAmount = options.ChaptersAmount;

        if (File.Exists(finalPath) && append && !options.FreshAppend)
        {
            Utils.UpdateWritingValuesForAppend(
                finalPath,
                ref finalStartFrom,
                ref finalChaptersAmount,
                options.StartFrom);
        }

        ChapterMarkdownGenerator.Generate(
            options,
            finalPath,
            append || options.FreshAppend,
            finalStartFrom,
            finalChaptersAmount,
            writer!);
    }

    private static void EnsureOutputDirectory(string finalPath)
    {
        string? directory = Path.GetDirectoryName(finalPath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
    }
}