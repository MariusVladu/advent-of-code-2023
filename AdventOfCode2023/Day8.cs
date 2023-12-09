using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day8
{
    public static string Part1(string[] input)
    {
        var instructions = input[0].ToArray();

        var nodes = new Dictionary<string, (string Left, string Right)>();

        for (int i = 2; i < input.Length; i++)
        {
            var match = Regex.Match(input[i], @"(\w{3}) = \((\w{3}), (\w{3})\)");

            nodes.Add(match.Groups[1].Value, (match.Groups[2].Value, match.Groups[3].Value));
        }

        int steps = 0;
        var currentNode = "AAA";
        int stepNo = 0;

        while(currentNode != "ZZZ")
        {
            if (stepNo >= instructions.Length)
                stepNo = 0;

            var currentStep = instructions[stepNo];

            currentNode = currentStep == 'L'
                ? nodes[currentNode].Left
                : nodes[currentNode].Right;

            steps++;
            stepNo++;
        }

        return $"Number of steps to reach ZZZ = {steps}";
    }

    public static string Part2(string[] input)
    {
        var instructions = input[0].ToArray();

        var nodes = new Dictionary<string, (string Left, string Right)>();

        for (int i = 2; i < input.Length; i++)
        {
            var match = Regex.Match(input[i], @"(\w{3}) = \((\w{3}), (\w{3})\)");

            nodes.Add(match.Groups[1].Value, (match.Groups[2].Value, match.Groups[3].Value));
        }


        var currentNodes = nodes.Keys.Where(n => n.EndsWith("A")).ToList();
        var stepsToReachZ = new List<long>();

        foreach (var node in currentNodes)
        {
            long steps = 0;
            int stepNo = 0;
            var currentNode = node;

            while (currentNode[2] != 'Z')
            {
                if (stepNo >= instructions.Length)
                    stepNo = 0;

                var currentStep = instructions[stepNo];

                currentNode = currentStep == 'L'
                    ? nodes[currentNode].Left
                    : nodes[currentNode].Right;

                steps++;
                stepNo++;
            }

            stepsToReachZ.Add(steps);
        }

        long result = GetLowestCommonMultiple(stepsToReachZ);

        return $"Number of steps to simultaneously reach nodes ending in Z = {result}";
    }

    private static long GetLowestCommonMultiple(List<long> numbers)
    {
        var primeFactorsMaxOccurances = new Dictionary<long, int>();

        foreach (var number in numbers)
        {
            var primeFactors = GetPrimeFactors(number);

            foreach (var primeFactorGroup in primeFactors.GroupBy(x => x))
            {
                var primeFactor = primeFactorGroup.Key;
                var occurences = primeFactorGroup.Count();

                if (primeFactorsMaxOccurances.ContainsKey(primeFactor))
                {
                    if (primeFactorsMaxOccurances[primeFactor] < occurences)
                        primeFactorsMaxOccurances[primeFactor] = occurences;
                }
                else
                {
                    primeFactorsMaxOccurances.Add(primeFactor, occurences);
                }
            }
        }

        long lowestCommonMultiple = 1;

        foreach (var (primeFactor, maxOccurances) in primeFactorsMaxOccurances)
        {
            lowestCommonMultiple *= (long)Math.Pow(primeFactor, maxOccurances);
        }

        return lowestCommonMultiple;
    }

    private static long[] GetPrimeFactors(long number)
    {
        var factors = new List<long>();

        while (number % 2 == 0)
        {
            factors.Add(2);
            number /= 2;
        }

        for (long i = 3; i <= Math.Sqrt(number); i += 2)
        {
            while (number % i == 0)
            {
                factors.Add(i);
                number /= i;
            }
        }

        if (number > 2)
            factors.Add(number);

        return factors.ToArray();
    }
}

