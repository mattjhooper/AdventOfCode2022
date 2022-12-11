// See https://aka.ms/new-console-template for more information
using System.Numerics;

Console.WriteLine("--- Day 11: Monkey in the Middle ---");

var lines = await File.ReadAllLinesAsync("Input.txt");


var monkeys = new List<Monkey>();
var skipTo = 0;
while (skipTo < lines.Length)
{
    var monkeyLines = lines.Skip(skipTo).Take(6);
    skipTo += monkeyLines.Count() + 1;

    monkeys.Add(Monkey.Create(monkeyLines.ToArray()));
}

Console.WriteLine($"Monkeys: {monkeys.Count}");

for (int r = 0; r < 20; r++)
{
    foreach (var monkey in monkeys)
    {
        monkey.ProcessItems(monkeys);
    }    
}

foreach (var monkey in monkeys)
{
    // monkey.PrintItems();
    Console.WriteLine($"Monkey {monkey.Id} inspected items {monkey.InspectedItems} times.");
}

BigInteger monkeyBusiness = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).Aggregate((BigInteger)1, (acc, m) => acc * m.InspectedItems);
Console.WriteLine($"Monkey Business {monkeyBusiness}");

class Item
{
    private readonly long original;
    private readonly int firstMonkey;

    public Item(long i, int monkey)
    {
        this.original= i;
        this.firstMonkey = monkey;
        Current = i;
    }

    public long Current { get; set; }

    public void CheckCurrent(int monkey)
    {
        Current = monkey == firstMonkey ? original : Current;
    }

}

class Monkey
{
    public int Id { get; set; }

    public Queue<Item> Items { get; }
    public Func<long, long> Operation { get; private set; }

    public int Divisor { get; set; }

    public int TrueMonkey { get; set; }
    public int FalseMonkey { get; set; }

    public long InspectedItems { get; set; } = 0;

    private Monkey()
    {
        Items = new Queue<Item>();
    }

    public void ProcessItems(List<Monkey> monkeys)
    {
        while (Items?.Count > 0)
        {
            var val = Items.Dequeue();

            /*
            if (val > 0.99 * long.MaxValue )
            {
                Console.WriteLine($"Val: {val}. Max: {long.MaxValue}. Difference: {long.MaxValue - val}");
            }
            */          

            val.CheckCurrent(Id);
            val.Current = Operation(val.Current);
            
            if (val.Current % Divisor == 0)
            {
                monkeys[TrueMonkey].Items.Enqueue(val);
            }
            else
            {
                monkeys[FalseMonkey].Items.Enqueue(val);
            }
            InspectedItems++;
        }
    }

    public void PrintItems()
    {
        Console.Write($"Monkey {Id}: ");
        foreach(var item in Items)
        {
            Console.Write($"{item}, ");
        }
        Console.WriteLine();
    }

    public static Monkey Create(string[] lines)
    {
        Monkey monkey = new Monkey();

        monkey.Id = int.Parse(lines[0][7].ToString());
        var items = lines[1].Substring(18).Split(',');

        foreach (var item in items)
        {
            monkey.Items.Enqueue(new Item(long.Parse(item.Trim()),monkey.Id));
        }

        monkey.Operation = GetFunction(lines[2].Substring(13));
        monkey.Divisor = GetValue(lines[3]);
        monkey.TrueMonkey = int.Parse(lines[4].Last().ToString());
        monkey.FalseMonkey = int.Parse(lines[5].Last().ToString());

        return monkey;
    }

    private static int GetValue(string line) => int.Parse(line.Split(' ').Last());

    private static Func<long, long> GetFunction(string line)
    {
        if (line == "new = old * old")
        {
            return (i => i * i);
        }

        if (line.StartsWith("new = old +"))
        {
            return (i => i + GetValue(line));
        }

        if (line.StartsWith("new = old *"))
        {
            return (i => i * GetValue(line));
        }

        return (n => n);
    }
}