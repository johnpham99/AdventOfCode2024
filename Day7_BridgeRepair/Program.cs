class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;

    static void Main()
    {
        Dictionary<long, long[]> equations = ReadInputFile("input.txt");

        long[] sums = FindSumOfTrueEquations(equations);
        Console.WriteLine("Part 1: " + sums[PART_ONE]);
        Console.WriteLine("Part 2: " + sums[PART_TWO]);
    }

    private static long[] FindSumOfTrueEquations(Dictionary<long, long[]> equations)
    {
        long sum1 = 0;
        long sum2 = 0;

        foreach (KeyValuePair<long, long[]> equation in equations)
        {
            long answer = equation.Key;
            long[] nums = equation.Value;
            long firstNum = nums[0];

            if (TryOperations(answer, nums, firstNum, 1))
            {
                sum1 += equation.Key;
                sum2 += equation.Key;
            }
            else if (TryOperationsWithConcat(answer, nums, firstNum, 1))
            {
                sum2 += equation.Key;
            }
        }
        return new long[] { sum1, sum2 };
    }

    private static bool TryOperations(long value, long[] equation, long curr, int index)
    {
        if (curr == value && index == equation.Length)
        {
            return true;
        }

        if (index == equation.Length)
        {
            return false;
        }

        long sum = curr + equation[index];
        long product = curr * equation[index];

        return TryOperations(value, equation, sum, index + 1) ||
               TryOperations(value, equation, product, index + 1);

    }

    private static bool TryOperationsWithConcat(long value, long[] equation, long curr, int index)
    {
        if (curr == value && index == equation.Length)
        {
            return true;
        }

        if (index == equation.Length)
        {
            return false;
        }

        long sum = curr + equation[index];
        long product = curr * equation[index];

        string concatenatedString = curr.ToString() + equation[index].ToString();
        long concat = long.Parse(concatenatedString);

        return TryOperationsWithConcat(value, equation, sum, index + 1) ||
               TryOperationsWithConcat(value, equation, product, index + 1) ||
               TryOperationsWithConcat(value, equation, concat, index + 1);
    }

    private static Dictionary<long, long[]> ReadInputFile(string filePath)
    {
        Dictionary<long, long[]> equations = new Dictionary<long, long[]>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file not found!");
            return equations;
        }

        string? line;
        try
        {
            using StreamReader sr = new StreamReader(filePath);
            line = sr.ReadLine();
            while (line != null)
            {
                string[] parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);

                long value = long.Parse(parts[0]);
                long[] equation = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(long.Parse)
                                    .ToArray();

                equations.Add(value, equation);

                line = sr.ReadLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        return equations;
    }
}