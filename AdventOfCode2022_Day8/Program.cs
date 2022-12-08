// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 8: Treetop Tree House ---");

string[] lines = File.ReadAllLines(@"Input.txt");

// int[,] map = lines.Aggregate(new int[lines.Length, lines[0].Length](), )

var maxX = lines[0].Length;
var maxY = lines.Length;
var map = new int[maxX, maxY];

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        map[x, y] = int.Parse(lines[y][x].ToString());
    }
}


/*
for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        Console.Write(map[x, y]);
    }
    Console.WriteLine();
}
*/

int visible = 0;

for (int y = 0; y < maxY; y++)
{
    for (int x = 0; x < maxX; x++)
    {
        int height = map[x, y];

        visible = map.GetUps(x, y).All(i => i < height) ||
                  map.GetDowns(x, y).All(i => i < height) ||
                  map.GetLefts(x, y).All(i => i < height) ||
                  map.GetRights(x, y).All(i => i < height) ? visible + 1 : visible;
    }
}

Console.WriteLine($"Visible Count: {visible}");

static class Extensions
{
    public static IEnumerable<int> GetUps(this int[,] grid, int startX, int startY)
    {
        for (int y = startY-1; y >= 0; y--)
        {
            yield return grid[startX, y];
        }
    }

    public static IEnumerable<int> GetDowns(this int[,] grid, int startX, int startY)
    {
        int maxY = grid.GetLength(1);
        for (int y = startY + 1; y < maxY; y++)
        {
            yield return grid[startX, y];
        }
    }

    public static IEnumerable<int> GetLefts(this int[,] grid, int startX, int startY)
    {
        for (int x = startX - 1; x >= 0; x--)
        {
            yield return grid[x, startY];
        }
    }

    public static IEnumerable<int> GetRights(this int[,] grid, int startX, int startY)
    {
        int maxX = grid.GetLength(0);
        for (int x = startX + 1; x < maxX; x++)
        {
            yield return grid[x, startY];
        }
    }
}