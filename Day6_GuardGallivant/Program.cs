class Program
{
    private static readonly (int i, int j) UP = (-1, 0);
    private static readonly (int i, int j) RIGHT = (0, 1);
    private static readonly (int i, int j) DOWN = (1, 0);
    private static readonly (int i, int j) LEFT = (0, -1);

    static void Main()
    {
        string[]? map = ReadInputFile("input.txt");
        if (map == null)
        {
            return;
        }

        (int i, int j) startPos = FindStartingPosition(map);
        char guardDirection = map[startPos.i][startPos.j];

        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        visited.Add(startPos);

        MoveGuard(map, startPos, guardDirection, visited);

        Console.WriteLine("Part 1: " + visited.Count);

        int loops = FindLoopCausingPositions(map, startPos, guardDirection);

        Console.WriteLine("Part 2: " + loops);
    }

    private static void MoveGuard(string[] map, (int i, int j) position, char direction, HashSet<(int, int)> visited)
    {
        while (true)
        {
            (int i, int j) nextPos = GetNextPosition(position, direction);

            if (OutOfBounds(nextPos, map))
            {
                break;
            }

            if (map[nextPos.i][nextPos.j] == '#')
            {
                direction = RotateGuardRight(direction);
            }
            else
            {
                position = nextPos;
                visited.Add(position);
            }
        }
    }

    private static int FindLoopCausingPositions(string[] map, (int i, int j) startPos, char startDirection)
    {
        int loopCount = 0;

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == '.' && (i, j) != startPos)
                {
                    char[] row = map[i].ToCharArray();
                    row[j] = '#';
                    map[i] = new string(row);

                    if (CausesLoop(map, startPos, startDirection))
                    {
                        loopCount++;
                    }

                    row[j] = '.';
                    map[i] = new string(row);
                }
            }
        }

        return loopCount;
    }

    private static bool CausesLoop(string[] map, (int i, int j) startPos, char startDirection)
    {
        HashSet<(int i, int j, char direction)> visited = new HashSet<(int, int, char)>();
        (int i, int j) position = startPos;
        char direction = startDirection;

        while (true)
        {
            if (visited.Contains((position.i, position.j, direction)))
            {
                return true;
            }

            visited.Add((position.i, position.j, direction));

            (int i, int j) nextPosition = GetNextPosition(position, direction);

            if (OutOfBounds(nextPosition, map))
            {
                break;
            }

            if (map[nextPosition.i][nextPosition.j] == '#')
            {
                direction = RotateGuardRight(direction);
            }
            else
            {
                position = nextPosition;
            }
        }

        return false;
    }

    private static bool OutOfBounds((int i, int j) position, string[] map)
    {
        return position.i < 0 || position.i >= map.Length ||
            position.j < 0 || position.j >= map[position.i].Length;
    }

    private static bool PositionIsObstacle((int i, int j) position, string[] map)
    {
        return map[position.i][position.j] == '#';
    }

    private static char RotateGuardRight(char guard)
    {
        return guard switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new InvalidOperationException("Invalid guard character: " + guard)
        };
    }

    private static (int i, int j) GetNextPosition((int i, int j) position, char guard)
    {
        return guard switch
        {
            '^' => (position.i + UP.i, position.j + UP.j),
            '>' => (position.i + RIGHT.i, position.j + RIGHT.j),
            'v' => (position.i + DOWN.i, position.j + DOWN.j),
            '<' => (position.i + LEFT.i, position.j + LEFT.j),
            _ => throw new InvalidOperationException("Invalid guard character: " + guard)
        };
    }

    private static (int x, int y) FindStartingPosition(string[] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if ("^>v<".Contains(map[i][j]))
                {
                    return (i, j);
                }
            }
        }
        throw new InvalidOperationException("No guard starting position found!");
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