namespace ChapterMD.Core;

public partial class WriterOptions
{
    public string FileName { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public int ChaptersAmount { get; set; }
    public int SubChaptersAmount { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public int StartFrom { get; set; }
    public int StartSubFrom { get; set; }
    public bool Append { get; set; }
    public bool FreshAppend { get; set; }
}
