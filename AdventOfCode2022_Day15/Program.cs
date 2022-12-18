// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net.Sockets;

Console.WriteLine("--- Day 15: Beacon Exclusion Zone ---");

var inputLines = await File.ReadAllLinesAsync("Input.txt");

var sensors = inputLines.Select(ProcessLine).ToList();

//var excludedPoints = sensors.SelectMany(s => GetExcludedPoints(s.sensor, s.distance, 10)).ToHashSet();
//var pointsMinusSensorsBeacons = excludedPoints.Except(sensors.Select(x => x.sensor)).Except(sensors.Select(x => x.beacon));

//Console.WriteLine($"Excluded Points: {excludedPoints.Count()}. Without Sensors: {pointsMinusSensorsBeacons.Count()}");

/*
var allExcludedPoints = sensors.SelectMany(s => GetAllExcludedPoints(s.sensor, s.distance)).ToHashSet();

var pos = GetRange(0, 4000000).First(p => !allExcludedPoints.Contains(p));

Console.WriteLine($"Position: {pos}. Frequency: {pos.Y + pos.X * 4000000}");
*/

bool keepLooking = true;
int y = 0;
int x = 0;

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

while (keepLooking && y <= 4000000)
{
    if (y % 10000 == 0)
    {
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine($"Y: {y}. RunTime {elapsedTime}");
    }

    
    var xPoints = Enumerable.Range(0, 4000000).ToList();

    int i = 0;

    while (xPoints.Count > 1 && i < sensors.Count)
    {
        var r = GetXRange(sensors[i].sensor, sensors[i].distance, y);
        xPoints.RemoveAll(n => r.minX <= n && n <= r.maxX);    
        i++;
    }

    if (xPoints.Count == 1)
    {
        keepLooking = false;
        x = xPoints.Single();
        continue;
    }

    y++;
}

Console.WriteLine($"Position: [{x},{y}]. Frequency: {y + x * 4000000}");

static (Point sensor, int distance, Point beacon) ProcessLine(string line)
{
    int start = 12;
    int end = line.IndexOf(',');

    int sensorX = int.Parse(line.Substring(start, end - start));
    start = end + 4;
    end = line.IndexOf(":", start);
    int sensorY = int.Parse(line.Substring(start, end - start));

    start = end + 25;
    end = line.IndexOf(",", start);
    int beaconX = int.Parse(line.Substring(start, end - start));
    int beaconY = int.Parse(line.Substring(end + 4));

    var sensor = new Point(sensorX, sensorY);
    var beacon = new Point(beaconX, beaconY);
    // Console.WriteLine($"Sensor at {sensor}: closest beacon is at {beacon}, which is {sensor.GetDistance(beacon)} distance.");

    return (sensor, sensor.GetDistance(beacon), beacon);
}

static IEnumerable<Point> GetExcludedPoints(Point sensor, int distance, int y)
{
    int yDelta = Math.Abs(y - sensor.Y);
    if  (yDelta > distance) yield break;

    for (int x = sensor.X - (distance - yDelta); x <= sensor.X + (distance - yDelta); x++)
    {
        yield return new Point(x, y);
    }
}

static IEnumerable<Point> GetAllExcludedPoints(Point sensor, int distance)
{
    for (int y = sensor.Y - distance; y <= sensor.Y + distance; y++)
    {
        foreach (var p in GetExcludedPoints(sensor, distance, y))
            yield return p;
    }
}

static IEnumerable<Point> GetRange(int min, int max)
{
    for (int x = 0; x <= max; x++)
        for (int y = 0; y <= max; y++)
            yield return new Point(x, y);
}

static (int minX, int maxX) GetXRange(Point sensor, int distance, int y)
{
    int yDelta = Math.Abs(y - sensor.Y);
    return (sensor.X - (distance - yDelta), sensor.X + (distance - yDelta));
}

record Point(int X, int Y)
{
    public override string ToString() => $"[{X},{Y}]";

    public int GetDistance(Point p)
    {
        return Math.Abs(p.X - X) + Math.Abs(p.Y - Y);
    }
}