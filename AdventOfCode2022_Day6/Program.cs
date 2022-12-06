// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 6: Tuning Trouble ---");

string stream = await File.ReadAllTextAsync(@"Input.txt");

int p = 0;
bool found = false;
var checkMarker = new Queue<char>();

while (!found && p < stream.Length)
{
    checkMarker.Enqueue(stream[p++]);

    if (checkMarker.Count() == 4)
    {
        if (checkMarker.Distinct().Count() == 4)
        {
            found = true;
        }
        checkMarker.Dequeue();
    }

}

Console.WriteLine($"start-of-packet marker: {p}");
