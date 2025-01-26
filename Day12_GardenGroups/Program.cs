class Program
{
    private static readonly (int, int)[] directions = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;

    static void Main()
    {
        string[]? map = ReadInputFile("input.txt");
        if (map == null)
        {
            return;
        }

        List<List<(int i, int j)>> regions = FindRegions(map);
        int[] prices = FindTotalPriceOfRegions(regions, map);

        Console.WriteLine("Part 1: " + prices[PART_ONE]);
        Console.WriteLine("Part 2: " + prices[PART_TWO]);
    }

    private static int[] FindTotalPriceOfRegions(List<List<(int i, int j)>> regions, string[] map)
    {
        int priceWithPerimeter = 0;
        int priceWithSides = 0;

        foreach (List<(int, int)> region in regions)
        {
            int area = region.Count;

            int perimeter = GetPerimeterOfRegion(region, map);
            priceWithPerimeter += perimeter * area;

            int sides = GetSidesOfRegion(region, map);
            priceWithSides += sides * area;
        }

        return new int[] { priceWithPerimeter, priceWithSides };
    }

    private static int GetSidesOfRegion(List<(int i, int j)> region, string[] map)
    {
        int sides = 0;
        foreach ((int i, int j) space in region)
        {
            sides += GetSides(space, map);
        }
        return sides;
    }

    private static int GetSides((int i, int j) position, string[] map)
    {
        int sides = 0;

        for (int i = 0; i < directions.Length; i++)
        {
            (int i, int j) dir1 = directions[i];
            (int i, int j) dir2 = directions[(i + 1) % 4];
            (int i, int j) diagonal = (dir1.i + dir2.i, dir1.j + dir2.j);

            (int, int) pos1 = (position.i + dir1.i, position.j + dir1.j);
            (int, int) pos2 = (position.i + dir2.i, position.j + dir2.j);
            (int, int) pos3 = (position.i + diagonal.i, position.j + diagonal.j);

            if (InDifferentRegions(position, pos1, map) &&
                InDifferentRegions(position, pos2, map))
            {
                sides++;
            }

            if (!InDifferentRegions(position, pos1, map) &&
                !InDifferentRegions(position, pos2, map) &&
                InDifferentRegions(position, pos3, map))
            {
                sides++;
            }
        }
        return sides;
    }

    private static int GetPerimeterOfRegion(List<(int i, int j)> region, string[] map)
    {
        int perimeter = 0;
        foreach ((int, int) space in region)
        {
            perimeter += GetEdges(space, map);
        }
        return perimeter;
    }

    private static int GetEdges((int i, int j) position, string[] map)
    {
        char currentRegion = map[position.i][position.j];
        int edges = 0;

        foreach ((int i, int j) direction in directions)
        {
            (int, int) newPosition = (position.i + direction.i, position.j + direction.j);
            if (InDifferentRegions(position, newPosition, map))
            {
                edges++;
            }
        }

        return edges;
    }

    private static List<List<(int, int)>> FindRegions(string[] map)
    {
        List<List<(int, int)>> regions = new List<List<(int, int)>>();

        HashSet<(int, int)> visitedPositions = new HashSet<(int, int)>();
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                (int, int) position = (i, j);
                if (!visitedPositions.Contains(position))
                {
                    List<(int, int)> region = new List<(int, int)>();
                    TraverseRegion(position, map, visitedPositions, region);
                    regions.Add(region);
                }
            }
        }
        return regions;
    }

    private static void TraverseRegion((int i, int j) position, string[] map, HashSet<(int, int)> visitedPositions, List<(int, int)> region)
    {
        if (visitedPositions.Contains(position))
        {
            return;
        }

        visitedPositions.Add(position);
        region.Add(position);

        foreach ((int i, int j) direction in directions)
        {
            (int, int) newPosition = (position.i + direction.i, position.j + direction.j);
            if (!IsOutOfBounds(newPosition, map) && !InDifferentRegions(position, newPosition, map))
            {
                TraverseRegion(newPosition, map, visitedPositions, region);
            }
        }
    }

    private static bool InDifferentRegions((int i, int j) position1, (int i, int j) position2, string[] map)
    {
        if (IsOutOfBounds(position1, map) || IsOutOfBounds(position2, map))
        {
            return true;
        }

        return map[position1.i][position1.j] != map[position2.i][position2.j];
    }

    private static bool IsOutOfBounds((int i, int j) position, string[] map)
    {
        return position.i < 0 || position.i >= map.Length ||
                position.j < 0 || position.j >= map[position.i].Length;
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