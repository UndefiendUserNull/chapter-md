using Spectre.Console;
using System.Text;

namespace ChapterMD.Cli;

public sealed class UConsoleTextWriter : TextWriter
{
    private readonly StringBuilder _buffer = new();
    private readonly object _lock = new();
    private readonly TextWriter _originalOut;
    private readonly IAnsiConsole _ansi;

    public UConsoleTextWriter(TextWriter originalConsoleOut)
    {
        _originalOut = originalConsoleOut;

        _ansi = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(originalConsoleOut),
            Ansi = AnsiSupport.Detect,
            ColorSystem = ColorSystemSupport.Detect,
            Interactive = InteractionSupport.Detect
        });
    }

    public override Encoding Encoding => _originalOut.Encoding;

    public override void Write(char value)
    {
        lock (_lock)
        {
            if (value == '\n')
            {
                FlushLine();
            }
            else if (value != '\r')
            {
                _buffer.Append(value);
            }
        }
    }

    public override void Write(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        foreach (var ch in value)
            Write(ch);
    }

    public override void WriteLine(string? value)
    {
        Write(value);
        Write('\n');
    }

    public override void WriteLine()
    {
        Write('\n');
    }

    private void FlushLine()
    {
        var line = _buffer.ToString();
        _buffer.Clear();

        if (line.StartsWith("[INFO]: ", StringComparison.Ordinal))
        {
            _ansi.MarkupLine($"[Gray100]{line.Replace("[INFO]: ", "").Replace("[/]", "")}[/]");
            return;
        }
        if (line.StartsWith("[WARNING]: ", StringComparison.Ordinal))
        {
            _ansi.MarkupLine($"[Gold1]{line.Replace("[WARNING]: ", "").Replace("[/]", "")}[/]");
            return;
        }
        if (line.StartsWith("[ERROR]: ", StringComparison.Ordinal))
        {
            _ansi.MarkupLine($"[Red]{line.Replace("[ERROR]: ", "").Replace("[/]", "")}[/]");
            return;
        }
        else
        {
            _ansi.WriteLine(line);
        }
    }
}