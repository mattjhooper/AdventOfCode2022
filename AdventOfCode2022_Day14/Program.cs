﻿// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Reflection;

Console.WriteLine("--- Day 14: Regolith Reservoir ---");

var inputPaths = await File.ReadAllLinesAsync("Input.txt");


var tiles = new Dictionary<Point, char>();

foreach (var rock in inputPaths.SelectMany(GetRocks).ToHashSet())
{
    tiles.TryAdd(rock, '#');
}

var ORIGIN = new Point(500, 0);

tiles.TryAdd(ORIGIN, '+');

var min = new Point(tiles.Select(d => d.Key.X).Min(), 0);
var max = new Point(tiles.Select(d => d.Key.X).Max(), tiles.Select(d => d.Key.Y).Max());

Console.WriteLine($"Min: {min}. Max: {max}");

Print(tiles, min, max);


var sand = ORIGIN with { Y = 1 };

var count = 0;

while (sand.Y <= max.Y)
{
    sand = ORIGIN with { Y = 1 };
    bool atRest;

    do
    {
        var nextPoint = tiles.NextPoint(sand);
        atRest = sand == nextPoint;
        sand = nextPoint;
    }
    while (!atRest && sand.Y <= max.Y);

    tiles.TryAdd(sand, 'O');
    Print(tiles, min, max);
    count++;
}

Console.WriteLine($"Sand Count {count - 1}");


static void Print(Dictionary<Point, char> tiles, Point min, Point max)
{
    for (int y = min.Y; y<=max.Y; y++)
    {
        for (int x = min.X; x<=max.X; x++)
        {
            char tile;

            if (!tiles.TryGetValue(new Point(x,y), out tile))
            {
                tile = '.';
            };

            Console.Write(tile);
        }
        Console.WriteLine();
    }
}

static IEnumerable<Point> GetLinePoints (Point p1, Point p2)
{
    if (p1.X == p2.X) // horizontal line
    {
        for (int y = Math.Min(p1.Y, p2.Y); y <= Math.Max(p1.Y, p2.Y); y++)
        {
            yield return p1 with { Y = y };
        }
    }
    else if (p1.Y == p2.Y) // Vertical line
    {
        for (int x = Math.Min(p1.X, p2.X); x <= Math.Max(p1.X, p2.X); x++)
        {
            yield return p1 with { X = x };
        }
    }
    else // not vertical or horizontal
    {
        yield break;
    }
}

static IEnumerable<Point> GetRocks(string path)
{
    var pathPoints = GetPathPoints(path).ToArray();
    for (int r = 1; r < pathPoints.Length; r++)
    {
        foreach(var p in GetLinePoints(pathPoints[r - 1], pathPoints[r]))
        {
            yield return p;
        }
    }
}

static IEnumerable<Point> GetPathPoints(string path)
{
    var points = path.Split(" -> ");
    foreach (var p in points)
    {
        var parts = p.Split(',');
        var point = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
        yield return point;
    }
}

record Point(int X, int Y)
{
    public override string ToString() => $"[{X},{Y}]";

    public Point MoveDown() => this with { Y = Y + 1 };

    public Point MoveLeft() => new (X-1, Y+1);

    public Point MoveRight() => new(X + 1, Y + 1);
}

static class Extensions
{
    public static Point NextPoint(this Dictionary<Point, char> tiles, Point p)
    {
        if (!tiles.ContainsKey(p.MoveDown()))
            return p.MoveDown();

        if (!tiles.ContainsKey(p.MoveLeft()))
            return p.MoveLeft();

        if (!tiles.ContainsKey(p.MoveRight()))
            return p.MoveRight();

        return p;
    }
}