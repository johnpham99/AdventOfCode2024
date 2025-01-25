class Program
{
    const int PART_ONE = 0;
    const int PART_TWO = 1;

    static void Main()
    {
        string[]? map = ReadInputFile("input.txt");
        if (map == null)
        {
            return;
        }

        HashSet<(int i, int j)> trailHeads = FindTrailHeads(map);
        int[] sums = SumOfTrailHeads(trailHeads, map);
        Console.WriteLine("Part 1: " + sums[PART_ONE]);
        Console.WriteLine("Part 2: " + sums[PART_TWO]);
    }

    private static int[] SumOfTrailHeads(HashSet<(int i, int j)> trailHeads, string[] map)
    {
        int sum1 = 0;
        int sum2 = 0;

        foreach ((int i, int j) trailHead in trailHeads)
        {
            int[] sums = FindSumsOfTrailHead(trailHead, map);
            sum1 += sums[PART_ONE];
            sum2 += sums[PART_TWO];
        }

        return new int[] { sum1, sum2 };
    }

    private static int[] FindSumsOfTrailHead((int i, int j) trailHead, string[] map)
    {
        Dictionary<(int i, int j), int> trailTails = new Dictionary<(int i, int j), int>();

        TraverseMap(trailHead, trailTails, map);

        int rating = 0;
        foreach ((int i, int j) trailTail in trailTails.Keys)
        {
            rating += trailTails[trailTail];
        }

        return new int[] { trailTails.Count, rating };
    }

    private static void TraverseMap((int i, int j) position,
                                    Dictionary<(int i, int j), int> trailTails,
                                    string[] map)
    {
        if (map[position.i][position.j] == '9')
        {
            if (!trailTails.ContainsKey(position))
            {
                trailTails.Add(position, 0);
            }

            trailTails[position] = trailTails[position] + 1;
        }

        (int i, int j) upPosition = (position.i - 1, position.j);
        (int i, int j) rightPosition = (position.i, position.j + 1);
        (int i, int j) downPosition = (position.i + 1, position.j);
        (int i, int j) leftPosition = (position.i, position.j - 1);

        (int i, int j)[] newPositions = { upPosition, rightPosition, downPosition, leftPosition };

        foreach ((int i, int j) newPosition in newPositions)
        {
            if (ValidMove(position, newPosition, map))
            {
                TraverseMap(newPosition, trailTails, map);
            }
        }
    }

    private static bool ValidMove((int i, int j) position,
                                 (int i, int j) newPosition,
                                 string[] map)
    {
        if (IsOutOfBounds(newPosition, map))
        {
            return false;
        }

        int positionHeight = (int)char.GetNumericValue(map[position.i][position.j]);
        int newPositionHeight = (int)char.GetNumericValue(map[newPosition.i][newPosition.j]);
        return newPositionHeight - positionHeight == 1;
    }

    private static bool IsOutOfBounds((int i, int j) position, string[] map)
    {
        int i = position.i;
        int j = position.j;

        return i < 0 || i >= map.Length || j < 0 || j >= map[i].Length;
    }

    private static HashSet<(int i, int j)> FindTrailHeads(string[] map)
    {
        HashSet<(int i, int j)> trailHeads = new HashSet<(int i, int j)>();

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '0')
                {
                    trailHeads.Add((i, j));
                }
            }
        }

        return trailHeads;
    }

    private static string[]? ReadInputFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return null;
        }

        return File.ReadAllLines(filePath);
    }
}