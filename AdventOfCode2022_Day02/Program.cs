// See https://aka.ms/new-console-template for more information
Console.WriteLine("---Day 2: Rock Paper Scissors-- - ");

string[] lines = File.ReadAllLines(@"Input.txt");

var totalScore = lines.Select(l => ApplyStrategy(ProcessLine(l))).Sum();

Console.WriteLine($"Total Score {totalScore}."); // 12111

static (IShape shape, IStrategy strategy) ProcessLine(string line)
{
    var parts = line.Split(' ');
    return (IShape.Create(parts[0][0]), IStrategy.Create(parts[1][0]));
}

static int ApplyStrategy((IShape shape, IStrategy strategy) parts) => parts.strategy.Score + parts.shape.ForStrategy(parts.strategy).Score;

interface IShape
{
    int Score { get; }

    public static IShape Create(char shapeChar) => shapeChar switch
    {
        'A' => new Rock(),
        'B' => new Paper(),
        'C' => new Scissors(),
        _ => throw new NotImplementedException(),
    };

    IShape ForStrategy(IStrategy strategy);
}

record Rock : IShape
{
    public int Score => 1;

    public IShape ForStrategy(IStrategy strategy) => strategy switch
    {
        IsBeatenBy => new Paper(),
        DrawsWith => new Rock(),
        LosesTo => new Scissors(),
        _ => throw new NotImplementedException(),
    };
}

record Paper : IShape
{
    public int Score => 2;
    public IShape ForStrategy(IStrategy strategy) => strategy switch
    {
        IsBeatenBy => new Scissors(),
        DrawsWith => new Paper(),
        LosesTo => new Rock(),
        _ => throw new NotImplementedException(),
    };
}

record Scissors : IShape
{
    public int Score => 3;
    public IShape ForStrategy(IStrategy strategy) => strategy switch
    {
        IsBeatenBy => new Rock(),
        DrawsWith => new Scissors(),
        LosesTo => new Paper(),
        _ => throw new NotImplementedException(),
    };
}

interface IStrategy
{
    int Score { get; }

    public static IStrategy Create(char strategyChar) => strategyChar switch
    {
        'X' => new LosesTo(),
        'Y' => new DrawsWith(),
        'Z' => new IsBeatenBy(),
        _ => throw new NotImplementedException(),
    };
}

record IsBeatenBy : IStrategy
{
    public int Score => 6;
}

record DrawsWith : IStrategy
{
    public int Score => 3;
}

record LosesTo : IStrategy
{
    public int Score => 0;
}