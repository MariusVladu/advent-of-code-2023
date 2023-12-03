namespace AdventOfCode2023;

public class Day3
{
    public static string Part1(string[] input)
    {
        var sum = 0;

        var rows = input.Length;
        var columns = input[0].Length;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                if (char.IsDigit(input[i][j]))
                {
                    var currentNumber = "";

                    while (j < columns && char.IsDigit(input[i][j]))
                        currentNumber += input[i][j++];

                    if (HasAdjacentSymbol(i, j - currentNumber.Length, j - 1, input))
                        sum += int.Parse(currentNumber);
                }
            }
        }

        return $"sum={sum}";
    }

    private static bool HasAdjacentSymbol(int numberRow, int numberStartCol, int numberEndCol, string[] input)
    {
        var startRow = Math.Max(0, numberRow - 1);
        var endRow = Math.Min(numberRow + 1, input.Length - 1);

        var startCol = Math.Max(0, numberStartCol - 1);
        var endCol = Math.Min(numberEndCol + 1, input[0].Length - 1);

        for (int i = startRow; i <= endRow; i++)
        {
            for (int j = startCol; j <= endCol; j++)
            {
                var currentChar = input[i][j];
                if (!char.IsDigit(currentChar) && currentChar != '.')
                    return true;
            }
        }

        return false;
    }

    public static string Part2(string[] input)
    {
        long sum = 0;

        var rows = input.Length;
        var columns = input[0].Length;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                if (input[i][j] == '*')
                {
                    var gerRatio = GetGearRatio(i, j, input);
                    if (gerRatio > 0)
                        sum += gerRatio;
                }
            }
        }

        return $"sum={sum}";
    }

    private static int GetGearRatio(int row, int column, string[] input)
    {
        var rows = input.Length;
        var columns = input[0].Length;

        var startRow = Math.Max(0, row - 1);
        var endRow = Math.Min(row + 1, rows - 1);

        var startCol = Math.Max(0, column - 1);
        var endCol = Math.Min(column + 1, columns - 1);

        var gearRatio = 1;
        var adjacentNumbers = 0;

        for (int i = startRow; i <= endRow; i++)
        {
            for (int j = startCol; j <= endCol; j++)
            {
                if (char.IsDigit(input[i][j]))
                {
                    var currentNumber = "";

                    int k = j;
                    while (k >= 0 && char.IsDigit(input[i][k]))
                        currentNumber = input[i][k--] + currentNumber;

                    k = j + 1;
                    while (k < columns && char.IsDigit(input[i][k]))
                        currentNumber = currentNumber + input[i][k++];

                    j = k;

                    if (currentNumber != "")
                    {
                        gearRatio *= int.Parse(currentNumber);
                        adjacentNumbers++;
                    }
                }

            }
        }

        return adjacentNumbers == 2 ? gearRatio : -1;
    }
}
