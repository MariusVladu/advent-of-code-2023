namespace AdventOfCode2023;

public class Day12
{
    private static char[] symbols = ['.', '#'];

    public static string Part1(string[] input)
    {
        foreach (var line in input)
        {
            var report = line.Split(' ')[0];
            var damagedBlocks = line.Split(' ')[1].Split(',').Select(int.Parse).ToArray();

            currentSolution = new char[report.Length];
            ComputePossibleSolutions(0, report.ToArray(), damagedBlocks);
        }

        return count.ToString();
    }

    private static int count;
    private static char[] currentSolution = null!;
    private static void ComputePossibleSolutions(int k, char[] report, int[] damagedBlocks)
    {
        if (k == report.Length)
        {
            if (IsValid(k - 1, currentSolution, damagedBlocks))
            {
                count++;
                Console.WriteLine(currentSolution);
            }

            return;
        }

        if (report[k] != '?')
        {
            currentSolution[k] = report[k];
            ComputePossibleSolutions(k + 1, report, damagedBlocks);
        }
        else
        {
            foreach (var symbol in symbols)
            {
                currentSolution[k] = symbol;

                if (IsValid(k, currentSolution, damagedBlocks))
                {
                    if (k < report.Length)
                        ComputePossibleSolutions(k + 1, report, damagedBlocks);
                }
            }
        }
    }

    private static bool IsValid(int k, char[] report, int[] damagedBlocks)
    {
        var damangedBlockIndex = 0;

        for (int i = 0; i <= k; i++)
        {
            if (report[i] == '#')
            {
                if (damangedBlockIndex >= damagedBlocks.Length)
                    return false;

                if (i > 0 && report[i - 1] != '.')
                    return false;

                var allowedDamagedInARow = damagedBlocks[damangedBlockIndex++];
                var lastIndexOfDamagedBlock = i + allowedDamagedInARow - 1;

                if (lastIndexOfDamagedBlock >= report.Length)
                    return false;

                for (; i <= Math.Min(lastIndexOfDamagedBlock, k); i++)
                {
                    if (report[i] != '#')
                        return false;
                }

                if (i <= k && report[i] != '.')
                    return false;
            }
        }

        if (k == report.Length - 1 && damangedBlockIndex != damagedBlocks.Length)
            return false;

        return true;
    }
}
