namespace AdventOfCode2023;

public class Day9
{
    public static string Part1(string[] input)
    {
        long sum = 0;

        foreach (var line in input)
        {
            var readings = line.Split(' ').Select(int.Parse).ToList();

            var sequences = new List<List<int>> { readings };
            var currentSequence = readings;

            while (currentSequence.Any(x => x != 0))
            {
                var differencesSequence = new List<int>();

                for (int i = 0; i < currentSequence.Count - 1; i++)
                {
                    differencesSequence.Add(currentSequence[i + 1] - currentSequence[i]);
                }

                sequences.Add(differencesSequence);

                currentSequence = sequences.Last();
            }

            sequences.Last().Add(0);

            for (int i = sequences.Count - 2; i >= 0; i--)
            {
                var prediction = sequences[i + 1].Last() + sequences[i].Last();
                sequences[i].Add(prediction);
            }

            sum += sequences[0].Last();
        }

        return sum.ToString();
    }
}
