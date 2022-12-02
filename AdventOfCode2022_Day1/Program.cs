using System.Linq;


Console.WriteLine("--- Day 1: Calorie Counting ---");

string[] lines = File.ReadAllLines(@"Input.txt");

long topThreeTotal = 0;

long currentTotal = 0;

List<long> totals = new List<long>();

foreach (string line in lines)
{
    if (line == "")
    {
        totals.Add(currentTotal);
        currentTotal = 0;
    }
    else
    {
        currentTotal += long.Parse(line);
    }    
}

totals.Add(currentTotal);

topThreeTotal = totals.OrderByDescending(x => x).Take(3).Sum();

Console.WriteLine(topThreeTotal);
