// See https://aka.ms/new-console-template for more information
Console.WriteLine("---Day 2: Rock Paper Scissors-- - ");

string[] lines = File.ReadAllLines(@"Input.txt");


int totalScore = 0;

/*
foreach (string line in lines)
{
    var hands = line.Split(' ');

    char opponent = hands[0][0];
    char player = hands[1][0];

    // Console.WriteLine($"Opponent plays {opponent}, I play {player}.");

    //int handscore = ShapeScore(player);
    //handscore += WinLossDraw(opponent, player);
    //totalScore += handscore;
    totalScore += Strategy(opponent, player);
}
*/

totalScore = lines.Select(l => CheckHand(GetHands(l))).Sum();
/*
totalScore = lines
    .Select(l => GetHands(l))
    .Select(p => CheckHand(p))
    .Sum();
*/

Console.WriteLine($"Total Score {totalScore}."); // 12111

static (char opponent, char goal) GetHands(string handsAsString)
{
    var hands = handsAsString.Split(' ');
    return (hands[0][0], hands[1][0]);
}

static int ShapeScore(char shape) => shape switch
{
    'X' => 1,
    'Y' => 2,
    'Z' => 3,
    _ => throw new ArgumentOutOfRangeException(nameof(shape), $"Unexpected shape value: {shape}"),
};

static int WinLossDraw(char opponent, char player) => (opponent, player) switch
{
    // A = Rock, B = Paper, C = Scissors
    // X = Rock, Y = Paper, Z = Scissors
    // Win = 6, Draw = 3, Loss = 0

    ('A', 'X') => 3,
    ('A', 'Y') => 6,
    ('A', 'Z') => 0,
    ('B', 'X') => 0,
    ('B', 'Y') => 3,
    ('B', 'Z') => 6,
    ('C', 'X') => 6,
    ('C', 'Y') => 0,
    ('C', 'Z') => 3,
    _ => throw new ArgumentOutOfRangeException($"Unexpected pair supplied: {opponent} {player}"),
};

static int Strategy(char opponent, char goal) => (opponent, goal) switch
{
    // A = Rock, B = Paper, C = Scissors
    // X = Rock, Y = Paper, Z = Scissors
    // X = lose, Y = Draw, Z = Win
    // Win = 6, Draw = 3, Loss = 0

    ('A', 'X') => 0 + ShapeScore('Z'),
    ('A', 'Y') => 3 + ShapeScore('X'),
    ('A', 'Z') => 6 + ShapeScore('Y'),
    ('B', 'X') => 0 + ShapeScore('X'),
    ('B', 'Y') => 3 + ShapeScore('Y'),
    ('B', 'Z') => 6 + ShapeScore('Z'),
    ('C', 'X') => 0 + ShapeScore('Y'),
    ('C', 'Y') => 3 + ShapeScore('Z'),
    ('C', 'Z') => 6 + ShapeScore('X'),
    _ => throw new ArgumentOutOfRangeException($"Unexpected pair supplied: {opponent} {goal}"),
};

static int CheckHand((char opponent, char goal) hands) => Strategy(hands.opponent, hands.goal);
