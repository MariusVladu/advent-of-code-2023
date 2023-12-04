using AdventOfCode2023;

var input = File.ReadAllLines("input.txt");
ArgumentNullException.ThrowIfNull(input);

var result = Day4.Part2(input);

Console.WriteLine(result);