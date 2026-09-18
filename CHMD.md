# CHMD

CHMD is a small DSL for declaring CLI options, which a generator script expands into C# source files.

## Format

Each entry is two lines, separated by a blank line:

    [<flag>, <help text>]
    <type> <Identifier> = <default>

#### Example:

```cs
["name", "File name"]
string FileName = "Chapters.md"
```

## Generated output

Options.g.cs | attributes for the CLI side:

```cs
[Option("name", Default = "Chapters.md", HelpText = "File name")]
public string FileName { get; set; } = "Chapters.md";
```

WriterOptions.g.cs | plain properties for the core side:

```cs
public string FileName { get; set; } = "Chapters.md";
```

## How to use:

1.  Add your options in "options.chmd", or create one, for example i will put these two.

    `options.chmd` :

```cs
["name", "File name"]
string FileName = "Chapters.md"

["verbose", "Prints all messages to standard output."]
bool Verbose = false
```

2. Run `py ./GenerateOptions.py`, Files will be generated in each project "Generated" folder.

**Beware that the generated files gets overwritten every time the generation runs, so don't change anything inside the generated files.**
