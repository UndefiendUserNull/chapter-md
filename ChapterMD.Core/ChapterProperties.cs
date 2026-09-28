namespace ChapterMD.Core;

public struct ChapterProperties(int Parts, int StartFrom, bool IsMarked, string Name)
{
    public int Parts { get; set; } = Parts;
    public int StartFrom { get; set; } = StartFrom;
    public bool IsMarked { get; set; } = IsMarked;
    public string Name { get; set; } = Name;

    public override string ToString() => $"Parts: {Parts}\nStartFrom: {StartFrom}\nIsMarked: {IsMarked}";
}
