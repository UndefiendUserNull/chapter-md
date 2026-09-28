namespace ChapterMD.Core;

public static class ChapterWriteValidator
{
    public static void ValidateAndNormalize(WriterOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.FileName)) throw new ArgumentException("File name is required.", nameof(options));

        if (options.StartFrom > options.ChaptersAmount) throw new InvalidOperationException("The start from index is bigger than the chapters amount.");

        if (options.StartSubFrom > options.SubChaptersAmount) throw new InvalidOperationException("The sub start from index is bigger than the sub chapters amount.");

        if (options.StartFrom < 0 ||
            options.StartSubFrom < 0 ||
            options.SubChaptersAmount < 0 ||
            options.ChaptersAmount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                $"Negative number found in " +
                $"(startFrom: {options.StartFrom}, " +
                $"startSubFrom: {options.StartSubFrom}, " +
                $"subChaptersAmount: {options.SubChaptersAmount}, " +
                $"chaptersAmount: {options.ChaptersAmount}).");
        }

        if (!options.FileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            options.FileName += ".md";
    }
}