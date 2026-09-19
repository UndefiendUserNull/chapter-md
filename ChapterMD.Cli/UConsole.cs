namespace ChapterMD.Cli;

public class UConsole(bool verbose = false)
{
    private bool _verbose = verbose;
    private bool _skipPressToContinue = false;

    public void WriteVerboseLine(object msg)
    {
        if (_verbose) WriteLine(msg);
    }

    public void WriteLine(object msg)
    {
        Console.WriteLine(msg);
    }

    public void PressToContinue()
    {
        if (_skipPressToContinue) return;
        WriteLine("Press any key to continue ...");
        Console.ReadKey(true);
    }

    public void SetVerbose(bool value) => _verbose = value;
}
