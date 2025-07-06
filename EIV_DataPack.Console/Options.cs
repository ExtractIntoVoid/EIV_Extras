using CommandLine;

namespace EIV_DataPack.ConsoleApp;

public class CommonOptions
{
    [Option('f', "file", Required = true, HelpText = "The file to to manipulate.")]
    public string File { get; set; } = string.Empty;
}

[Verb("list", HelpText = "List all files from the .eivp file.")]
public class ListOptions : CommonOptions;

[Verb("fversion", HelpText = "Show the eivp file version.")]
public class FileVersionOption : CommonOptions;

[Verb("create", HelpText = "Create and add all files to the .eivp file.")]
public class CreateOptions : CommonOptions
{
    [Option('d', "dir", Required = true, HelpText = "The directory to create from.")]
    public string Dir { get; set; } = string.Empty;

    [Option('r', "recursive", Required = false, HelpText = "Recursive create from Directory.")]
    public bool Recursive { get; set; }
}

[Verb("extract", HelpText = "Extract specific file from the .eivp Pack to a directory.")]
public class ExtractOptions : CommonOptions
{
    [Option('d', "dir", Required = true, HelpText = "The directory to extract from.")]
    public string Dir { get; set; } = string.Empty;
    [Option('p', "path", Required = true, HelpText = "The exact file path to extract.")]
    public string Path { get; set; } = string.Empty;
}
