namespace ChapterMD.Cli;

public class UConsole(bool verbose = false, bool skip = false)
{
    public bool Verbose { get; set; } = verbose;
    public bool SkipPressToContinue { get; set; } = skip;

    public void WriteVerboseLine(object msg)
    {
        if (Verbose) WriteLine(msg);
    }

    public void WriteLine(object msg)
    {
        Console.WriteLine(msg);
    }

    public void PressToContinue()
    {
        if (SkipPressToContinue) return;
        WriteLine("Press any key to continue ...");
        Console.ReadKey(true);
    }
}
