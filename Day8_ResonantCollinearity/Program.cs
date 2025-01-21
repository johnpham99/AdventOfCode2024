class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;
    static void Main()
    {
        string[]? map = ReadInputFile("input.txt");
        if (map == null)
        {
            return;
        }

        Dictionary<char, List<(int r, int c)>> antennas = GetAntennaLocations(map);

        int[] antinodes = CountAllAntinodes(map, antennas);
        Console.WriteLine("Part 1: " + antinodes[PART_ONE]);
        Console.WriteLine("Part 2: " + antinodes[PART_TWO]);
    }
    private static int[] CountAllAntinodes(string[] map, Dictionary<char, List<(int r, int c)>> antennas)
    {
        Dictionary<int, HashSet<(int r, int c)>> antinodes = new Dictionary<int, HashSet<(int r, int c)>>();
        antinodes.Add(PART_ONE, new HashSet<(int r, int c)>());
        antinodes.Add(PART_TWO, new HashSet<(int r, int c)>());

        foreach (KeyValuePair<char, List<(int r, int c)>> antenna in antennas)
        {
            CalculateAntinodesOfAntenna(map, antenna.Value, antinodes);
        }

        return new int[] { antinodes[PART_ONE].Count(), antinodes[PART_TWO].Count() };
    }

    private static void CalculateAntinodesOfAntenna(string[] map, List<(int r, int c)> locations, Dictionary<int, HashSet<(int r, int c)>> antinodes)
    {
        foreach ((int r, int c) location1 in locations)
        {
            foreach ((int r, int c) location2 in locations)
            {
                if (location1 != location2)
                {
                    CheckAntinodes(map, location1, location2, antinodes[PART_ONE]);
                    CheckAntinodesExtended(map, location1, location2, antinodes[PART_TWO]);
                }
            }

            if (locations.Count > 1)
            {
                antinodes[PART_TWO].Add(location1);
            }
        }
    }

    private static void CheckAntinodes(string[] map, (int r, int c) location1, (int r, int c) location2, HashSet<(int r, int c)> antinodes)
    {
        /*
        . . . . .
        . . . . .
        . . . . .
        . . B . .
        . . . . A

        A = (4, 4)
        B = (3, 2)

        A - B tells you how to get from B to A
        verticalDiff = 1
        horizontalDiff = 2

        antinode1 = (a.r + verticalDiff, a.c + horizontalDiff)
        antinode1 = (5, 6)

        B - A tells you how to get from A to B

        antinode2 = (b.r - verticalDiff, b.c - horizontalDiff)
        antinode2 = (2, 0) 

        */

        int verticalDiff = location1.r - location2.r;
        int horizontalDiff = location1.c - location2.c;

        (int r, int c) antinode1 = (location1.r + verticalDiff, location1.c + horizontalDiff);
        (int r, int c) antinode2 = (location2.r - verticalDiff, location2.c - horizontalDiff);

        if (InMap(antinode1, map))
        {
            antinodes.Add(antinode1);
        }

        if (InMap(antinode2, map))
        {
            antinodes.Add(antinode2);
        }
    }

    private static void CheckAntinodesExtended(string[] map, (int r, int c) location1, (int r, int c) location2, HashSet<(int r, int c)> antinodes)
    {
        int verticalDiff = location1.r - location2.r;
        int horizontalDiff = location1.c - location2.c;

        (int r, int c) antinode1 = (location1.r + verticalDiff, location1.c + horizontalDiff);
        (int r, int c) antinode2 = (location2.r - verticalDiff, location2.c - horizontalDiff);

        while (InMap(antinode1, map))
        {
            antinodes.Add(antinode1);
            antinode1.r += verticalDiff;
            antinode1.c += horizontalDiff;
        }

        while (InMap(antinode2, map))
        {
            antinodes.Add(antinode2);
            antinode2.r -= verticalDiff;
            antinode2.c -= horizontalDiff;
        }
    }

    private static bool InMap((int r, int c) location, string[] map)
    {
        return location.r > -1 && location.r < map.Length
            && location.c > -1 && location.c < map[location.r].Length;
    }

    private static Dictionary<char, List<(int r, int c)>> GetAntennaLocations(string[] map)
    {
        Dictionary<char, List<(int r, int c)>> antennas = new Dictionary<char, List<(int r, int c)>>();

        for (int r = 0; r < map.Length; r++)
        {
            for (int c = 0; c < map[r].Length; c++)
            {
                char character = map[r][c];
                if (character != '.')
                {
                    if (!antennas.ContainsKey(character))
                    {
                        antennas.Add(character, new List<(int r, int c)>());
                    }
                    antennas[character].Add((r, c));
                }
            }
        }

        return antennas;
    }

    private static string[]? ReadInputFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            return lines;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}