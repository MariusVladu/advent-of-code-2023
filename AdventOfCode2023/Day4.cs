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
