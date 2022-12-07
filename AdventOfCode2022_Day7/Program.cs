// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 7: No Space Left On Device ---");

string[] lines = File.ReadAllLines(@"Input.txt");

SystemDirectory currDir = null;

currDir = lines.Aggregate(currDir, ProcessLine);
currDir = currDir.GetRoot();

var sumDirsUnder100k = currDir.GetThisAndAllSubDirectories().Select(d => d.GetSize()).Where(s => s <= 100000).Sum();
Console.WriteLine($"Total sum of Directories under 100k: {sumDirsUnder100k}");

var allMemoryUsed = currDir.GetSize();
var spaceRequired = 30000000 - (70000000 - allMemoryUsed);
Console.WriteLine($"All memory used: {allMemoryUsed}. Space required: {spaceRequired}");


var directorySizeToDelete = currDir.GetThisAndAllSubDirectories().Select(d => d.GetSize()).OrderBy(s => s).First(s => s >= spaceRequired);
Console.WriteLine($"Directory Size to delete: {directorySizeToDelete}.");

static SystemDirectory ProcessLine(SystemDirectory d, string line)
{
    if (line == "$ cd /")
    {
        var root = new SystemDirectory
        {
            Name = "/",
        };

        return root;
    }

    if (line == "$ cd ..")
    {
        return d.ParentDirectory;
    }

    if (line.StartsWith("$ cd "))
    {
        var parts = line.Split(' ');
        var name = parts[2];

        var subdirectory = d.ChildDirectories.First(d => d.Name == name);

        return subdirectory;
    }

    if (line == "$ ls")
    {
        return d;
    }

    if (line.StartsWith("dir"))
    {
        var name = line.Split(' ')[1];
        var childDir = new SystemDirectory
        {
            Name = name,
            ParentDirectory = d,
        };
        d.ChildDirectories.Add(childDir);
        return d;
    }

    var fileParts = line.Split(' ');
    var childFile = new SystemFile()
    {
        Name = fileParts[1],
        Size = long.Parse(fileParts[0]),
    };
    d.SystemFiles.Add(childFile);

    return d;
}

class SystemDirectory: IPrintable
{
    public string Name { get; init; }
    public List<SystemDirectory> ChildDirectories { get; } = new();

    public List<SystemFile> SystemFiles { get; } = new();

    public SystemDirectory? ParentDirectory { get; init; }

    public SystemDirectory GetRoot()
    {
        if (ParentDirectory is null)
        {
            return this;
        }

        return ParentDirectory.GetRoot();
    }

    public long GetSize()
    {
        return ChildDirectories.Select(d => d.GetSize()).Sum() + SystemFiles.Select(f => f.Size).Sum();
    }

    public void Print(string prefix)
    {
        System.Console.WriteLine($"{prefix}- {Name} (dir)");    
        foreach(IPrintable printable in ChildDirectories.Select(d => d as IPrintable).Union(SystemFiles.Select(f => f as IPrintable)))
        {
            printable.Print(prefix + "  ");
        }
    }

    public IEnumerable<SystemDirectory> GetThisAndAllSubDirectories()
    {
        return ThisAsEnumerable().Union(ChildDirectories.SelectMany(d => d.GetThisAndAllSubDirectories()));
    }

    private IEnumerable<SystemDirectory> ThisAsEnumerable()
    {
        yield return this;
    }

}

interface IPrintable
{
    void Print(string prefix);
}

class SystemFile: IPrintable
{
    public string Name { get; init; }

    public long Size { get; init; }

    public void Print(string prefix)
    {
        System.Console.WriteLine($"{prefix}- {Name} (file, size={Size})");
    }
}

public static class Extensions
{
    const char CMD = '$';
    public static bool IsCommand(this string line) => line.StartsWith(CMD);
}