using CommandLine;

namespace EIV_DataPack.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        bool ret = Parser.Default.ParseArguments<ListOptions, CreateOptions, ExtractOptions, FileVersionOption>(args)
            .MapResult(
              (ListOptions opts) => ListFiles(opts.File),
              (CreateOptions opts) => CreateEIVP(opts.File, opts.Dir, opts.Recursive),
              (ExtractOptions opts) => ExtractSpecific(opts.File, opts.Dir, opts.Path),
              (FileVersionOption opts) => ShowVersion(opts.File),
              errs => false);
        Console.WriteLine(ret);
    }

    static bool ShowVersion(string FileName)
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine("File not Exists!");
            return false;
        }
        var datapackCreator = DatapackManager.Read(FileName);
        var reader = datapackCreator.GetReader();
        if (reader == null)
            return false;
        Console.WriteLine(reader.Pack.Version);
        return true;
    }

    static bool ListFiles(string FileName)
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine("File not Exists!");
            return false;
        }
        var datapackCreator = DatapackManager.Read(FileName);
        var reader = datapackCreator.GetReader();
        if (reader == null)
            return false;
        reader.ReadFileNames();
        foreach (string item in reader.Pack.FileNames)
            Console.WriteLine(item);
        return true;
    }

    static bool CreateEIVP(string fileName, string directory, bool rec)
    {
        var datapack = DatapackManager.Create(fileName);
        var writer = datapack.GetWriter();
        if (writer == null)
            return false;
        writer.AddDirectory(directory, rec);
        writer.Save();
        Console.WriteLine("File Saved!");
        return true;
    }

    static bool ExtractSpecific(string fileName, string directory, string? file)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine("File not Exists!");
            return false;
        }
        var datapackCreator = DatapackManager.Read(fileName);
        var reader = datapackCreator.GetReader();
        if (reader == null)
            return false;
        if (file == null)
        {
            reader.Pack.FileNames.ToList().ForEach(x => reader.ExportFile(x, Path.Combine(directory, x)));
            return false;
        }
        if (!reader.Pack.FileNames.Contains(file))
        {
            Console.WriteLine("File to extract is not exists inside the pack!");
            return false;
        }
        reader.ExportFile(file, Path.Combine(directory, file));
        return true;
    }
}
