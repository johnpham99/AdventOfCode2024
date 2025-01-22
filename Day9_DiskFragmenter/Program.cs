class Program
{
    static void Main()
    {
        string? diskMap = ReadFirstLine("input.txt");
        if (diskMap == null)
        {
            return;
        }

        List<(int index, int length)> freeSpaces = new List<(int index, int length)>();
        List<(int index, int length)> files = new List<(int index, int length)>();

        string[] fullLayout = GetFullLayout(diskMap, freeSpaces, files);

        string[] compactLayout = GetCompactLayout(fullLayout, freeSpaces);
        string[] compactLayoutWithFullFile = GetCompactLayoutWithFullFile(fullLayout, freeSpaces, files);

        Console.WriteLine("Part 1: " + CalculateCheckSum(compactLayout));
        Console.WriteLine("Part 2: " + CalculateCheckSum(compactLayoutWithFullFile));
    }

    private static long CalculateCheckSum(string[] compactLayout)
    {
        long checkSum = 0;

        for (int i = 0; i < compactLayout.Length; i++)
        {
            if (compactLayout[i] == ".")
            {
                continue;
            }

            checkSum += (i * int.Parse(compactLayout[i]));
        }

        return checkSum;
    }

    private static string[] GetCompactLayout(string[] fullLayout, List<(int index, int spaces)> freeSpaces)
    {
        string[] compactLayout = new string[fullLayout.Length];
        Array.Copy(fullLayout, compactLayout, fullLayout.Length);

        int swapIndex = compactLayout.Length - 1;

        foreach ((int index, int spaces) freeSpace in freeSpaces)
        {
            for (int i = 0; i < freeSpace.spaces; i++)
            {
                if (freeSpace.index + i >= swapIndex)
                {
                    break;
                }

                while (swapIndex >= 0 && compactLayout[swapIndex] == ".")
                {
                    swapIndex--;
                }

                compactLayout[freeSpace.index + i] = compactLayout[swapIndex];
                compactLayout[swapIndex] = ".";
                swapIndex--;
            }
        }

        return compactLayout;
    }

    private static string[] GetCompactLayoutWithFullFile(string[] fullLayout, List<(int index, int length)> freeSpaces, List<(int index, int length)> files)
    {
        string[] compactLayout = new string[fullLayout.Length];
        Array.Copy(fullLayout, compactLayout, fullLayout.Length);

        HashSet<int> usedFreeSpaces = new HashSet<int>();

        for (int i = files.Count - 1; i >= 0; i--)
        {
            int fileSize = files[i].length;
            int fileIndex = files[i].index;

            for (int j = 0; j < freeSpaces.Count; j++)
            {
                int freeSpaceSize = freeSpaces[j].length;
                int freeSpaceIndex = freeSpaces[j].index;

                if (!usedFreeSpaces.Contains(j) &&
                    fileSize <= freeSpaceSize &&
                    freeSpaceIndex < fileIndex)
                {
                    int spaceIndex = freeSpaceIndex;

                    while (fileSize > 0)
                    {
                        compactLayout[spaceIndex] = compactLayout[fileIndex];
                        compactLayout[fileIndex] = ".";

                        fileIndex++;
                        spaceIndex++;
                        fileSize--;
                        freeSpaceSize--;
                    }

                    if (freeSpaceSize == 0)
                    {
                        usedFreeSpaces.Add(j);
                    }
                    else
                    {
                        freeSpaces[j] = (spaceIndex, freeSpaceSize);
                    }
                    break;
                }
            }
        }
        return compactLayout;
    }

    private static string[] GetFullLayout(string diskMap, List<(int index, int spaces)> freeSpaces, List<(int index, int spaces)> files)
    {
        List<string> fullLayout = new List<string>();
        int id = 0;
        int index = 0;
        bool file = true;

        foreach (char c in diskMap)
        {
            int value = (int)char.GetNumericValue(c);

            if (file)
            {
                files.Add((index, value));
                for (int i = 0; i < value; i++)
                {
                    fullLayout.Add(id.ToString());
                }
                id++;
                file = false;
            }
            else
            {
                freeSpaces.Add((index, value));
                for (int i = 0; i < value; i++)
                {
                    fullLayout.Add(".");
                }
                file = true;
            }

            index += value;
        }

        return fullLayout.ToArray();
    }

    private static string? ReadFirstLine(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}