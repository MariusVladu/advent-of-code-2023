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
            var currentNumber = "";

            for (var j = 0; j < columns; j++)
            {
                var currentChar = input[i][j];

                if (char.IsDigit(currentChar))
                {
                    currentNumber += currentChar;
                }
                else
                {
                    if (currentNumber != "")
                    {
                        if (HasAdjacentSymbol(i, j - currentNumber.Length, j - 1, input))
                        {
                            sum += int.Parse(currentNumber);
                        }

                        currentNumber = "";
                    }
                }
            }

            if (currentNumber != "")
            {
                if (HasAdjacentSymbol(i, columns - currentNumber.Length, columns - 1, input))
                {
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
}
