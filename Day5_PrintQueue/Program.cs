using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    private const int PART_ONE = 0;
    private const int PART_TWO = 1;

    static void Main()
    {
        string inputFilePath = "input.txt";
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Input file not found!");
            return;
        }

        var rules = new List<(int X, int Y)>();
        var updates = new List<List<int>>();

        PopulateRulesAndUpdates(inputFilePath, rules, updates);

        int[] sums = FindSums(rules, updates);
        Console.WriteLine("Part 1: " + sums[PART_ONE]);
        Console.WriteLine("Part 2: " + sums[PART_TWO]);
    }

    private static int[] FindSums(List<(int X, int Y)> rules, List<List<int>> updates)
    {
        int correctMiddleSum = 0;
        int fixedMiddleSum = 0;
        var incorrectUpdates = new List<List<int>>();

        foreach (var update in updates)
        {
            if (IsCorrectlyOrdered(update, rules))
            {
                int middle = update[update.Count / 2];
                correctMiddleSum += middle;
            }
            else
            {
                incorrectUpdates.Add(update);
            }
        }

        foreach (var update in incorrectUpdates)
        {
            var orderedUpdate = OrderUpdate(update, rules);
            int middle = orderedUpdate[orderedUpdate.Count / 2];
            fixedMiddleSum += middle;
        }

        return new int[] { correctMiddleSum, fixedMiddleSum };
    }

    private static void PopulateRulesAndUpdates(string filePath, List<(int X, int Y)> rules, List<List<int>> updates)
    {
        var lines = File.ReadAllLines(filePath);
        bool parsingRules = true;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parsingRules = false;
                continue;
            }

            if (parsingRules)
            {
                var parts = line.Split('|').Select(int.Parse).ToList();
                rules.Add((parts[0], parts[1]));
            }
            else
            {
                var update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }
    }

    private static bool IsCorrectlyOrdered(List<int> update, List<(int X, int Y)> rules)
    {
        var indexMap = update
            .Select((page, index) => (page, index))
            .ToDictionary(p => p.page, p => p.index);

        foreach (var (X, Y) in rules)
        {
            if (indexMap.ContainsKey(X) && indexMap.ContainsKey(Y))
            {
                if (indexMap[X] > indexMap[Y])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static List<int> OrderUpdate(List<int> update, List<(int X, int Y)> rules)
    {
        var graph = new Dictionary<int, List<int>>();
        var inDegree = new Dictionary<int, int>();

        foreach (var page in update)
        {
            graph[page] = new List<int>();
            inDegree[page] = 0;
        }

        foreach (var (X, Y) in rules)
        {
            if (update.Contains(X) && update.Contains(Y))
            {
                graph[X].Add(Y);
                inDegree[Y]++;
            }
        }

        var queue = new Queue<int>();
        foreach (var page in update)
        {
            if (inDegree[page] == 0)
            {
                queue.Enqueue(page);
            }
        }

        var sortedPages = new List<int>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sortedPages.Add(current);

            foreach (var neighbor in graph[current])
            {
                inDegree[neighbor]--;
                if (inDegree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return sortedPages;
    }
}