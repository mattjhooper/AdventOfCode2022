// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using static Directions;

Console.WriteLine("--- Day 9: Rope Bridge ---");

string[] lines = File.ReadAllLines(@"Input.txt");

var moves = lines.SelectMany(GetDirections);

var head = new Point(0, 0);
var tail = new Point(0, 0);
var visited = new HashSet<Point>();

// Print(head, tail);
foreach (var move in moves)
{
    head = head.Move(move);
    // Print(head, tail);

    tail = Adjust(head, tail);

    visited.Add(tail);
    // Print(head, tail);
}

Print(head, tail, visited);

Console.WriteLine($"Tail visited {visited.Count} points.");

static void Print(Point head, Point tail, HashSet<Point>? visited = null)
{
    var v = visited ?? new HashSet<Point>();

    Console.SetCursorPosition(0, 0);
    for (int y = 40; y > -40; y--)
    {
        for (int x = -40; x < 40; x++)
        {
            var pos = new Point(x, y);
            if (head == pos)
            {
                Console.Write('H');
            }
            if (tail == pos)
            {
                Console.Write('T');
            }
            else if (x == 0 && y == 0)
            {
                Console.Write('S');
            }
            else if (v.Contains(pos))
            {
                Console.Write('#');
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



static class Directions
{
    static readonly Point Up = new Point(0, 1);
    static readonly Point Down = new Point(0, -1);
    static readonly Point Left = new Point(-1, 0);
    static readonly Point Right = new Point(1, 0);
    static readonly Point Stay = new Point(0, 0);


    public static IEnumerable<Point> GetDirections(string line)
    {
        var parts = line.Split(' ');

        for (int i = 0; i < int.Parse(parts[1]); i++)
        {
            yield return GetDirection(parts[0][0]);
        }
    }

    public static Point Move(this Point p, Point direction) => new Point (p.X + direction.X, p.Y + direction.Y);

    public static Point Delta(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

    public static Point Adjust(Point head, Point tail)
    {
        if (head.X - tail.X > 1)
            return head.Move(Left);

        if (head.X - tail.X < -1)
            return head.Move(Right);

        if (head.Y - tail.Y > 1)
            return head.Move(Down);

        if (head.Y - tail.Y < -1)
            return head.Move(Up);

        return tail;

    }

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

