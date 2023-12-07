namespace AdventOfCode2023;

public class Day6
{
    public static string Part1(string[] input)
    {
        var racesAllowedTimes = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var racesBestDistances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        var numberOfWaysToBeat = new int[racesAllowedTimes.Length];
        var product = 1;

        for (int i = 0; i < racesAllowedTimes.Length; i++)
        {
            var allowedTime = racesAllowedTimes[i];
            var bestDistance = racesBestDistances[i];

            // x^2 - allowedTime*x + bestDistance = 0

            var delta = allowedTime * allowedTime - 4 * 1 * bestDistance;
            var shortestWinningHold = (int)Math.Ceiling((allowedTime - Math.Sqrt(delta)) / 2);
            var longestWinningHold = (int)Math.Floor((allowedTime + Math.Sqrt(delta)) / 2);

            if (GetDistance(shortestWinningHold, allowedTime) == bestDistance)
                shortestWinningHold++;

            if(GetDistance(longestWinningHold, allowedTime) == bestDistance)
                longestWinningHold--;

            numberOfWaysToBeat[i] = longestWinningHold - shortestWinningHold + 1;
            product *= numberOfWaysToBeat[i];
        }

        return $"product = {product}";
    }

    private static int GetDistance(int holdTime, int allowedTime)
    {
        var speed = holdTime * 1;
        return (allowedTime - holdTime) * speed;
    }
}
