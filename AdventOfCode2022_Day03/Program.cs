// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Rucksack Reorganization ---");

string[] lines = File.ReadAllLines(@"Input.txt");
int prioritySum = lines
    .Select(GetCompartments)
    .Select(b => b.compartment1.Intersect(b.compartment2).Single())
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Priority Sum {prioritySum}.");

var badgePrioritySum = GetGroups(lines)
    .Select(b => b[0].Intersect(b[1]).Intersect(b[2]).Single())
    .Select(GetPriority)
    .Sum();

Console.WriteLine($"Badge Priority Sum 2 {badgePrioritySum}.");

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

static IEnumerable<string[]> GetGroups(string[] lines)
{
    int groupStart = 0;
    while (groupStart < lines.Length)
    {
        yield return lines.Skip(groupStart).Take(3).ToArray();
        groupStart += 3;
    }    
}