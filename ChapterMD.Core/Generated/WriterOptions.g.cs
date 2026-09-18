namespace ChapterMD.Core;

public partial class WriterOptions
{
	public string FileName {get; set;} = "Chapters.md";

	public bool Verbose {get; set;} = false;

	public int SubChaptersAmount {get; set;} = 5;

	public int ChaptersAmount {get; set;} = 5;

	public string Title {get; set;} = "Chapter";

	public string SubTitle {get; set;} = "Part";

	public int StartFrom {get; set;} = 1;

	public int StartSubFrom {get; set;} = 1;

	public bool Append {get; set;} = true;

	public bool FreshAppend {get; set;} = false;

	public string Output {get; set;} = ".";

	public bool Overwrite {get; set;} = false;

}