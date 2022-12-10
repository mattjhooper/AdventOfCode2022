// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 10: Cathode-Ray Tube ---");

var lines = File.ReadAllLines("Input.txt");

var cycle = 0;
var x = 1;
int signalStrength = 0;
int checkCycle = 20;

foreach (var line in lines)
{
    CheckCycle();
    if (line != "noop")
    {
        CheckCycle();
        int addX = int.Parse(line.Split(' ')[1]);

        x += addX;
    }
}

Console.WriteLine($"Sum of signalStrength: {signalStrength}");

cycle = 0;
x = 1;
foreach (var line in lines)
{
    DrawPixel();
    if (line != "noop")
    {
        DrawPixel();
        int addX = int.Parse(line.Split(' ')[1]);

        x += addX;
    }
}

void DrawPixel()
{
    char pixel = x-1 <= cycle && cycle <= x+1 ? '#' : '.';
    Console.Write(pixel);

    if (cycle == 39)
    {
        cycle = 0;
        Console.WriteLine();
    }
    else
    {
        cycle++;
    }
}

void CheckCycle()
{
    cycle++;
    if (cycle == checkCycle)
    {
        // Console.WriteLine($"Cycle: {cycle}. X: {x}. Signal Strength: {cycle * x}");
        signalStrength += (cycle * x);
        checkCycle += 40;
    }
}