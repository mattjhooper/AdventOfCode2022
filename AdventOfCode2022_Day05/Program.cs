// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

Console.WriteLine("--- Day 5: Supply Stacks ---");

string[] lines = File.ReadAllLines(@"Input.txt");

const int BASE = 7;
const int STACK_COUNT = 9;

var builder = ImmutableArray.CreateBuilder<ImmutableList<char>>();

for(int s=0; s< STACK_COUNT; s++)
{
    builder.Add(ImmutableList.Create<char>());
}
var stacks = builder.ToImmutable();


for (int l = 0; l<= BASE; l++)
{
    // Console.WriteLine(lines[l]);
    int pos = 1;
    for (int s=0; s < STACK_COUNT; s++)
    {
        var crate = lines[l][pos];
        if (crate != ' ')
        {
            stacks[s].Add(crate);
        }
        pos += 4;
    }
}

// StackInfo(stacks);

/*
for (int s = 0; s < STACK_COUNT; s++)
{
   foreach(var crate in stacks[s])
    {
        Console.WriteLine($"Stack: {s}. Crate [{crate}]");
    }
}
*/

for (int i= BASE + 3; i<lines.Length; i++)
{
    // Console.WriteLine(lines[i]);
    var instructions = lines[i].Split(' ');
    int crateCount = int.Parse(instructions[1]);
    int fromIndex = int.Parse(instructions[3])-1;
    int toIndex = int.Parse(instructions[5])-1;
    // Console.WriteLine($"move {crateCount} from {fromIndex} to {toIndex}");

    foreach (var item in stacks[fromIndex].Take(crateCount))
    {
        stacks[toIndex].Prepend(item);
    }
    stacks[fromIndex].RemoveRange(0, crateCount);

    // StackInfo(stacks);
}
 
for (int s = 0; s < STACK_COUNT; s++)
{
    Console.Write(stacks[s][0]);
}
// DCVTCVPCL

static void StackInfo(Stack<char>[] stacks)
{
    for (int s = 0; s < stacks.Length; s++)
    {
        char crate;
        stacks[s].TryPeek(out crate);
        Console.WriteLine($"Stack: {s+1} has {stacks[s].Count} crates. Top Crate contains [{crate}]");        
    }
}