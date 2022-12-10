// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using static Directions;

Console.WriteLine("--- Day 9: Rope Bridge ---");

string[] lines = File.ReadAllLines(@"Input.txt");

var moves = lines.SelectMany(GetDirections);

var head = new Point(0, 0);
var tail = new Point(0, 0);
var visited = new HashSet<Point>();

foreach (var move in moves)
{
    head = head.Move(move);

    tail = GetNewPosition(head, tail);

    visited.Add(tail);
}

Console.WriteLine($"Tail visited {visited.Count} points."); // 6037

var rope = new List<Point>();

for (int k = 0; k<10; k++)
{
    rope.Add(new Point(0,0));
}

var visited2 = new HashSet<Point>();
foreach (var move in moves)
{
    for (int k = 0; k<rope.Count; k++)
    {
        rope[k] = k == 0 ? rope[k].Move(move) : GetNewPosition(rope[k - 1], rope[k]);

    }
    visited2.Add(rope[9]);
}
Console.WriteLine($"Tail visited {visited2.Count} points."); // 2485

static void PrintHeadTail(Point head, Point tail, HashSet<Point>? visited = null)
{
    var v = visited ?? new HashSet<Point>();

    Console.SetCursorPosition(0, 0);
    for (int y = 15; y > -15; y--)
    {
        for (int x = -15; x < 15; x++)
        {
            var pos = new Point(x, y);
            if (v.Contains(pos))
            {
                Console.Write('#');
            }
            else if (head == pos)
            {
                Console.Write('H');
            }
            else if (tail == pos)
            {
                Console.Write('T');
            }
            else if (x == 0 && y == 0)
            {
                Console.Write('S');
            }
            else
            {
                Console.Write('.');
            }

        }
        Console.WriteLine();
    }
    Thread.Sleep(150);
}

static void PrintRope(List<Point> rope)
{
    Console.WriteLine();
    for (int y = 5; y >= 0; y--)
    {
        for (int x = 0; x < 6; x++)
        {
            var pos = new Point(x, y);

            if (rope[0] == pos)
            {
                Console.Write('H');
            }
            else if (rope.Contains(pos))
            {
                for (int r = 1; r < rope.Count; r++)
                {
                    if (rope[r] == pos)
                    {
                        Console.Write(r);
                        goto skipTo;
                    }
                }
            skipTo:;
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

static class Directions
{
    static readonly Point Up = new Point(0, 1);
    static readonly Point Down = new Point(0, -1);
    static readonly Point Left = new Point(-1, 0);
    static readonly Point Right = new Point(1, 0);    


    public static IEnumerable<Point> GetDirections(string line)
    {
        var parts = line.Split(' ');

        for (int i = 0; i < int.Parse(parts[1]); i++)
        {
            yield return GetDirection(parts[0][0]);
        }
    }

    public static Point Move(this Point p, Point direction) => new Point (p.X + direction.X, p.Y + direction.Y);

    public static Point GetNewPosition(Point head, Point tail)
    {
        int horizontalAdjustment = GetAdjustment(head.X, tail.X);
        int verticalAdjustment = GetAdjustment(head.Y, tail.Y);

        if (horizontalAdjustment != 0 || verticalAdjustment != 0)
        {
            return new Point(head.X + horizontalAdjustment, head.Y + verticalAdjustment);
        }

        return tail;
    }

    public static int GetAdjustment(int v1, int v2) => (v1 - v2) switch
    {
        > 1 => -1,
        < -1 => 1,
        _ => 0,
    };

    static Point GetDirection(char c) => c switch
    {
        'U' => Up,
        'D' => Down,
        'L' => Left,
        'R' => Right,
        _ => throw new NotImplementedException(),
    };
}

record Point(int X, int Y);

