class Program
{
    static void Main()
    {
        string[]? map = ReadInputFile("input.txt");
        if (map == null)
        {
            return;
        }

        HashSet<(int i, int j)> trailHeads = FindTrailHeads(map);
        int sum = SumOfTrailHeadScores(trailHeads, map);
        Console.WriteLine("Part 1: " + sum);
    }

    private static int SumOfTrailHeadScores(HashSet<(int i, int j)> trailHeads, string[] map)
    {
        int sum = 0;

        foreach ((int i, int j) trailHead in trailHeads)
        {
            sum += FindScoreOfTrailHead(trailHead, map);
        }

        return sum;
    }

    private static int FindScoreOfTrailHead((int i, int j) trailHead, string[] map)
    {
        HashSet<(int i, int j)> trailTails = new HashSet<(int i, int j)>();
        HashSet<(int i, int j)> visited = new HashSet<(int i, int j)>();
        TraverseMap(trailHead, trailTails, visited, map);
        return trailTails.Count;
    }

    private static void TraverseMap((int i, int j) position,
                                    HashSet<(int i, int j)> trailTails,
                                    HashSet<(int i, int j)> visited,
                                    string[] map)
    {
        if (VisitedOrOutOfBounds(position, visited, map))
        {
            return;
        }

        visited.Add(position);

        int i = position.i;
        int j = position.j;

        if (map[i][j] == '9')
        {
            trailTails.Add(position);
        }

        (int i, int j) upPosition = (position.i - 1, position.j);
        (int i, int j) rightPosition = (position.i, position.j + 1);
        (int i, int j) downPosition = (position.i + 1, position.j);
        (int i, int j) leftPosition = (position.i, position.j - 1);

        (int i, int j)[] newPositions = { upPosition, rightPosition, downPosition, leftPosition };

        foreach ((int i, int j) newPosition in newPositions)
        {
            if (ValidMove(position, newPosition, visited, map))
            {
                TraverseMap(newPosition, trailTails, visited, map);
            }
        }
    }

    private static bool ValidMove((int i, int j) position,
                                 (int i, int j) newPosition,
                                 HashSet<(int i, int j)> visited,
                                 string[] map)
    {
        if (VisitedOrOutOfBounds(newPosition, visited, map))
        {
            return false;
        }

        int positionHeight = (int)char.GetNumericValue(map[position.i][position.j]);
        int newPositionHeight = (int)char.GetNumericValue(map[newPosition.i][newPosition.j]);
        return newPositionHeight - positionHeight == 1;
    }

    private static bool VisitedOrOutOfBounds((int i, int j) position, HashSet<(int i, int j)> visited, string[] map)
    {
        int i = position.i;
        int j = position.j;

        if (i < 0 || i >= map.Length || j < 0 || j >= map[i].Length)
        {
            return true;
        }

        if (visited.Contains(position))
        {
            return true;
        }

        return false;
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