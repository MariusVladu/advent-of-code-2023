namespace AdventOfCode2023;

public class Day10
{
    public static string Part1(string[] input)
    {
        var steps = GetStepsMatrix(input);

        long maxSteps = 0;

        for (int i = 0; i < steps.GetLength(0); i++)
        {
            for (int j = 0; j < steps.GetLength(1); j++)
            {
                if (steps[i, j] > maxSteps)
                    maxSteps = steps[i, j];
            }
        }

        return $"max steps: {maxSteps}";
    }

    public static string Part2(string[] input)
    {
        var steps = GetStepsMatrix(input);
        var enlarged = GetEnlargedMatrix(input, steps);

        Print(enlarged, sleepMs: 100);

        var tilesEnclosedByLoop = 0;

        for (int i = 0; i < enlarged.GetLength(0); i++)
        {
            for (int j = 0; j < enlarged.GetLength(1); j++)
            {
                if (enlarged[i, j] != ' ')
                    continue;

                var tilesCluster = FindTilesCluster(enlarged, i, j);

                var reachedOutside = tilesCluster.Any(tile => tile.I == 0 || tile.I == enlarged.GetLength(0) - 1 || tile.J == 0 || tile.J == enlarged.GetLength(1) - 1);
                if (!reachedOutside)
                {
                    foreach (var tile in tilesCluster)
                    {
                        var isEnlargedTile = tile.I % 2 == 0 || tile.J % 2 == 0;

                        if (!isEnlargedTile)
                        {
                            enlarged[tile.I, tile.J] = 'I';
                            tilesEnclosedByLoop++;
                        }
                        else
                            enlarged[tile.I, tile.J] = 'i';
                    }
                }

                Print(enlarged, 500);
            }
        }

        return $"Tiles enclosed by the loop: {tilesEnclosedByLoop}";
    }

    private static long[,] GetStepsMatrix(string[] input)
    {
        var startingPoint = FindStartingPoint(input);

        var steps = new long[input.Length, input[0].Length];
        InitMatrix(steps, -1);

        steps[startingPoint.I, startingPoint.J] = 0;

        var positionsQueue = new Queue<Position>();
        positionsQueue.Enqueue(startingPoint);

        while (positionsQueue.TryDequeue(out var currentPosition))
        {
            var currentSymbol = input[currentPosition.I][currentPosition.J];
            var currentSymbolAvailableConnections = FindAvailableConnectionsForPipe(currentSymbol, currentPosition.I, currentPosition.J);

            var connectedPipes = new List<Position>();

            foreach (var availableConnection in currentSymbolAvailableConnections)
            {
                if (availableConnection.I < 0 || availableConnection.I >= input.Length)
                    continue;

                if (availableConnection.J < 0 || availableConnection.J >= input[0].Length)
                    continue;

                var alreadyVisited = steps[availableConnection.I, availableConnection.J] > -1;
                if (alreadyVisited)
                    continue;

                var adjacentSymbol = input[availableConnection.I][availableConnection.J];
                var adjacentSymbolPossibleConnections = FindAvailableConnectionsForPipe(adjacentSymbol, availableConnection.I, availableConnection.J);

                if (adjacentSymbolPossibleConnections.Contains(currentPosition))
                    connectedPipes.Add(availableConnection);
            }

            foreach (var connectedPipe in connectedPipes)
            {
                positionsQueue.Enqueue(connectedPipe);
                var stepsToConnectedPipe = steps[currentPosition.I, currentPosition.J] + 1;
                steps[connectedPipe.I, connectedPipe.J] = stepsToConnectedPipe;
            }
        }

        return steps;
    }

