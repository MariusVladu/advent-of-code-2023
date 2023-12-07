namespace AdventOfCode2023;

public class Day6
{
    public static string Part1(string[] input)
    {
        var racesAllowedTimes = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var racesBestDistances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        long product = 1;

        for (int i = 0; i < racesAllowedTimes.Length; i++)
        {
            var allowedTime = racesAllowedTimes[i];
            var bestDistance = racesBestDistances[i];

            var (numberOfWaysToBeat, _, _) = GetWaysToWin(allowedTime, bestDistance);

            product *= numberOfWaysToBeat;
        }

        return $"product = {product}";
    }

    public static string Part2(string[] input)
    {
        var allowedTime = long.Parse(input[0].Split(':')[1].Replace(" ", ""));
        var bestDistance = long.Parse(input[1].Split(':')[1].Replace(" ", ""));

        var (numberOfWaysToBeat, _, _) = GetWaysToWin(allowedTime, bestDistance);

        return $"number of ways to beat = {numberOfWaysToBeat}";
    }

    private static (long numberOfWaysToWin, long shortestWinningHold, long longestWinningHold) GetWaysToWin(long allowedTime, long bestDistance)
    {
        // x^2 - allowedTime*x + bestDistance = 0

        var delta = allowedTime * allowedTime - 4 * 1 * bestDistance;
        var shortestWinningHold = (long)Math.Ceiling((allowedTime - Math.Sqrt(delta)) / 2);
        var longestWinningHold = (long)Math.Floor((allowedTime + Math.Sqrt(delta)) / 2);

        if (GetDistance(shortestWinningHold, allowedTime) == bestDistance)
            shortestWinningHold++;

        if (GetDistance(longestWinningHold, allowedTime) == bestDistance)
            longestWinningHold--;

        var numberOfWaysToBeat = longestWinningHold - shortestWinningHold + 1;

        return (numberOfWaysToBeat, shortestWinningHold, longestWinningHold);
    }

    private static long GetDistance(long holdTime, long allowedTime)
    {
        var speed = holdTime * 1;
        return (allowedTime - holdTime) * speed;
    }
}
