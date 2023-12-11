
using System.Collections.Concurrent;

namespace AdventOfCode2023;

public class Day11
{
    public static string Part1(string[] input)
    {
        Utils.Print(input, sleepMs: 250);
        var image = ApplyCosmicExpansion(input);
        Utils.Print(image, sleepMs: 250);

        var galaxies = new List<Position>();

        for (int i = 0; i < image.GetLength(0); i++)
        {
            for (int j = 0; j < image.GetLength(1); j++)
            {
                if (image[i, j] == '#')
                    galaxies.Add(new Position(i, j));
            }
        }

        var distances = new Distance[galaxies.Count, galaxies.Count];

        Parallel.For(0, galaxies.Count, i =>
        {
            for (int j = 0; j < galaxies.Count; j++)
            {
                if (i == j)
                    continue;

                distances[i, j] = GetDistance(galaxies[i], galaxies[j], image);
            }
        });

        long sum = 0;

        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = 0; j < galaxies.Count; j++)
            {
                sum += distances[i, j].Length;
            }
        }

        sum /= 2;

        return sum.ToString();
    }

    private static char[,] ApplyCosmicExpansion(string[] input)
    {
        var expandedImage = new List<List<char>>();

        var rowsToExpand = new List<int>();
        var columnsToExpand = new List<int>();

        for(int i = 0; i < input.Length; i++)
        {
            var onlySpacesInRow = input[i].All(c => c == '.');

            if (onlySpacesInRow)
                rowsToExpand.Add(i);
        }

        for (int j = 0; j < input[0].Length; j++)
        {
            var onlySpacesInColumn = true;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i][j] != '.')
                {
                    onlySpacesInColumn = false;
                    break;
                }
            }

            if (onlySpacesInColumn)
                columnsToExpand.Add(j);
        }

        for (int i = 0; i < input.Length; i++)
        {
            var row = new List<char>();

            for (int j = 0; j < input[0].Length; j++)
            {
                row.Add(input[i][j]);
                if (columnsToExpand.Contains(j))
                    row.Add(input[i][j]);
            }

            expandedImage.Add(row);
            if (rowsToExpand.Contains(i))
                expandedImage.Add(new List<char>(row));
        }

        return Utils.To2DimensionalArray(expandedImage);
    }

    private static Distance GetDistance(Position from, Position to, char[,] image)
    {
        int[,] distances = new int[image.GetLength(0), image.GetLength(1)];
        Utils.InitMatrix(distances, -1);

        var queue = new Queue<(Position pos, int prevDist)>();
        queue.Enqueue((from, -1));

        while(queue.TryDequeue(out var current))
        {
            var currentPosition = current.pos;

            if (Utils.IsOutsideMatrix(currentPosition.I, currentPosition.J, distances))
                continue;

            if (distances[currentPosition.I, currentPosition.J] != -1)
                continue;

            var currentDistance = current.prevDist + 1;
            distances[currentPosition.I, currentPosition.J] = currentDistance;

            if (currentPosition == to)
                return new Distance(from, to, currentDistance);

            queue.Enqueue((new Position(currentPosition.I - 1, currentPosition.J), currentDistance));
            queue.Enqueue((new Position(currentPosition.I + 1, currentPosition.J), currentDistance));
            queue.Enqueue((new Position(currentPosition.I, currentPosition.J - 1), currentDistance));
            queue.Enqueue((new Position(currentPosition.I, currentPosition.J + 1), currentDistance));
        }

        throw new Exception("No path found");
    }

    private record struct Position(int I, int J);
    private record struct Distance(Position From, Position To, int Length);
}
