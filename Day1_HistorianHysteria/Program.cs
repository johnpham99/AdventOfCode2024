class Program
{
    static List<int> leftList = new List<int>();
    static List<int> rightList = new List<int>();
    static Dictionary<int, int> rightListCounts = new Dictionary<int, int>();

    static void Main(string[] args)
    {
        ReadInputFileAndPopulateLists("input.txt");

        if (!ValidateAndSortLists())
        {
            Console.WriteLine("Invalid input file...");
            return;
        }

        // Part 1
        int distance = SumDifferencesBetweenLists();
        Console.WriteLine("Distance: " + distance);

        // Part 2
        CountItemsInRightList();
        int similarity = CalculateSimilarityBetweenLists();
        Console.WriteLine("Similarity: " + similarity);
    }
    private static void ReadInputFileAndPopulateLists(string filePath)
    {
        string? line;
        try
        {
            using StreamReader sr = new StreamReader(filePath);
            line = sr.ReadLine();
            while (line != null)
            {
                string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                int number1 = int.Parse(parts[0]);
                int number2 = int.Parse(parts[1]);

                leftList.Add(number1);
                rightList.Add(number2);

                line = sr.ReadLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static int CalculateSimilarityBetweenLists()
    {
        int similarityScore = 0;

        foreach (int value in leftList)
        {
            int occurrence = rightListCounts.GetValueOrDefault(value, 0);
            similarityScore += value * occurrence;
        }

        return similarityScore;
    }

    private static void CountItemsInRightList()
    {
        if (rightListCounts.Count != 0)
        {
            rightListCounts.Clear();
        }

        foreach (int value in rightList)
        {
            rightListCounts[value] = rightListCounts.GetValueOrDefault(value, 0) + 1;
        }
    }

    private static int SumDifferencesBetweenLists()
    {
        int totalDifference = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            int absoluteDifference = CalculateAbsoluteDifference(leftList[i], rightList[i]);
            totalDifference += absoluteDifference;
        }

        return totalDifference;
    }

    private static int CalculateAbsoluteDifference(int x, int y)
    {
        int difference = x - y;

        if (difference < 0)
        {
            difference *= -1;
        }

        return difference;
    }

    private static bool ValidateAndSortLists()
    {
        if (ListsValid())
        {
            leftList.Sort();
            rightList.Sort();
            return true;
        }
        return false;
    }


    private static bool ListsValid()
    {
        return leftList.Count == rightList.Count;
    }

}
