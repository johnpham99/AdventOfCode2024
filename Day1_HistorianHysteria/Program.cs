
// Sort the Arrays
// Iterate and calculate the differences
// Sum the differences

class Program
{
    static List<int> leftList = new List<int>();
    static List<int> rightList = new List<int>();
    static Dictionary<int, int> rightListCounts = new Dictionary<int, int>();

    static void Main(string[] args)
    {
        // Part 1
        readInputFileAndPopulateLists("input.txt");

        leftList.Sort();
        rightList.Sort();

        int distance = sumDifferencesBetweenLists();
        Console.WriteLine("Distance: " + distance);

        // Part 2
        countItemsInRightList();

        int similarity = calculateSimilarityBetweenLists();
        Console.WriteLine("Similarity: " + similarity);
    }
    private static void readInputFileAndPopulateLists(string filePath)
    {
        string? line;
        try
        {
            StreamReader sr = new StreamReader(filePath);
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
            sr.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static int calculateSimilarityBetweenLists()
    {
        if (!listsValid())
        {
            return -1;
        }

        int similarityScore = 0;

        foreach (int value in leftList)
        {
            int occurrence = rightListCounts.GetValueOrDefault(value, 0);
            similarityScore += value * occurrence;
        }

        return similarityScore;
    }

    private static void countItemsInRightList()
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

    private static int sumDifferencesBetweenLists()
    {
        if (!listsValid())
        {
            return -1;
        }

        int totalDifference = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            int absoluteDifference = calculateAbsoluteDifference(leftList[i], rightList[i]);
            totalDifference += absoluteDifference;
        }

        return totalDifference;
    }

    private static int calculateAbsoluteDifference(int x, int y)
    {
        int differnece = x - y;

        if (differnece < 0)
        {
            differnece *= -1;
        }

        return differnece;
    }

    private static bool listsValid()
    {
        if (leftList.Count != rightList.Count ||
             leftList.Count == 0 || rightList.Count == 0)
        {
            return false;
        }

        return true;
    }

}
