class Coordinate
{
    public long x;
    public long y;

    public Coordinate()
    {
        this.x = 0;
        this.y = 0;
    }

    public Coordinate(Coordinate coordinate)
    {
        this.x = coordinate.x;
        this.y = coordinate.y;
    }

    public void Add(long x, long y)
    {
        this.x += x;
        this.y += y;
    }
    public void Subtract(Coordinate coordinate)
    {
        this.x -= coordinate.x;
        this.y -= coordinate.y;
    }

    public void Subtract(long x, long y)
    {
        this.x -= x;
        this.y -= y;
    }

    public override string ToString()
    {
        return $"({this.x}, {this.y})";
    }

    public void Print()
    {
        Console.WriteLine(this.ToString());
    }

    public static bool operator <(Coordinate c1, Coordinate c2)
    {
        return c1.x < c2.x || (c1.x == c2.x && c1.y < c2.y);
    }

    public static bool operator >(Coordinate c1, Coordinate c2)
    {
        return c1.x > c2.x || (c1.x == c2.x && c1.y > c2.y);
    }

    public static bool operator ==(Coordinate c1, Coordinate c2)
    {
        if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
        {
            return ReferenceEquals(c1, c2);
        }
        return c1.x == c2.x && c1.y == c2.y;
    }

    public static bool operator !=(Coordinate c1, Coordinate c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Coordinate other)
        {
            return this.x == other.x && this.y == other.y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}

class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;
    private const int A_BUTTON_COST = 3;
    private const int B_BUTTON_COST = 1;

    static void Main()
    {
        List<(Coordinate a, Coordinate b, Coordinate prize)>? machines = ReadInputFile("input.txt");
        if (machines == null)
        {
            return;
        }

        long fewestTokensNeededPartOne = 0;
        long fewestTokensNeededPartTwo = 0;
        foreach ((Coordinate a, Coordinate b, Coordinate prize) machine in machines)
        {
            fewestTokensNeededPartOne += CalculateFewestTokensNeeded(machine.a, machine.b, machine.prize);

            machine.prize.Add(10000000000000, 10000000000000);
            fewestTokensNeededPartTwo += CalculateFewestTokensNeeded(machine.a, machine.b, machine.prize);
        }

        Console.WriteLine("Part 1: " + fewestTokensNeededPartOne);
        Console.WriteLine("Part 2: " + fewestTokensNeededPartTwo);
    }

    private static long CalculateFewestTokensNeeded(Coordinate a, Coordinate b, Coordinate prize)
    {
        bool solution = false;
        long tokensNeeded = long.MaxValue;

        /*
        (i * a.x) + (j * b.x) = p.x
        (i * a.y) + (j * b.y) = p.y

        (i * a.x) + (j * b.x) = p.x
        (i * a.x) = p.x - (j * b.x)
        i = (p.x - j * b.x) / a.x
        
        (i * a.y)                           + (j * b.y)         = p.y
        ((p.x - j * b.x) * a.y / a.x )      + (j * b.y)         = p.y
        (p.x - j * b.x) * a.y               + j * b.y * a.x     = p.y * a.x
        (p.x * ay) - ((j * b.x) * a.y)      + (j * b.y * a.x)   = p.y * a.x
        (-j * b.x) * a.y                    + (j * b.y * a.x)   = (p.y * a.x) - (p.x * ay) 
        -j (b.x * a.y)                      + j (b.y * a.x)     = (p.y * a.x) - (p.x * ay)
        j ((b.y * a.x) - (b.x * a.y)) =                         = (p.y * a.x) - (p.x * ay)
        j = (p.y * a.x) - (p.x * ay) / (b.y * a.x) - (b.x * a.y)
        */

        long timesUseB = ((prize.y * a.x) - (prize.x * a.y)) / ((b.y * a.x) - (b.x * a.y));
        long timesUseA = (prize.x - (timesUseB * b.x)) / a.x;

        if ((timesUseB * b.x) + (timesUseA * a.x) == prize.x)
        {
            if (timesUseB * b.y + timesUseA * a.y == prize.y)
            {
                solution = true;
                long tokensUsed = (timesUseB * B_BUTTON_COST) + (timesUseA * A_BUTTON_COST);
                tokensNeeded = Math.Min(tokensUsed, tokensNeeded);
            }
        }

        if (!solution)
        {
            return 0;
        }

        return tokensNeeded;
    }

    private static List<(Coordinate, Coordinate, Coordinate)>? ReadInputFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return null;
        }

        List<(Coordinate, Coordinate, Coordinate)> machines = new List<(Coordinate, Coordinate, Coordinate)>();

        string[] lines = File.ReadAllLines(filePath);
        Coordinate buttonA, buttonB, prize;

        for (int i = 0; i < lines.Length; i += 4)
        {
            string buttonALine = lines[i];
            buttonA = ParseCoordinate(buttonALine, "Button A: X+", "Y+");

            string buttonBLine = lines[i + 1];
            buttonB = ParseCoordinate(buttonBLine, "Button B: X+", "Y+");

            string prizeLine = lines[i + 2];
            prize = ParseCoordinate(prizeLine, "Prize: X=", "Y=");

            machines.Add((buttonA, buttonB, prize));
        }

        return machines;
    }

    private static Coordinate ParseCoordinate(string line, string xPrefix, string yPrefix)
    {
        Coordinate coordinate = new Coordinate();

        int xStart = line.IndexOf(xPrefix) + xPrefix.Length;
        int xEnd = line.IndexOf(",", xStart);
        coordinate.x = long.Parse(line.Substring(xStart, xEnd - xStart));

        int yStart = line.IndexOf(yPrefix) + yPrefix.Length;
        coordinate.y = long.Parse(line.Substring(yStart));

        return coordinate;
    }

}