    private static char[,] GetEnlargedMatrix(string[] input, long[,] steps)
    {
        var enlarged = new char[2 * input.Length + 1, 2 * input[0].Length];
        InitMatrix(enlarged, ' ');

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (steps[i, j] != -1)
                    enlarged[i * 2 + 1, j * 2 + 1] = input[i][j];
            }
        }

        Print(enlarged, sleepMs: 100);

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                var currentPosition = new Position(i, j);
                var enlargedCurrentPosition = new Position(i * 2 + 1, j * 2 + 1);

                var n = new Position(i - 1, j);
                var s = new Position(i + 1, j);
                var w = new Position(i, j - 1);
                var e = new Position(i, j + 1);

                if (!IsOutsideMatrix(n, input) && AreConnectedInMainLoop(currentPosition, n, steps))
                    enlarged[enlargedCurrentPosition.I - 1, enlargedCurrentPosition.J] = '|';

                if (!IsOutsideMatrix(s, input) && AreConnectedInMainLoop(currentPosition, s, steps))
                    enlarged[enlargedCurrentPosition.I + 1, enlargedCurrentPosition.J] = '|';

                if (!IsOutsideMatrix(w, input) && AreConnectedInMainLoop(currentPosition, w, steps))
                    enlarged[enlargedCurrentPosition.I, enlargedCurrentPosition.J - 1] = '-';

                if (!IsOutsideMatrix(e, input) && AreConnectedInMainLoop(currentPosition, e, steps))
                    enlarged[enlargedCurrentPosition.I, enlargedCurrentPosition.J + 1] = '-';

                Print(enlarged, sleepMs: 100);
            }
        }

        return enlarged;
    }

    private static List<Position> FindTilesCluster(char[,] enlarged, int i, int j)
    {
        var queue = new Queue<Position>();
        queue.Enqueue(new Position(i, j));

        var tilesCluster = new List<Position>();

        while (queue.TryDequeue(out var currentTile))
        {
            if (IsOutsideMatrix(currentTile, enlarged))
                continue;

            if (enlarged[currentTile.I, currentTile.J] != ' ')
                continue;

            tilesCluster.Add(currentTile);
            enlarged[currentTile.I, currentTile.J] = 'O';

            queue.Enqueue(new Position(currentTile.I - 1, currentTile.J));
            queue.Enqueue(new Position(currentTile.I + 1, currentTile.J));
            queue.Enqueue(new Position(currentTile.I, currentTile.J - 1));
            queue.Enqueue(new Position(currentTile.I, currentTile.J + 1));

            Print(enlarged, 50);
        }

        return tilesCluster;
    }

    private static bool IsOutsideMatrix(Position position, string[] matrix)
    {
        return position.I < 0 || position.I >= matrix.Length || position.J < 0 || position.J >= matrix[0].Length;
    }

    private static bool IsOutsideMatrix(Position position, char[,] matrix)
    {
        return position.I < 0 || position.I >= matrix.GetLength(0) || position.J < 0 || position.J >= matrix.GetLength(1);
    }

    private static bool AreConnectedInMainLoop(Position p1, Position p2, long[,] steps)
    {
        var p1Steps = steps[p1.I, p1.J];
        var p2Steps = steps[p2.I, p2.J];

        if (p1Steps == -1 || p2Steps == -1)
            return false;

        return Math.Abs(p1Steps - p2Steps) == 1;
    }

    private static List<Position> FindAvailableConnectionsForPipe(char symbol, int symbolI, int simbolJ)
    {
        var possibleConnections = new List<Position>();

        if (symbol == '|')
        {
            possibleConnections.Add(new Position(symbolI - 1, simbolJ));
            possibleConnections.Add(new Position(symbolI + 1, simbolJ));
        }
        else if (symbol == '-')
        {
            possibleConnections.Add(new Position(symbolI, simbolJ - 1));
            possibleConnections.Add(new Position(symbolI, simbolJ + 1));
        }
        else if (symbol == 'L')
        {
            possibleConnections.Add(new Position(symbolI - 1, simbolJ));
            possibleConnections.Add(new Position(symbolI, simbolJ + 1));
        }
        else if (symbol == 'J')
        {
            possibleConnections.Add(new Position(symbolI - 1, simbolJ));
            possibleConnections.Add(new Position(symbolI, simbolJ - 1));
        }
        else if (symbol == '7')
        {
            possibleConnections.Add(new Position(symbolI + 1, simbolJ));
            possibleConnections.Add(new Position(symbolI, simbolJ - 1));
        }
        else if (symbol == 'F')
        {
            possibleConnections.Add(new Position(symbolI + 1, simbolJ));
            possibleConnections.Add(new Position(symbolI, simbolJ + 1));
        }
        else if (symbol == 'S')
        {
            possibleConnections.Add(new Position(symbolI - 1, simbolJ));
            possibleConnections.Add(new Position(symbolI + 1, simbolJ));
            possibleConnections.Add(new Position(symbolI, simbolJ - 1));
            possibleConnections.Add(new Position(symbolI, simbolJ + 1));
        }

        return possibleConnections;
    }

    private static Position FindStartingPoint(string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] == 'S')
                    return new Position(i, j);
            }
        }

        return new Position(0, 0);
    }

    private static void Print(char[,] matrix, int sleepMs = 0)
    {
        Console.Clear();

        for (int i = 0; i < matrix.GetLength(1); i++)
            Console.Write('-');
        Console.WriteLine();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write(matrix[i, j]);

            Console.WriteLine();
        }

        for (int i = 0; i < matrix.GetLength(1); i++)
            Console.Write('-');
        Console.WriteLine();

        if (sleepMs > 0)
            Thread.Sleep(sleepMs);
    }

    private static void InitMatrix<T>(T[,] matrix, T value)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = value;
            }
        }
    }

    private record struct Position(int I, int J);
}

