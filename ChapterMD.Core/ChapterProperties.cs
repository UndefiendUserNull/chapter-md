namespace ChapterMD.Core;

public struct ChapterProperties(int Parts, int StartFrom, bool IsMarked)
{
    public int Parts { get; set; } = Parts;
    public int StartFrom { get; set; } = StartFrom;
    public bool IsMarked { get; set; } = IsMarked;

    public override string ToString() => $"Parts: {Parts}\nStartFrom: {StartFrom}\nIsMarked: {IsMarked}";
}
