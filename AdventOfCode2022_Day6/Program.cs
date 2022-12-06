// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 6: Tuning Trouble ---");

string stream = await File.ReadAllTextAsync(@"Input.txt");

const int MARKER_LENGTH = 4;

int p = 0;
bool found = false;

while (!found)
{
    found = stream.Skip(p++).Take(MARKER_LENGTH).Distinct().Count() == MARKER_LENGTH;
}

Console.WriteLine($"start-of-packet marker: {p + MARKER_LENGTH - 1}");

int i = 0;
var position = stream.Select(s => new { i = i, chunk = stream.Substring(i++, MARKER_LENGTH) }).First(c => c.chunk.Distinct().Count() == MARKER_LENGTH).i;

Console.WriteLine($"start-of-packet marker 2: {position + MARKER_LENGTH}");

/*
foreach(var section in sections)
    Console.WriteLine($"Starter: {section.i}. chunk: {section.chunk}");
*/

// 3425