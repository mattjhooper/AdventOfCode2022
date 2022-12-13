// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Linq;

Console.WriteLine("--- Day 12: Hill Climbing Algorithm ---");

var lines = await File.ReadAllLinesAsync("Input.txt");

var terrain = new Terrain(lines);

bool endFound;
Node? endNode;
do
{
    (endFound, endNode) = terrain.ProcessNextOpenNode();
} while (!endFound);

int best = endNode.StepsToStart();
Console.WriteLine($"Steps from Start to End: {best}");

foreach (var start in terrain.GetNodes('a'))
{
    terrain.ResetStart(start.Location);
    do
    {
        (endFound, endNode) = terrain.ProcessNextOpenNode();
    } while (!endFound);
    int routeSteps = endNode is null? int.MaxValue : endNode.StepsToStart();
    best = Math.Min(best, routeSteps);
}
Console.WriteLine($"Minimum Steps from Start to End: {best}");

class Terrain
{
    const char S = 'S';
    const char E = 'E';

    public Terrain(string[] levels)
    {
        Levels = levels;
        Start = GetLocation(levels, S);
        End = GetLocation(levels, E);
        OpenPoints = new Dictionary<Point, Node>();
        ClosedPoints = new Dictionary<Point, Node>();

        ResetStart(Start);
    }

    public string[] Levels { get; }

    public Point Start { get; private set; }

    public Point End { get; }

    public Dictionary<Point,Node> OpenPoints { get; }
    public Dictionary<Point, Node> ClosedPoints { get; }

    private static Point GetLocation(string[] levels, char searchChar) =>
    levels.Select((l, i) =>
    {
        var loc = Array.FindIndex(l.ToCharArray(), c => c == searchChar);
        return new Point(loc, loc == -1 ? -1 : i);
    }
    ).FirstOrDefault(p => p.X != -1 && p.Y != -1, new Point(-1, -1));

    private static bool IsPermitted(char a, char b) => b - a <= 1;

    private static IEnumerable<Point> GetAdjacentPoints(Point p)
    {
        yield return p with { X = p.X - 1 };
        yield return p with { Y = p.Y - 1 };
        yield return p with { X = p.X + 1 };
        yield return p with { Y = p.Y + 1 };
    }

    private IEnumerable<Point> GetInBoundsAdjacentPoints(Point p) => GetAdjacentPoints(p).Where(p => InBounds(p));

    private bool IsPermitted(Point a, Point b) => IsPermitted(GetChar(a), GetChar(b));

    private Node GetNode(Point p, Node? parent) => new Node(p, GetChar(p), parent, p.Distance(End), parent?.G + 10 ?? 0);

    public IEnumerable<Point> GetCandidates(Point p)
    {
        return GetInBoundsAdjacentPoints(p).Where(b => IsPermitted(p, b)).Where(b => !ClosedPoints.ContainsKey(b));
    }

    public char GetChar(Point p)
    {
        if (p == Start) return 'a';
        if (p == End) return 'z';

        if (InBounds(p))
            return Levels[p.Y][p.X];

        throw new ArgumentOutOfRangeException(nameof(p), $"{p} is out of bounds");
    }

    public Node StartNode() => GetNode(Start, null);

    public IEnumerable<Node> GetCandidates(Node node) => GetCandidates(node.Location).Select(p => GetNode(p, node));

    public bool InBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X < Levels[0].Length && p.Y < Levels.Length;

    public (bool end, Node? node) ProcessNextOpenNode()
    {
        if (OpenPoints.Count == 0)
        {
            return (true, null);
        }

        var f = OpenPoints.ToList().Select(v => v.Value).OrderBy(n => n.F).First();
        OpenPoints.Remove(f.Location);

        ClosedPoints.Add(f.Location, f);

        if (f.Location == End)
        {
            return (true, f);
        }

        foreach (var n in GetCandidates(f))
        {
            Node existing;
            if (OpenPoints.TryGetValue(n.Location, out existing))
            {
                if (existing.G > n.G)
                {
                    OpenPoints[n.Location] = n;
                }
            }
            else
            {
                OpenPoints.Add(n.Location, n);
            }
        }

        return (false, f);
    }

    public IEnumerable<Node> GetNodes(char c)
    {
        for (int y = 0; y < Levels.Length; y++)
        {
            for (int x = 0; x < Levels[y].Length; x++)
            {
                if (Levels[y][x] == c)
                {
                    yield return GetNode(new Point(x, y), null);
                }
            }
        }
    }

    public void ResetStart(Point p)
    {
        Start = p;
        OpenPoints.Clear();
        ClosedPoints.Clear();
        OpenPoints.Add(Start, StartNode());
    }

}

record Point(int X, int Y)
{
    public int Distance(Point target) => 10 * (Math.Abs(target.X - X) + Math.Abs(target.Y - Y));

    public override string ToString() => $"[{X},{Y}]";
}

record Node
{
    public Point Location { get; init; }
    public char Level { get; init; }
    public Node? Parent { get; init; }
    public int H { get; init; }
    public int G { get; init; }
    public int F => H + G;

    public int StepsToStart() => Parent is null? 0 : 1 + Parent.StepsToStart();

    public Node(Point location, char level, Node parent, int h, int g)
    {
        this.Location = location;
        this.Level = level;
        this.Parent = parent;
        this.H = h;
        this.G = g;
    }
}