# options.txt Syntax

Options are defined in `./tools/options.txt`.  
Each option is a block of [Spectre.Console](https://github.com/spectreconsole/spectre.console) attributes followed by the variable declaration.

## Basic format

```txt
[CommandOption("--option-name")]
[Description("Description text.")]
[DefaultValue(default)]
Type PropertyName = default
```

- `[CommandOption("--option-name")]` — CLI flag.
- `[Description("...")]` — help text.
- `[DefaultValue(...)]` — default value shown by the CLI.
- `Type PropertyName = default` — property type, name, and initial value.
  - **No semicolon needed.**

## Example from code

```txt
[CommandOption("--file-name")]
[Description("File name")]
[DefaultValue("Chapters.md")]
string FileName = "Chapters.md"
```

## Notes

- Comments starts with a `#`.
- Attributes are copied to the generated `ChapterMD.Cli/Options.g.cs`.
- For enums, use the full type and value:

```txt
[CommandOption("--numbering-style")]
[Description("Numbering style.")]
[DefaultValue(ChapterMD.Core.NumberingStyle.English)]
ChapterMD.Core.NumberingStyle NumberingStyle = ChapterMD.Core.NumberingStyle.English
```

After adding an option, run the generator.  
It re-writes `ChapterMD.Cli/Generated/Options.g.cs`, `ChapterMD.Core/Generated/WriterOptions.g.cs`, and `ChapterMD.Cli/Generated/Utils.g.cs` with the new options.  
Do not edit the generated files manually.
