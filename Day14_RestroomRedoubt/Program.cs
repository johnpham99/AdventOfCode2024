class Robot
{
    public int x;
    public int y;
    public int dx;
    public int dy;

    public Robot()
    {
        this.x = 0;
        this.y = 0;
    }

    public Robot(int x, int y, int dx, int dy)
    {
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
    }

    public void Move(int mapWidth, int mapHeight)
    {
        // this.x = ((this.x + this.dx) % mapWidth + mapWidth) % mapWidth;
        // this.y = ((this.y + this.dy) % mapHeight + mapHeight) % mapHeight;
        this.x += this.dx;
        this.y += this.dy;

        this.x = ((this.x % mapWidth) + mapWidth) % mapWidth;
        this.y = ((this.y % mapHeight) + mapHeight) % mapHeight;
    }

    public void MoveMultipleTimes(int numMoves, int mapWidth, int mapHeight)
    {
        this.x = ((this.x + this.dx * numMoves) % mapWidth + mapWidth) % mapWidth;
        this.y = ((this.y + this.dy * numMoves) % mapHeight + mapHeight) % mapHeight;
    }

    public override string ToString()
    {
        return $"({this.x}, {this.y})";
    }

    public void Print()
    {
        Console.WriteLine(this.ToString());
    }
}

class Program
{
    private const int MAP_WIDTH = 101;
    private const int MAP_HEIGHT = 103;
    private const int TOP_LEFT_QUADRANT = 0;
    private const int TOP_RIGHT_QUADRANT = 1;
    private const int BOTTOM_RIGHT_QUADRANT = 2;
    private const int BOTTOM_LEFT_QUADRANT = 3;

    private const int SYMMETRY_TOLERANCE = 50;

    static void Main()
    {
        List<Robot>? robots = ReadInputFile("input.txt");
        if (robots == null)
        {
            return;
        }


        int[,] map = new int[MAP_HEIGHT, MAP_WIDTH];

        int turns = 0;
        int safetyFactor = 1;
        while (true)
        {
            turns++;
            foreach (Robot robot in robots)
            {
                robot.Move(MAP_WIDTH, MAP_HEIGHT);
                InsertInMap(robot, map);
            }

            if (ContainsConsecutiveOccupiedSpaces(map, 10))
            {
                Console.WriteLine("Part 2: " + turns);
                PrintMap(map);
                break;
            }

            Array.Clear(map, 0, map.Length);

            if (turns == 100)
            {
                int[] quadrants = CountRobotsInQuadrants(robots, MAP_WIDTH, MAP_HEIGHT);
                foreach (int numRobots in quadrants)
                {
                    safetyFactor *= numRobots;
                }
                Console.WriteLine("Part 1: " + safetyFactor); // 229069152
            }

            if (turns == 1000000000)
            {
                break;
            }
        }
    }

    private static bool ContainsConsecutiveOccupiedSpaces(int[,] map, int consecutiveOccupiedSpaces)
    {
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        for (int row = 0; row < height; row++)
        {
            int consecutiveCount = 0;

            for (int col = 0; col < width; col++)
            {
                if (map[row, col] != 0)
                {
                    consecutiveCount++;
                }
                else
                {
                    consecutiveCount = 0;
                }

                if (consecutiveCount >= consecutiveOccupiedSpaces)
                {
                    return true;
                }
            }
        }

        return false;
    }
    private static bool IsHorizontallySymmetric(int[,] map)
    {
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        int count = 0;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width / 2; col++)
            {
                if (map[row, col] != 0 &&
                    map[row, width - 1 - col] != 0)
                {
                    count++;
                }
            }
        }

        if (count >= SYMMETRY_TOLERANCE)
        {
            return true;
        }

        return false;
    }

    private static int[] CountRobotsInQuadrants(List<Robot> robots,
                                                int mapWidth,
                                                int mapHeight)
    {
        int[] quadrants = new int[4];

        int midWidth = mapWidth / 2;
        int midHeight = mapHeight / 2;

        foreach (Robot robot in robots)
        {
            if (robot.x < midWidth && robot.y < midHeight)
            {
                quadrants[TOP_LEFT_QUADRANT]++;
            }

            else if (robot.x > midWidth && robot.y < midHeight)
            {
                quadrants[TOP_RIGHT_QUADRANT]++;
            }

            else if (robot.x > midWidth && robot.y > midHeight)
            {
                quadrants[BOTTOM_RIGHT_QUADRANT]++;
            }

            else if (robot.x < midWidth && robot.y > midHeight)
            {
                quadrants[BOTTOM_LEFT_QUADRANT]++;
            }
        }

        return quadrants;
    }

    private static void InsertInMap(Robot robot, int[,] map)
    {
        map[robot.y, robot.x]++;
    }

    private static void PrintMap(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (map[i, j] != 0)
                {
                    Console.Write(map[i, j]);
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    private static List<Robot>? ReadInputFile(string filePath)
    {
        List<Robot> robots = new List<Robot>();

        foreach (string line in File.ReadLines(filePath))
        {
            string[] parts = line.Split(' ');

            string[] position = parts[0].Substring(2).Split(',');
            int posX = int.Parse(position[0]);
            int posY = int.Parse(position[1]);

            string[] velocity = parts[1].Substring(2).Split(',');
            int velX = int.Parse(velocity[0]);
            int velY = int.Parse(velocity[1]);

            robots.Add(new Robot(posX, posY, velX, velY));
        }

        return robots;
    }
}