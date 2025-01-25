class Program
{
    private const int PART_ONE_BLINKS = 25;
    private const int PART_TWO_BLINKS = 75;

    static void Main()
    {
        List<long> stones = ReadInputFile("input.txt");

        Dictionary<long, long> dp = new Dictionary<long, long>();
        foreach (int stone in stones)
        {
            if (dp.ContainsKey(stone))
            {
                dp[stone]++;
            }
            else
            {
                dp.Add(stone, 1);
            }
        }

        long partOneNumStones = 0;
        long partTwoNumStones = 0;

        for (int i = 0; i < PART_TWO_BLINKS; i++)
        {
            if (i == PART_ONE_BLINKS)
            {
                partOneNumStones = dp.Values.Sum();
            }

            dp = Blink(dp);
        }
        partTwoNumStones = dp.Values.Sum();

        Console.WriteLine("Part 1: " + partOneNumStones);
        Console.WriteLine("Part 2: " + partTwoNumStones);
    }

    private static Dictionary<long, long> Blink(Dictionary<long, long> lastIteration)
    {
        Dictionary<long, long> thisIteration = new Dictionary<long, long>();

        foreach (KeyValuePair<long, long> stone in lastIteration)
        {
            long stoneLastIteration = stone.Key;
            long count = stone.Value;

            if (stoneLastIteration == 0)
            {
                thisIteration[1] = thisIteration.GetValueOrDefault(1) + count;
                continue;
            }

            if (HasEvenNumberOfDigits(stoneLastIteration))
            {
                (long leftStone, long rightStone) split = SplitStone(stoneLastIteration);
                thisIteration[split.leftStone] = thisIteration.GetValueOrDefault(split.leftStone) + count;
                thisIteration[split.rightStone] = thisIteration.GetValueOrDefault(split.rightStone) + count;
                continue;
            }

            thisIteration[stoneLastIteration * 2024] = thisIteration.GetValueOrDefault(stoneLastIteration * 2024) + count;
        }

        return thisIteration;
    }
    private static (long left, long right) SplitStone(long stone)
    {
        string numStr = stone.ToString();
        int length = numStr.Length;

        string leftStr = numStr.Substring(0, length / 2);
        string rightStr = numStr.Substring(length / 2, length / 2);

        long leftStone = long.Parse(leftStr);
        long rightStone = long.Parse(rightStr);

        return (leftStone, rightStone);
    }

    private static bool HasEvenNumberOfDigits(long number)
    {
        string numStr = number.ToString();
        return numStr.Length % 2 == 0;
    }

    private static List<long> ReadInputFile(string filePath)
    {
        List<long> stones = new List<long>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return stones;
        }

        try
        {
            using StreamReader sr = new StreamReader(filePath);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                stones.AddRange(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(long.Parse));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        return stones;
    }
}