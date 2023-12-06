using AdventOfCode2023;

var input = File.ReadAllLines("input.txt");
ArgumentNullException.ThrowIfNull(input);

var result = Day5.Part1(input);

Console.WriteLine(result);