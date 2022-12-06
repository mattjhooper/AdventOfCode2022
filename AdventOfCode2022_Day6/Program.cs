// See https://aka.ms/new-console-template for more information
using System.IO;

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
var position = stream.Select(_ => GetSection(stream, i++, MARKER_LENGTH)).First(c => c.section.Distinct().Count() == MARKER_LENGTH).i;

Console.WriteLine($"start-of-packet marker 2: {position + MARKER_LENGTH}");

/*
foreach(var section in sections)
    Console.WriteLine($"Starter: {section.i}. chunk: {section.chunk}");
*/

// 3425

static (int i, string section) GetSection(string stream, int start, int length) => (start, stream.Substring(start, length));