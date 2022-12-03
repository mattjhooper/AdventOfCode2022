// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("--- Day 3: Rucksack Reorganization ---");

string[] lines = File.ReadAllLines(@"Input.txt");
int prioritySum = 0;
foreach (string line in lines)
{
    var (compartment1, compartment2) = GetCompartments(line);

    var compartment1Items = new HashSet<char>();
    foreach(var item in compartment1)
    {
        compartment1Items.Add(item);
    }

    char match = default;
    for(int i=0; match == default && i < compartment2.Length; i++)
    {
        compartment1Items.TryGetValue(compartment2[i], out match);
    }

    Console.WriteLine($"Common item {match}. Priority: {GetPriority(match)}");
    prioritySum += GetPriority(match);
}

Console.WriteLine($"Priority Sum {prioritySum}.");

static (string, string) GetCompartments(string line)
{
    var compartment1 = line.Remove(line.Length / 2);
    var compartment2 = line.Substring(line.Length / 2);
    return (compartment1, compartment2);
}

static int GetPriority(char item)
{
    return item < 'a' ? (int)item - (int)'A' + 27 : (int)item - (int)'a' + 1;
}