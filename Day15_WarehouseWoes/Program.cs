class Program
{
    static void Main()
    {
        char[,]? map = ReadMapFromInput("input.txt");
        if (map == null)
        {
            return;
        }

        char[,]? wideMap = WidenMap(map);
        if (wideMap == null)
        {
            return;
        }

        char[]? moves = ReadMovesFromInput("input.txt");
        if (moves == null)
        {
            return;
        }

        PerformMoves(map, moves);
        int sumOfBoxGps = CalculateSumOfBoxGps(map);
        PrintMap(map);
        Console.WriteLine("Part 1: " + sumOfBoxGps + '\n');

        PerformMoves(wideMap, moves);
        int sumOfWideBoxGps = CalculateSumOfBoxGps(wideMap);
        PrintMap(wideMap);
        Console.WriteLine("Part 2: " + sumOfWideBoxGps);
    }

    private static int CalculateSumOfBoxGps(char[,] map)
    {
        int sum = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 'O' || map[i, j] == '[')
                {
                    sum += (100 * i + j);
                }
            }
        }
        return sum;
    }

    private static void PerformMoves(char[,] map, char[] moves)
    {
        (int i, int j) currPos = ReadStartingPositionFromMap(map);


        foreach (char move in moves)
        {
            currPos = Move(currPos, move, map);
        }
    }

    private static (int, int) Move((int i, int j) pos, char move, char[,] map)
    {
        (int i, int j) dir = GetDirection(move);
        (int i, int j) nextPos = (pos.i + dir.i, pos.j + dir.j);

        if (IsWall(nextPos, map))
        {
            return pos;
        }

        if (IsBox(nextPos, map))
        {
            if (!Push(nextPos, dir, map))
            {
                return pos;
            }
        }

        map[nextPos.i, nextPos.j] = '@';
        map[pos.i, pos.j] = '.';
        return nextPos;
    }

    private static bool Push((int i, int j) box, (int i, int j) dir, char[,] map)
    {
        if (!IsBox(box, map))
        {
            return false;
        }

        if (map[box.i, box.j] == 'O')
        {
            return PushBox(box, dir, map);
        }

        if (map[box.i, box.j] == '[' || map[box.i, box.j] == ']')
        {
            return PushWideBox(box, dir, map);
        }

        return false;
    }

    private static bool PushWideBox((int i, int j) box, (int i, int j) dir, char[,] map)
    {
        (int i, int j) nextPos = (box.i + dir.i, box.j + dir.j);

        if (IsWall(nextPos, map))
        {
            return false;
        }

        if (IsBox(nextPos, map))
        {
            if (!PushWideBox(nextPos, dir, map))
            {
                return false;
            }
        }

        if (dir.i == 0)
        {
            if (map[box.i, box.j] == '[')
            {
                map[nextPos.i, nextPos.j] = '[';
                map[box.i, box.j] = '.';
            }

            if (map[box.i, box.j] == ']')
            {
                map[nextPos.i, nextPos.j] = ']';
                map[box.i, box.j] = '.';
            }
        }
        else
        {
            (int i, int j) otherPart = (box.i, box.j + 1);
            if (map[box.i, box.j] == ']')
            {
                otherPart = (box.i, box.j - 1);
            }

            (int i, int j) otherNextPos = (otherPart.i + dir.i, otherPart.j + dir.j);

            if (IsWall(otherNextPos, map))
            {
                return false;
            }

            if (IsBox(otherNextPos, map))
            {
                if (!PushWideBox(otherNextPos, dir, map))
                {
                    return false;
                }
            }

            map[nextPos.i, nextPos.j] = map[box.i, box.j];
            map[otherNextPos.i, otherNextPos.j] = map[otherPart.i, otherPart.j];

            map[box.i, box.j] = '.';
            map[otherPart.i, otherPart.j] = '.';
        }

        return true;
    }

    private static bool PushBox((int i, int j) box, (int i, int j) dir, char[,] map)
    {
        (int i, int j) nextPos = (box.i + dir.i, box.j + dir.j);
        if (IsWall(nextPos, map))
        {
            return false;
        }

        if (IsBox(nextPos, map))
        {
            if (!Push(nextPos, dir, map))
            {
                return false;
            }
        }

        map[nextPos.i, nextPos.j] = 'O';
        map[box.i, box.j] = '.';
        return true;
    }

    private static bool IsWall((int i, int j) pos, char[,] map)
    {
        return map[pos.i, pos.j] == '#';
    }

    private static bool IsBox((int i, int j) pos, char[,] map)
    {
        return map[pos.i, pos.j] == 'O' ||
            map[pos.i, pos.j] == '[' ||
            map[pos.i, pos.j] == ']';
    }

    private static (int, int) GetDirection(char move)
    {
        if (move == '^')
        {
            return (-1, 0);
        }

        if (move == '>')
        {
            return (0, 1);
        }

        if (move == 'v')
        {
            return (1, 0);
        }

        if (move == '<')
        {
            return (0, -1);
        }

        Console.WriteLine(move + " is not a valid move.");
        return (0, 0);
    }

    private static (int x, int y) ReadStartingPositionFromMap(char[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == '@')
                {
                    return (i, j);
                }
            }
        }
        throw new InvalidOperationException("No guard starting position found!");
    }

    private static char[,]? ReadMapFromInput(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return null;
        }

        var lines = File.ReadAllLines(filePath);
        List<string> mapLines = new List<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            mapLines.Add(line);
        }

        int rows = mapLines.Count;
        int cols = mapLines[0].Length;
        char[,] map = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = mapLines[i][j];
            }
        }

        return map;
    }

    private static char[,]? WidenMap(char[,] map)
    {
        char[,] widerMap = new char[map.GetLength(0), map.GetLength(1) * 2];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == '#')
                {
                    widerMap[i, j * 2] = '#';
                    widerMap[i, j * 2 + 1] = '#';
                }

                if (map[i, j] == 'O')
                {
                    widerMap[i, j * 2] = '[';
                    widerMap[i, j * 2 + 1] = ']';
                }

                if (map[i, j] == '.')
                {
                    widerMap[i, j * 2] = '.';
                    widerMap[i, j * 2 + 1] = '.';
                }

                if (map[i, j] == '@')
                {
                    widerMap[i, j * 2] = '@';
                    widerMap[i, j * 2 + 1] = '.';
                }
            }
        }

        return widerMap;
    }

    private static char[]? ReadMovesFromInput(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return null;
        }

        var lines = File.ReadAllLines(filePath);

        bool isMoveSection = false;
        List<char> moves = new List<char>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isMoveSection = true;
                continue;
            }

            if (isMoveSection && line.All(c => "^>v<".Contains(c)))
            {
                moves.AddRange(line);
            }
        }

        if (moves.Count == 0)
        {
            Console.WriteLine("No moves found in input!");
            return null;
        }

        return moves.ToArray();
    }

    private static void PrintMap(char[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }
            Console.WriteLine();
        }
    }

}

