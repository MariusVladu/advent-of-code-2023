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
}

