namespace ChapterMD.Cli;

public class UConsole(bool verbose = false)
{
    private bool _verbose = verbose;

    public void WriteVerboseLine(object msg)
    {
        if (_verbose) WriteLine(msg);
    }

    public void WriteLine(object msg)
    {
        Console.WriteLine(msg);
    }

    public void SetVerbose(bool value) => _verbose = value;
}
