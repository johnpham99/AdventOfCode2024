class Program
{
    private const string inputFile = "input.txt";
    private const string word1 = "XMAS";
    private const string word2 = "MAS";
    static void Main(string[] args)
    {
        string[] input = File.ReadAllLines(inputFile);
        if (input == null)
        {
            Console.WriteLine("Error reading input file.");
            return;
        }

        int partOneMatches = 0;
        int partTwoMatches = 0;

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == word1[0])
                {
                    partOneMatches += WordSearch1(input, i, j, word1, 1);
                }

                if (input[i][j] == word2[word2.Length / 2])
                {
                    partTwoMatches += WordSearch2(input, i, j, word2, word2.Length / 2);
                }
            }
        }

        Console.WriteLine("Part 1: " + partOneMatches);
        Console.WriteLine("Part 2: " + partTwoMatches);
    }

    private static int WordSearch1(string[] input, int i, int j, string word, int wordIndex)
    {
        if (OutOfBounds(input, i, j))
        {
            return 0;
        }

        int matches = 0;
        matches += SearchUp(input, i - 1, j, word, 1, 1);
        matches += SearchUpRight(input, i - 1, j + 1, word, 1, 1);
        matches += SearchRight(input, i, j + 1, word, 1, 1);
        matches += SearchDownRight(input, i + 1, j + 1, word, 1, 1);
        matches += SearchDown(input, i + 1, j, word, 1, 1);
        matches += SearchDownLeft(input, i + 1, j - 1, word, 1, 1);
        matches += SearchLeft(input, i, j - 1, word, 1, 1);
        matches += SearchUpLeft(input, i - 1, j - 1, word, 1, 1);
        return matches;
    }

    private static int WordSearch2(string[] input, int i, int j, string word, int wordIndex)
    {
        if (OutOfBounds(input, i, j))
        {
            return 0;
        }

        /*
        . . S
        . A .
        M . .
        */
        bool forwardLeftToRight = SearchUpRight(input, i - 1, j + 1, word, wordIndex + 1, 1) + SearchDownLeft(input, i + 1, j - 1, word, wordIndex - 1, -1) == 2;

        /*
        . . M
        . A .
        S . .
        */
        bool forwardRightToLeft = SearchUpRight(input, i - 1, j + 1, word, wordIndex - 1, -1) + SearchDownLeft(input, i + 1, j - 1, word, wordIndex + 1, 1) == 2;

        /*
       M . .
       . A .
       . . S
       */
        bool backLeftToRight = SearchUpLeft(input, i - 1, j - 1, word, wordIndex - 1, -1) + SearchDownRight(input, i + 1, j + 1, word, wordIndex + 1, 1) == 2;

        /*
        S . .
        . A .
        . . M
        */
        bool backRightToLeft = SearchUpLeft(input, i - 1, j - 1, word, wordIndex + 1, 1) + SearchDownRight(input, i + 1, j + 1, word, wordIndex - 1, -1) == 2;

        if ((forwardLeftToRight || forwardRightToLeft) && (backLeftToRight | backRightToLeft))
        {
            return 1;
        }

        return 0;
    }

    private static bool OutOfBounds(string[] input, int i, int j)
    {
        if (i < 0 || i == input.Length)
        {
            return true;
        }

        if (j < 0 || j == input[i].Length)
        {
            return true;
        }

        return false;
    }

    private static int SearchUp(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchUp(input, i - 1, j, word, wordIndex + direction, direction);
    }

    private static int SearchUpRight(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchUpRight(input, i - 1, j + 1, word, wordIndex + direction, direction);
    }

    private static int SearchRight(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchRight(input, i, j + 1, word, wordIndex + direction, direction);
    }

    private static int SearchDownRight(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchDownRight(input, i + 1, j + 1, word, wordIndex + direction, direction);
    }

    private static int SearchDown(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchDown(input, i + 1, j, word, wordIndex + direction, direction);
    }

    private static int SearchDownLeft(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchDownLeft(input, i + 1, j - 1, word, wordIndex + direction, direction);
    }

    private static int SearchLeft(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchLeft(input, i, j - 1, word, wordIndex + direction, direction);
    }

    private static int SearchUpLeft(string[] input, int i, int j, string word, int wordIndex, int direction)
    {
        if (wordIndex == word.Length || wordIndex == -1)
        {
            return 1;
        }

        if (OutOfBounds(input, i, j) || input[i][j] != word[wordIndex])
        {
            return 0;
        }

        if (input[i][j] != word[wordIndex])
        {
            return 0;
        }

        return SearchUpLeft(input, i - 1, j - 1, word, wordIndex + direction, direction);
    }

    private static string[]? ConvertFileToArray(string filePath)
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