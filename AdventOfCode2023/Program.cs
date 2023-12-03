using AdventOfCode2023;

var input = File.ReadAllLines("input.txt");
ArgumentNullException.ThrowIfNull(input);

var result = Day3.Part1(input);

Console.WriteLine(result);