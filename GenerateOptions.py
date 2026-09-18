import os


cli_options = "./ChapterMD.Cli/Options.cs"
core_options = "./ChapterMD.Core/WriterOptions.cs"
utils = "./ChapterMD.Cli/Utils.cs"
options_text = "./options.chmd"
unprepared_dict = dict()
prepared_dict = dict()

generated_cli_options = "./ChapterMD.Cli/Generated/Options.g.cs"
generated_utils = "./ChapterMD.Cli/Generated/Options.g.cs"
generated_core_options = "./ChapterMD.Core/Generated/Options.g.cs"

file_opened = open(options_text).read().rstrip().lstrip().splitlines()

for i in range(0, file_opened.__len__()):
    if file_opened[i].startswith('#'): continue
    
    if file_opened[i].startswith('['):
        header = file_opened[i]
        variable = file_opened[i+1] 
        
        unprepared_dict[header]= variable;

for k, v in unprepared_dict.items():
    attribute_clean = str(k).removeprefix('[').removesuffix(']').split(',')
    command = attribute_clean[0]
    help_text = attribute_clean[1]
    
    vs = str(v)
    
    half_split_index = vs.index(" = ")
    
    default = vs.replace(' ', '')[half_split_index:]

    first_half = vs[0:half_split_index]
    last_half = vs[half_split_index:]
    
    attribute = f"[Option({command}, Default = {default}, HelpText ={help_text})]"
    variable_code = f"public {first_half}" + " {get; set;}" + last_half + ';\n\n'
    
    prepared_dict[attribute] = variable_code
        

os.makedirs(os.path.dirname(generated_cli_options), exist_ok=True)
os.makedirs(os.path.dirname(generated_core_options), exist_ok=True)

with open(generated_cli_options, "w+") as f:
    f.write("""using CommandLine;

namespace ChapterMD.Cli;

public partial class Options
{
""")
    for k, v in prepared_dict.items():
        f.write(f"\t{k}\n\t{v}")
    f.write("}")
for k, v in prepared_dict.items():
    print(f"{k}: {v}")