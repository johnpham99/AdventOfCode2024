using System.Text.RegularExpressions;
class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;
    private const int MAX_FACTOR_LENGTH = 3;
    private const string METHOD_NAME = "mul(";
    private const string DO_INSTRUCTION = "do()";
    private const string DONT_INSTRUCTION = "don't()";

    static void Main(string[] args)
    {
        string input = ReadFile("input.txt");
        int[] sums = SumOfMulInstructions(input);
        Console.WriteLine("Sum of Mul Instructions (Part 1): " + sums[PART_ONE]);
        Console.WriteLine("Sum of Mul Instructions (Part 2): " + sums[PART_TWO]);
    }
    private static int[] SumOfMulInstructions(string input)
    {
        int partOneSum = 0;
        int partTwoSum = 0;

        bool doFlag = true;

        for (int i = 0; i < input.Length; i++)
        {
            if (SearchForDo(input, i))
            {
                doFlag = true;
            }
            else if (SearchForDont(input, i))
            {
                doFlag = false;
            }


            int product = SearchAndComputeMul(input, i);
            partOneSum += product;

            if (doFlag)
            {
                partTwoSum += product;
            }


        }

        return new int[] { partOneSum, partTwoSum };
    }

    private static bool SearchForDo(string input, int index)
    {
        return input.Substring(index).StartsWith(DO_INSTRUCTION);
    }

    private static bool SearchForDont(string input, int index)
    {
        return input.Substring(index).StartsWith(DONT_INSTRUCTION);
    }

    private static int SearchAndComputeMul(string input, int index)
    {
        if (!input.Substring(index).StartsWith(METHOD_NAME))
        {
            return 0;
        }

        int product = GetProduct(input, index + METHOD_NAME.Length);
        return product;

    }
    private static int GetProduct(string input, int index)
    {
        string argumentOneStr = "";
        string argumentTwoStr = "";

        for (int i = 0; i < MAX_FACTOR_LENGTH; i++)
        {
            if (index == input.Length)
            {
                return 0;
            }

            char character = input[index];
            if (character == ',')
            {
                break;
            }

            if (Char.IsDigit(character))
            {
                argumentOneStr += character;
            }
            index++;
        }

        if (index == input.Length || input[index] != ',')
        {
            return 0;
        }

        int argumentOne = ParseFactor(argumentOneStr);
        if (argumentOne == 0)
        {
            return 0;
        }

        index++;
        for (int i = 0; i < MAX_FACTOR_LENGTH; i++)
        {
            if (index == input.Length)
            {
                return 0;
            }

            char character = input[index];
            if (character == ')')
            {
                break;
            }

            if (Char.IsDigit(character))
            {
                argumentTwoStr += character;
            }
            index++;
        }

        if (index == input.Length || input[index] != ')')
        {
            return 0;
        }

        int argumentTwo = ParseFactor(argumentTwoStr);
        if (argumentTwo == 0)
        {
            return 0;
        }

        return argumentOne * argumentTwo;
    }

    private static int ParseFactor(string input)
    {
        try
        {
            if (input.Length <= MAX_FACTOR_LENGTH)
            {
                int factor = Int32.Parse(input);
                return factor;
            }
            else
            {
                return 0;
            }
        }
        catch (FormatException)
        {
            return 0;
        }
    }

    private static String ReadFile(string filePath)
    {
        string fileContent = "";
        try
        {
            fileContent = File.ReadAllText(filePath);
            fileContent = fileContent.Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return fileContent;
    }
}