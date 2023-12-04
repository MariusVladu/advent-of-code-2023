namespace AdventOfCode2023;

public class Day4
{
    public static string Part1(string[] input)
    {
        var sum = 0;

        foreach (var line in input)
        {
            var card = new Card(line);

            var winningNumbers = card.WinningNumbers.Intersect(card.Numbers).ToList();

            sum += (int)Math.Pow(2, winningNumbers.Count - 1);
        }

        return $"sum={sum}";
    }

    public static string Part2(string[] input)
    {
        var copies = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            var card = new Card(input[i]);
            var winningNumbersCount = card.WinningNumbers.Intersect(card.Numbers).Count();

            var totalCardInstancesCount = copies[i] + 1;

            var k = i + 1;
            while (winningNumbersCount > 0 && k < input.Length)
            {
                copies[k] += totalCardInstancesCount;
                k++;
                winningNumbersCount--;
            }
        }

        var totalScratchcards = copies.Select(x => x + 1).Sum();

        return $"total scratchcards = {totalScratchcards}";
    }
}

file record Card
{
    public Card(string line)
    {
        Id = int.Parse(line.Split(" | ")[0].Split(':')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
        WinningNumbers = line.Split(" | ")[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        Numbers = line.Split(" | ")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    }

    public int Id { get; init; }
    public List<int> WinningNumbers { get; init; }
    public List<int> Numbers { get; init; }
}
