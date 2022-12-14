// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 4: Camp Cleanup ---");

string[] lines = File.ReadAllLines(@"Input.txt");

var assignments = lines
    .Select(GetAssignments)
    .ToList();

var fullyContainedTotal = assignments
    .Select(FullyContainedCount)
    .Sum();

Console.WriteLine($"Fully Contained Total: {fullyContainedTotal}");

var overlapTotal = assignments
    .Select(OverlapCount)
    .Sum();

Console.WriteLine($"Overlaps Total: {overlapTotal}");

static (Assignment a1, Assignment a2) GetAssignments(string line)
{
    var parts = line.Split(',');
    return (GetAssignment(parts[0]), GetAssignment(parts[1]));
}

static Assignment GetAssignment(string assignment)
{
    var sections = assignment.Split('-');
    return new Assignment(sections[0], sections[1]);
}

static int FullyContainedCount((Assignment a1, Assignment a2) pair) => pair.a1.FullyContains(pair.a2) || pair.a2.FullyContains(pair.a1) ? 1 : 0;

static int OverlapCount((Assignment a1, Assignment a2) pair) => pair.a1.Overlaps(pair.a2) ? 1 : 0;

record struct Assignment
{
    public int Start { get; init; }
    public int End { get; init; }
    public Assignment(int start, int end)
    {
        Start = start;
        End = end;
    }

    public Assignment(string start, string end) : this (int.Parse(start), int.Parse(end))
    {
    }

    public bool FullyContains(Assignment r) => Start <= r.Start && r.End <= End;

    public bool Overlaps(Assignment r) => (Start <= r.Start && r.Start <= End) || (r.Start <= Start && Start <= r.End);

    public override string ToString() => $"{Start}-{End}";
}
