// See https://aka.ms/new-console-template for more information
Console.WriteLine("--- Day 13: Distress Signal ---");

var lines = await File.ReadAllLinesAsync("Input.txt");

int offset = 0;
int index = 1;
var correctPairs = new List<int>();

while (offset < lines.Length)
{
    var packet1 = lines[offset++];
    var packet2 = lines[offset++];
    offset++;

    Console.Write($"{index}: Compare {packet1} vs {packet2}. ");
    var left = UnpackPacket(packet1);
    var right = UnpackPacket(packet2);

    if (CorrectOrderList(left, right))
    {
        correctPairs.Add(index); 
        Console.Write($"<----OK");
    }
    Console.WriteLine();

    index++;
}

Console.WriteLine($"Sum of the indices: {correctPairs.Sum()}");

static bool IsDigit(char c) => c >= '0' && c <= '9';

static ListInput UnpackPacket(string packet)
{
    var currList = new ListInput(null);

    int c = 1;
    while (c < packet.Length)
    {
        if (packet[c] == '[')
        {
            var newList = new ListInput(currList);
            currList.Children.Add(newList);
            currList = newList;
            c++;
            continue;
        }

        if (packet[c] == ']')
        {
            currList = currList?.Parent ?? currList;
            c++;
            continue;
        }

        if (packet[c] == ',')
        {
            c++;
            continue;
        }

        var digits = new string(packet.ToCharArray().Skip(c).TakeWhile(IsDigit).ToArray());

        c += digits.Length - 1;

        var val = new IntegerInput(int.Parse(digits), currList);
        currList.Children.Add(val);

        c++;
    }

    return currList;
}

static bool CorrectOrder(IInput left, IInput right)
{
    if (left is ListInput && right is ListInput)
    {
        return CorrectOrderList(left as ListInput, right as ListInput);
    }
    else if (left is IntegerInput && right is IntegerInput)
    {
        return CorrectOrderIntegerInput(left as IntegerInput, right as IntegerInput);
    }
    else
    {
        return CorrectOrderIntegerInput(left.AsIntegerInput(), right.AsIntegerInput());
    }
}

static bool CorrectOrderIntegerInput(IntegerInput left, IntegerInput right) => left.I <= right.I;

static bool CorrectOrderList(ListInput left, ListInput right)
{
    for (int x = 0; x < left.Children.Count; x++)
    {
        if (x >= right.Children.Count)
        {
            return false;
        }

        if (!CorrectOrder(left.Children[x], right.Children[x]))
        {
            return false;
        }
    }
    return left.Children.Count <= right.Children.Count;
}

interface IInput
{
    ListInput? Parent { get; }

    ListInput AsListInput();

    IntegerInput AsIntegerInput();
}

class ListInput : IInput
{
    public ListInput(ListInput? parent)
    {
        Parent = parent;
        Children = new List<IInput>();
    }

    public ListInput? Parent { get; }

    public List<IInput> Children { get; }

    public int Count => Children.Count;

    public ListInput AsListInput() => this;

    public IntegerInput AsIntegerInput() => Children.Count >= 1 ? Children.First().AsIntegerInput() : default;
}

record IntegerInput : IInput
{
    public IntegerInput(int i, ListInput? parent)
    {
        Parent = parent;
        I = i;
    }

    public ListInput? Parent { get; }

    public int I { get; init; }

    public ListInput AsListInput()
    {
        var l = new ListInput(Parent);
        l.Children.Add(this);
        if (Parent != null)
        {
            Parent.Children.Remove(this);
            Parent.Children.Add(l);
        } 
        return l;
    }

    public IntegerInput AsIntegerInput() => this;
}