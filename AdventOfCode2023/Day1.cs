namespace AdventOfCode2023;

public class Day1
{
    public static string Part1(string[] input)
    {
        var sum = 0;

        foreach (var line in input)
        {
            var firstDigit = line.First(char.IsDigit);
            var lastDigit = line.Last(char.IsDigit);

            var number = int.Parse($"{firstDigit}{lastDigit}");
            sum += number;
        }

        return $"sum={sum}";
    }

    public static string Part2(string[] input)
    {
        var sum = 0;

        foreach (var line in input)
        {
            var digits = ExtractDigits(line);
            if (digits.Count == 0)
                throw new InvalidOperationException($"The following line does not contain any digit: {line}");

            var number = int.Parse($"{digits.First()}{digits.Last()}");
            sum += number;
        }

        return $"sum={sum}";
    }

    private static readonly string[] Digits = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    private static List<int> ExtractDigits(string line)
    {
        var foundDigits = new List<int>();

        for (var i = 0; i < line.Length; i++)
        {
            if (char.IsDigit(line[i]))
            {
                foundDigits.Add(line[i] - '0');
                continue;
            }

            for (int j = 0; j < Digits.Length; j++)
            {
                var digit = Digits[j];

                var possibleDigit = line.Substring(i, Math.Min(digit.Length, line.Length - i));
                if (possibleDigit == digit)
                {
                    foundDigits.Add(j);
                    i += digit.Length - 2;
                    break;
                }
            }
        }

        return foundDigits;
    }

}