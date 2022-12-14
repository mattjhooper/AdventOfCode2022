// See https://aka.ms/new-console-template for more information
using System.Drawing;
using static Utils;

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

    // Console.Write($"{index}: Compare {packet1} vs {packet2}. ");
    var left = UnpackPacket(packet1);
    var right = UnpackPacket(packet2);

    if (CorrectOrderList(left, right))
    {
        correctPairs.Add(index); 
        // Console.Write($"<----OK");
    }
    // Console.WriteLine();

    index++;
}

Console.WriteLine($"Sum of the indices: {correctPairs.Sum()}");

var part2 = lines.Where(l => !string.IsNullOrEmpty(l)).ToList();
part2.Add("[[2]]");
part2.Add("[[6]]");

var unpacked = part2.Select(UnpackPacket).ToList();
unpacked.Sort(new ListInputComparer());

var asString = unpacked.Select(l => l.ToString()).ToList();
int i1 = 1 + asString.IndexOf("[[2]]");
int i2 = 1 + asString.IndexOf("[[6]]");

Console.WriteLine($"Key 1: {i1}. Key 2: {i2}. Product: {i1 * i2}");


public static class Utils
{
    public static bool IsDigit(char c) => c >= '0' && c <= '9';

    public static ListInput UnpackPacket(string packet)
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

    public static bool CorrectOrder(IInput left, IInput right)
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
            return CorrectOrderList(left.AsListInput(), right.AsListInput());
        }
    }

    public static bool Equal(IInput left, IInput right)
    {
        // Console.WriteLine($"Compare Equal {left} vs {right}.");
        if (left is IntegerInput && right is IntegerInput)
        {
            return EqualIntegerInput(left as IntegerInput, right as IntegerInput);
        }

        if (left is ListInput && right is ListInput)
        {
            return EqualListInput(left as ListInput, right as ListInput);
        }
        return false;
    }

    public static bool CorrectOrderIntegerInput(IntegerInput left, IntegerInput right)
    {
        // Console.WriteLine($"Compare Order {left} vs {right}.");
        if (left.I < right.I)
        {
            // Console.WriteLine("Left side is smaller, so inputs are in the right order");
            return true;
        }
        else if (left.I > right.I)
        {
            // Console.WriteLine("Right side is smaller, so inputs are not in the right order");
            return false;
        }

        // Console.WriteLine("Left side is equal to Right side");
        return true;
    }


    public static bool EqualIntegerInput(IntegerInput left, IntegerInput right) => left.I == right.I;

    public static bool EqualListInput(ListInput left, ListInput right)
    {
        bool equal = true;
        if (left.Children.Count != right.Children.Count)
        {
            equal = false;
        }

        int i = 0;
        while (equal && i < left.Children.Count)
        {
            equal = Equal(left.Children[i], right.Children[i]);
            i++;
        }

        return equal;
    }

    public static bool CorrectOrderList(ListInput left, ListInput right)
    {
        // Console.WriteLine($"Compare {left} vs {right}.");
        for (int x = 0; x < Math.Min(left.Children.Count, right.Children.Count); x++)
        {
            if (Equal(left.Children[x], right.Children[x]))
            {
                continue;
            }

            return CorrectOrder(left.Children[x], right.Children[x]);

            /*
            if (!CorrectOrder(left.Children[x], right.Children[x]))
            {
                Console.WriteLine("CorrectOrderList, order is not correct.");
                return false;
            } 
            */
        }

        if (left.Children.Count <= right.Children.Count)
        {
            // Console.WriteLine("Left side ran out of items, so inputs are in the right order");
            return true;
        }
        else
        {
            // Console.WriteLine("Right side ran out of items, so inputs are not in the right order");
            return false;
        }
    }
}

public class ListInputComparer : IComparer<ListInput>
{
    public int Compare(ListInput? x, ListInput? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        if (EqualListInput(x, y))
        {
            return 0;
        }

        if (CorrectOrderList(x, y))
        {
            return -1;
        }

        return 1;
    }
}

public interface IInput
{
    ListInput? Parent { get; }

    ListInput AsListInput();

    IntegerInput AsIntegerInput();

}

public class ListInput : IInput
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

    public override string ToString()
    {
        return $"[{string.Join(',', Children)}]";
    }
}

public record IntegerInput : IInput
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
        return l;
    }

    public IntegerInput AsIntegerInput() => this;

    public override string ToString() => I.ToString();
}