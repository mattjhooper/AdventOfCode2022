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

var divisorProduct = monkeys.Select(m => m.Divisor).Distinct().Aggregate((long)1, (acc, val) => acc * val);
Console.WriteLine($"Monkeys: {monkeys.Count}. DivisorProduct: {divisorProduct}");

for (int r = 0; r < 10000; r++)
{
    foreach (var monkey in monkeys)
    {
        monkey.ProcessItems(monkeys, divisorProduct);
    }    
}

foreach (var monkey in monkeys)
{
    Console.WriteLine($"Monkey {monkey.Id} inspected items {monkey.InspectedItems} times.");
}

long monkeyBusiness = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).Aggregate((long)1, (acc, m) => acc * m.InspectedItems);
Console.WriteLine($"Monkey Business {monkeyBusiness}"); // 30599555965

class Monkey
{
    public int Id { get; init; }

    public Queue<long> Items { get; }
    public Func<long, long> Operation { get; init; }

    public int Divisor { get; init; }

    public int TrueMonkey { get; init; }
    public int FalseMonkey { get; init; }

    public long InspectedItems { get; private set; } = 0;

    private Monkey(int id, int divisor, int trueMonkey, int falseMonkey, Func<long, long> operation)
    {
        Id = id;
        Divisor = divisor;
        TrueMonkey = trueMonkey;
        FalseMonkey = falseMonkey;
        Items = new Queue<long>();
        Operation = operation;
    }

    public void ProcessItems(List<Monkey> monkeys, long divisorProduct)
    {
        while (Items?.Count > 0)
        {
            var val = Operation(Items.Dequeue() % divisorProduct);

            if (val % Divisor == 0)
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
        Monkey monkey = new Monkey(id: int.Parse(lines[0][7].ToString())
             ,divisor: GetValue(lines[3])
             ,trueMonkey: int.Parse(lines[4].Last().ToString())
             ,falseMonkey: int.Parse(lines[5].Last().ToString())
             ,operation: GetFunction(lines[2].Substring(13)));

        var items = lines[1].Substring(18).Split(',');

        foreach (var item in items)
        {
            monkey.Items.Enqueue(long.Parse(item.Trim()));
        }

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