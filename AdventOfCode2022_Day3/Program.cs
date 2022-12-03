// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Rucksack Reorganization ---");

string[] lines = File.ReadAllLines(@"Input.txt");
int prioritySum = lines
    .Select(l => GetCompartments(l))
    .Select(b => b.compartment1.Intersect(b.compartment2).Single())
    .Select(c => GetPriority(c))
    .Sum();

Console.WriteLine($"Priority Sum {prioritySum}."); // 8298

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