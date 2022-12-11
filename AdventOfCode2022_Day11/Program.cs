// See https://aka.ms/new-console-template for more information
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

var monkeyBusiness = monkeys.OrderByDescending(m => m.InspectedItems).Take(2).Aggregate(1, (acc, m) => acc * m.InspectedItems);
Console.WriteLine($"Monkey Business {monkeyBusiness}");

class Monkey
{
    public int Id { get; set; }

    public Queue<int> Items { get; }
    public Func<int, int> Operation { get; private set; }

    public int Divisor { get; set; }

    public int TrueMonkey { get; set; }
    public int FalseMonkey { get; set; }

    public int InspectedItems { get; set; } = 0;

    private Monkey()
    {
        Items = new Queue<int>();
    }

    public void ProcessItems(List<Monkey> monkeys)
    {
        while (Items?.Count > 0)
        {
            var val = Items.Dequeue();
            val = Operation(val);
            val = val / 3;

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
        Monkey monkey = new Monkey();

        monkey.Id = int.Parse(lines[0][7].ToString());
        var items = lines[1].Substring(18).Split(',');

        foreach (var item in items)
        {
            monkey.Items.Enqueue(int.Parse(item.Trim()));
        }

        monkey.Operation = GetFunction(lines[2].Substring(13));
        monkey.Divisor = GetValue(lines[3]);
        monkey.TrueMonkey = int.Parse(lines[4].Last().ToString());
        monkey.FalseMonkey = int.Parse(lines[5].Last().ToString());

        return monkey;
    }

    private static int GetValue(string line) => int.Parse(line.Split(' ').Last());

    private static Func<int,int> GetFunction(string line)
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