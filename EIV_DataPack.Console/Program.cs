using System.CommandLine;
using System.CommandLine.Parsing;

namespace EIV_DataPack.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        #region options
        var fileOption = new Option<FileInfo>(
            name: "--file",
            description: "The file to to manipulate.");

        var dirOption = new Option<DirectoryInfo>(
            name: "--dir",
            description: "The directory to create from.");

        var recursiveOption = new Option<bool>(
            name: "--recursive",
            description: "Recursive create from Directory.");

        var extractdirOption = new Option<DirectoryInfo>(
            name: "--dir",
            description: "The directory to extract to.");

        var exactFile = new Option<string?>(
            name: "--path",
            description: "The exact file path to extract.");

        #endregion
        #region commands
        var listCommand = new Command("list", "List all files from the .eivp file")
        {
            fileOption
        };
        listCommand.SetHandler(ListFiles, fileOption);

        var createCommand = new Command("create", "Create .eivp file from the directory")
        {
            fileOption,
            dirOption,
            recursiveOption
        };
        createCommand.SetHandler(CreateEIVP, fileOption, dirOption, recursiveOption);

        var extractCommand = new Command("extract", "Extract specific file from the .eivp Pack to a directory")
        {
            fileOption,
            extractdirOption,
            exactFile
        };
        extractCommand.SetHandler(ExtractSpecific, fileOption, extractdirOption, exactFile);

        #endregion

        var rootCommand = new RootCommand("EIV DataPack file manipulator");
        rootCommand.AddCommand(listCommand);
        rootCommand.AddCommand(createCommand);
        rootCommand.AddCommand(extractCommand);
        rootCommand.Invoke(args);
    }

    static void ListFiles(FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            var datapackCreator = DatapackCreator.Read(fileInfo.FullName);
            var reader = datapackCreator.GetReader();
            if (reader == null)
                return;
            reader.ReadFileNames(false);
            reader.Pack.FileNames.ForEach(Console.WriteLine);
        }
        else
        {
            Console.WriteLine("File not Exists!");
        }
    }

    static void CreateEIVP(FileInfo fileInfo, DirectoryInfo directory, bool rec)
    {
        var datapack = DatapackCreator.Create(fileInfo.FullName);
        var writer = datapack.GetWriter();
        if (writer == null)
            return;
        writer.AddDirectory(directory.FullName, rec);
        writer.Save();
        Console.WriteLine("File Saved!");
    }

    static void ExtractAll(FileInfo fileInfo, DirectoryInfo directory)
    {
        if (fileInfo.Exists)
        {
            var datapackCreator = DatapackCreator.Read(fileInfo.FullName);
            var reader = datapackCreator.GetReader();
            if (reader == null)
                return;
            reader.ReadFileNames(false);
            
        }
        else
        {
            Console.WriteLine("File not Exists!");
        }
    }
    static void ExtractSpecific(FileInfo fileInfo, DirectoryInfo directory, string? file)
    {
        if (fileInfo.Exists)
        {
            var datapackCreator = DatapackCreator.Read(fileInfo.FullName);
            var reader = datapackCreator.GetReader();
            if (reader == null)
                return;
            reader.ReadFileNames(false);
            if (file == null)
            {
                reader.Pack.FileNames.ForEach(x => reader.ExportFile(x, Path.Combine(directory.FullName, x)));
                return;
            }
            if (!reader.Pack.FileNames.Contains(file))
            {
                Console.WriteLine("File to extract is not exists inside the pack!");
                return;
            }
            reader.ExportFile(file, Path.Combine(directory.FullName, file));
        }
        else
        {
            Console.WriteLine("File not Exists!");
        }
    }
}
