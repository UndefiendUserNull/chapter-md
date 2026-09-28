namespace ChapterMD.Core;

internal sealed class ChapterLineFormatter(WriterOptions p_options)
{
    private readonly WriterOptions _options = p_options;

    public void WriteChapter(TextWriter writer, int chapterNumber, string mark)
    {
        string styledChapterNumber = FormatNumber(chapterNumber);

        writer.WriteLine(
            $"- [{mark}] {_options.Title} {styledChapterNumber}");
    }

    public void WriteSubChapter(
        TextWriter writer,
        int chapterNumber,
        int subChapterNumber,
        string mark)
    {
        string styledChapterNumber = FormatNumber(chapterNumber);
        string styledSubChapterNumber = FormatNumber(subChapterNumber);

        string ending = GetSubChapterEnding(
            styledChapterNumber,
            styledSubChapterNumber);

        writer.WriteLine(
            $"\t- [{mark}] {_options.SubTitle} {ending}");
    }

    private string FormatNumber(int number)
    {
        return Utils.ConvertDecimalToNumberType(
            number,
            _options.NumberingStyle);
    }

    private string GetSubChapterEnding(
        string styledChapterNumber,
        string styledSubChapterNumber)
    {
        return _options.SubChapterStyleType switch
        {
            SubChapterStyleType.XAndY =>
                $"{styledChapterNumber}.{styledSubChapterNumber}",

            SubChapterStyleType.XOnly =>
                styledChapterNumber,

            SubChapterStyleType.YOnly =>
                styledSubChapterNumber,

            SubChapterStyleType.None =>
                string.Empty,

            _ => throw new ArgumentOutOfRangeException(
                "SubChapterStyleType",
                _options.SubChapterStyleType,
                "Unknown sub-chapter style.")
        };
    }
}