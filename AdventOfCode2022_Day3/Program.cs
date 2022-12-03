// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Rucksack Reorganization ---");

string[] lines = File.ReadAllLines(@"Input.txt");
int prioritySum = lines
    .Select(GetCompartments)
    .Select(b => b.compartment1.Intersect(b.compartment2).Single())
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Priority Sum {prioritySum}."); // 8298


int groupStart = 0;
int badgePriority = 0;
while (groupStart < lines.Length)
{
    var group = lines.Skip(groupStart).Take(3).ToArray();
    groupStart += 3;
    char badge = group[0].Intersect(group[1]).Intersect(group[2]).Single();
    badgePriority += GetPriority(badge);
}

Console.WriteLine($"Badge Priority Sum {badgePriority}.");

static (string compartment1, string compartment2) GetCompartments(string line)
{
    var compartment1 = line.Remove(line.Length / 2);
    var compartment2 = line.Substring(line.Length / 2);
    return (compartment1, compartment2);
}

static int GetPriority(char item)
{
    return item < 'a' ? (int)item - (int)'A' + 27 : (int)item - (int)'a' + 1;
}

