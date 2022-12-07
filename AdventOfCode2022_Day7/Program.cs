// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 7: No Space Left On Device ---");

string[] lines = File.ReadAllLines(@"Input.txt");

int i = 0;

SystemDirectory currDir = null;

while (i < lines.Length)
{
    (currDir, i) = ProcessLine(lines, currDir, i);
}

currDir = currDir.GetRoot();
currDir.Print("");
var total = currDir.GetThisAndAllSubDirectories().Select(d => d.GetSize()).Where(s => s <= 100000).Sum();

Console.WriteLine($"Total: {total}");


static (SystemDirectory currentDirectory, int currentLine) ProcessLine(string[] lines, SystemDirectory directory, int line)
{
    string outputLine = lines[line];

    if (!outputLine.IsCommand())
    {
        Console.WriteLine($"Not a command: {outputLine}");
        return (directory, ++line);
    }

    if (outputLine == "$ cd ..")
    {
        Console.WriteLine($"Move to parent directory");
        return (directory.ParentDirectory, ++line);
    }

    if (outputLine == "$ cd /")
    {
        Console.WriteLine($"Move to root");
        var root = new SystemDirectory
        {
            Name = "/",
        };

        return (root, ++line);
    }

    if (outputLine.StartsWith("$ cd "))
    {
        var parts = outputLine.Split(' ');
        var name = parts[2];

        Console.WriteLine($"Move to {name}");
        var subdirectory = directory.ChildDirectories.First(d => d.Name == name);

        return (subdirectory, ++line);
    }

    if (outputLine == "$ ls")
    {
        Console.WriteLine($"List contents");

        bool keepLooking = true;
        line++;

        while (keepLooking) 
        {                    
            outputLine = lines[line];

            if (outputLine.StartsWith("dir"))
            {
                var name = outputLine.Split(' ')[1];
                var childDir = new SystemDirectory
                {
                    Name = name,
                    ParentDirectory = directory,
                };
                directory.ChildDirectories.Add(childDir);
            }
            else
            {
                var parts = outputLine.Split(' ');
                var childFile = new SystemFile()
                {
                    Name = parts[1],
                    Size = long.Parse(parts[0]),
                };
                directory.SystemFiles.Add(childFile);
            }

            keepLooking = ++line < lines.Length && !lines[line].IsCommand();
        }

        return (directory, line);
    }

    return (directory, ++line);  
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