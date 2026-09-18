import os

OPTIONS_FILE_PATH = "./options.chmd"

unprepared_dict = dict()
prepared_dict = dict()

GENERATED_CLI_OPTIONS_PATH = "./ChapterMD.Cli/Generated/Options.g.cs"
GENERATED_UTILS_PATH = "./ChapterMD.Cli/Generated/Utils.g.cs"
GENERATED_CORE_OPTIONS_PATH = "./ChapterMD.Core/Generated/WriterOptions.g.cs"

file_opened = open(OPTIONS_FILE_PATH).read().rstrip().lstrip().splitlines()

def read_unprepared():
    print(f"Reading {OPTIONS_FILE_PATH} ...")
    for i in range(0, file_opened.__len__()):
        if file_opened[i].startswith('#'): continue
        
        if file_opened[i].startswith('['):
            header = file_opened[i]
            variable = file_opened[i+1] 
            
            unprepared_dict[header]= variable

def prepare_dict():
    print("Setting up final dictionary ...")
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


def create_dirs():
    print("Creating Generated dirs ...")
    os.makedirs(os.path.dirname(GENERATED_CLI_OPTIONS_PATH), exist_ok=True)
    os.makedirs(os.path.dirname(GENERATED_CORE_OPTIONS_PATH), exist_ok=True)
    os.makedirs(os.path.dirname(GENERATED_UTILS_PATH), exist_ok=True)

def generate_cli_options():
    print('Generating Options.g.cs ...')
    
    with open(GENERATED_CLI_OPTIONS_PATH, "w+") as f:
        f.write("""using CommandLine;
namespace ChapterMD.Cli;

public partial class Options
{
""")
        for k, v in prepared_dict.items():
            f.write(f"\t{k}\n\t{v}")
        f.write("}")
    
    print('Alhamdulillah, Options.g.cs generation completed.')
    

def generate_core_options():
    print('Generating WriterOptions.g.cs ...')
    with open(GENERATED_CORE_OPTIONS_PATH, "w+") as f:
        f.write("""namespace ChapterMD.Core;

public partial class WriterOptions
{
""")
        for _, v in prepared_dict.items():
            f.write(f"\t{v}")
    
        f.write('}')
    print('Alhamdulillah, core options generation completed.')

# TODO: Generate Utils


if __name__ == "__main__":
    read_unprepared()
    create_dirs()
    
    prepare_dict()
    
    generate_cli_options()
    generate_core_options()