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
    private record struct Position(int I, int J);
}

