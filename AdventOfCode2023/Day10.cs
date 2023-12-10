using System.ComponentModel.Design;
using System.Data.Common;

namespace AdventOfCode2023;

public class Day10
{
    public static string Part1(string[] input)
    {
        var startingPoint = FindStartingPoint(input);

        var steps = new long[input.Length, input[0].Length];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                steps[i, j] = -1;
            }
        }

        steps[startingPoint.I, startingPoint.J] = 0;

        var positionsQueue = new Queue<Position>();
        positionsQueue.Enqueue(startingPoint);

        long maxSteps = 0;

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

                if (stepsToConnectedPipe > maxSteps)
                    maxSteps = stepsToConnectedPipe;
            }
        }

        return $"max steps: {maxSteps}";
    }

    public static string Part2(string[] input)
    {
        var startingPoint = FindStartingPoint(input);

        var steps = new long[input.Length, input[0].Length];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                steps[i, j] = -1;
            }
        }

        steps[startingPoint.I, startingPoint.J] = 0;

        var positionsQueue = new Queue<Position>();
        positionsQueue.Enqueue(startingPoint);

        long maxSteps = 0;

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

                if (stepsToConnectedPipe > maxSteps)
                    maxSteps = stepsToConnectedPipe;
            }
        }


        var enlarged = new List<List<char>>();

        for (int i = 0; i < input.Length; i++)
        {
            enlarged.Add(new List<char>());
            for (int j = 0; j < input[0].Length; j++)
            {
                enlarged.Last().Add(' ');
                enlarged.Last().Add(' ');
            }
            enlarged.Last().Add(' ');

            enlarged.Add(new List<char>());
            for (int j = 0; j < input[0].Length; j++)
            {
                enlarged.Last().Add(' ');
                enlarged.Last().Add(steps[i, j] != -1 ? input[i][j] : ' ');
            }
            enlarged.Last().Add(' ');
        }

        enlarged.Add(new List<char>());
        for (int j = 0; j < input[0].Length; j++)
        {
            enlarged.Last().Add(' ');
            enlarged.Last().Add(' ');
        }
        enlarged.Last().Add(' ');

        Print(enlarged);

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
                    enlarged[enlargedCurrentPosition.I - 1][enlargedCurrentPosition.J] = '|';

                if (!IsOutsideMatrix(s, input) && AreConnectedInMainLoop(currentPosition, s, steps))
                    enlarged[enlargedCurrentPosition.I + 1][enlargedCurrentPosition.J] = '|';

                if (!IsOutsideMatrix(w, input) && AreConnectedInMainLoop(currentPosition, w, steps))
                    enlarged[enlargedCurrentPosition.I][enlargedCurrentPosition.J - 1] = '-';

                if (!IsOutsideMatrix(e, input) && AreConnectedInMainLoop(currentPosition, e, steps))
                    enlarged[enlargedCurrentPosition.I][enlargedCurrentPosition.J + 1] = '-';

                Print(enlarged, sleepMs: 100);
            }
        }

        Print(enlarged);



        var visitedTiles = new bool[enlarged.Count, enlarged[0].Count];

        for (int i = 0; i < enlarged.Count; i++)
        {
            for (int j = 0; j < enlarged[0].Count; j++)
            {
                if (enlarged[i][j] != ' ')
                    continue;

                var queue = new Queue<Position>();
                queue.Enqueue(new Position(i, j));

                var tilesCluster = new List<Position>();

                while (queue.TryDequeue(out var currentTile))
                {
                    if (IsOutsideMatrix(currentTile, enlarged))
                        continue;

                    if (visitedTiles[currentTile.I, currentTile.J])
                        continue;

                    if (enlarged[currentTile.I][currentTile.J] != ' ')
                        continue;

                    tilesCluster.Add(currentTile);
                    visitedTiles[currentTile.I, currentTile.J] = true;

                    enlarged[currentTile.I][currentTile.J] = 'O';

                    var n = new Position(currentTile.I - 1, currentTile.J);
                    var s = new Position(currentTile.I + 1, currentTile.J);
                    var w = new Position(currentTile.I, currentTile.J - 1);
                    var e = new Position(currentTile.I, currentTile.J + 1);

                    queue.Enqueue(n); queue.Enqueue(s); queue.Enqueue(w); queue.Enqueue(e);

                    Print(enlarged, 50);
                }

                var reachedOutside = tilesCluster.Any(tile => tile.I == 0 || tile.I == enlarged.Count - 1 || tile.J == 0 || tile.J == enlarged[0].Count - 1);
                if (!reachedOutside)
                    tilesCluster.ForEach(tile => enlarged[tile.I][tile.J] = 'I');

                Print(enlarged, 500);
            }
        }

        var tilesEnclosedByLoop = 0;
        for (int i = 0; i < enlarged.Count; i++)
        {
            for (int j = 0; j < enlarged[0].Count; j++)
            {
                var isEnlargedTile = i % 2 == 0 || j % 2 == 0;

                if (enlarged[i][j] == 'I')
                {
                    if (!isEnlargedTile)
                        tilesEnclosedByLoop++;
                    else
                        enlarged[i][j] = 'i';
                }
            }
        }

        Print(enlarged);

        return $"Tiles enclosed by the loop: {tilesEnclosedByLoop}";
    }

    private static bool IsOutsideMatrix(Position position, string[] matrix)
    {
        return position.I < 0 || position.I >= matrix.Length || position.J < 0 || position.J >= matrix[0].Length;
    }

    private static bool IsOutsideMatrix(Position position, List<List<char>> matrix)
    {
        return position.I < 0 || position.I >= matrix.Count || position.J < 0 || position.J >= matrix[0].Count;
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

    private static void Print(List<List<char>> matrix, int sleepMs = 0)
    {
        // clear console
        Console.Clear();

        for (int i = 0; i < matrix[0].Count; i++)
            Console.Write('-');
        Console.WriteLine();

        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[0].Count; j++)
                Console.Write(matrix[i][j]);

            Console.WriteLine();
        }

        for (int i = 0; i < matrix[0].Count; i++)
            Console.Write('-');
        Console.WriteLine();

        if (sleepMs > 0)
            Thread.Sleep(sleepMs);
    }

    private record struct Position(int I, int J);
}

