using AdventOfCode2023;

//var input = File.ReadAllLines("input_sample.txt");
var input = File.ReadAllLines("input.txt");
ArgumentNullException.ThrowIfNull(input);

var result = Day8.Part2(input);

Console.WriteLine(result);
