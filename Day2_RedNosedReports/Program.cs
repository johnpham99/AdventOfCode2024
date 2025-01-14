class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;
    private const int MAX_DIFF = 3;
    private const int MIN_DIFF = 1;
    static void Main(string[] args)
    {
        using StreamReader? inputFile = LoadInputFile("input.txt");

        if (inputFile != null)
        {
            int[] safeReports = CountSafeReports(inputFile);
            Console.WriteLine("# Safe Reports (Part 1) = " + safeReports[PART_ONE]);
            Console.WriteLine("# Safe Reports (Part 2) = " + safeReports[PART_TWO]);
        }
    }

    private static int[] CountSafeReports(StreamReader inputFile)
    {

        int[] safeReports = new int[2];
        string? report;

        try
        {
            report = inputFile.ReadLine();
            while (report != null)
            {
                bool[] reportSafety = CalculateReportSafety(report);
                if (reportSafety[PART_ONE])
                {
                    safeReports[PART_ONE]++;
                }

                if (reportSafety[PART_TWO])
                {
                    safeReports[PART_TWO]++;
                }

                report = inputFile.ReadLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        return safeReports;
    }

    private static bool[] CalculateReportSafety(string? report)
    {
        if (string.IsNullOrWhiteSpace(report))
        {
            return new bool[] { false, false };
        }

        try
        {
            int[]? levels = ParseLevels(report);
            if (levels == null) return new bool[] { false, false };

            bool partOneSafe = LevelsAreSafe(levels);
            bool partTwoSafe = partOneSafe || LevelsAreSafeAfterRemoving(levels);

            return new bool[] { partOneSafe, partTwoSafe };
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            return new bool[] { false, false };
        }
    }

    private static bool LevelsAreSafe(int[] levels)
    {
        bool increasing = true;
        bool decreasing = true;

        for (int i = 0; i < levels.Length - 1; i++)
        {
            int diff = levels[i + 1] - levels[i];

            if (diff >= MIN_DIFF && diff <= MAX_DIFF)
            {
                decreasing = false;
            }
            else if (diff <= -MIN_DIFF && diff >= -MAX_DIFF)
            {
                increasing = false;
            }
            else
            {
                return false;
            }

        }
        return increasing || decreasing;
    }

    private static bool LevelsAreSafeAfterRemoving(int[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (LevelsAreSafeWithoutIndex(levels, i))
            {
                return true;
            }
        }
        return false;
    }

    private static bool LevelsAreSafeWithoutIndex(int[] levels, int indexToSkip)
    {
        bool increasing = true;
        bool decreasing = true;

        for (int i = 0; i < levels.Length - 2; i++)
        {
            int current = i >= indexToSkip ? levels[i + 1] : levels[i];
            int next = i + 1 >= indexToSkip ? levels[i + 2] : levels[i + 1];

            int diff = next - current;

            if (diff >= MIN_DIFF && diff <= MAX_DIFF)
            {
                decreasing = false;
            }
            else if (diff <= -MIN_DIFF && diff >= -MAX_DIFF)
            {
                increasing = false;
            }
            else
            {
                return false;
            }
        }

        return increasing || decreasing;
    }

    private static bool IsValidDifference(int diff)
    {
        return diff <= MAX_DIFF && diff >= -MAX_DIFF;
    }

    private static int[]? ParseLevels(string report)
    {
        try
        {
            string[] parts = report.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(parts, int.Parse);
        }
        catch
        {
            Console.WriteLine("Failed to parse levels: " + report);
            return null;
        }
    }

    private static StreamReader? LoadInputFile(string filePath)
    {
        try
        {
            return new StreamReader(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
            return null;
        }
    }
}