// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 5: Supply Stacks ---");

string[] lines = File.ReadAllLines(@"Input.txt");

const int BASE = 7;
const int STACK_COUNT = 9;

var stacks = new Stack<char>[STACK_COUNT];

for(int s=0; s< STACK_COUNT; s++)
{
    stacks[s] = new();
}

for (int l = BASE; l>=0; l--)
{
    Console.WriteLine(lines[l]);
    int pos = 1;
    for (int s=0; s < STACK_COUNT; s++)
    {
        var crate = lines[l][pos];
        if (crate != ' ')
        {
            stacks[s].Push(crate);
        }
        pos += 4;
    }
}

StackInfo(stacks);

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
    Console.WriteLine(lines[i]);
    var instructions = lines[i].Split(' ');
    int crateCount = int.Parse(instructions[1]);
    int fromIndex = int.Parse(instructions[3])-1;
    int toIndex = int.Parse(instructions[5])-1;
    // Console.WriteLine($"move {crateCount} from {fromIndex} to {toIndex}");

    for (int m = 0; m < crateCount; m++)
    {
        char crate;
        if (stacks[fromIndex].TryPop(out crate))
        {
            stacks[toIndex].Push(crate);
        }
    }

    StackInfo(stacks);
}

 
for (int s = 0; s < STACK_COUNT; s++)
{
    Console.Write(stacks[s].Peek());
}

static void StackInfo(Stack<char>[] stacks)
{
    for (int s = 0; s < stacks.Length; s++)
    {
        char crate = ' ';
        stacks[s].TryPeek(out crate);
        Console.WriteLine($"Stack: {s+1} has {stacks[s].Count} crates. Top Crate contains [{crate}]");        
    }
}