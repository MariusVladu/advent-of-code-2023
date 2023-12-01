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
}