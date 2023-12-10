using AdventOfCode2023;

//var input = File.ReadAllLines("input_sample.txt");
var input = File.ReadAllLines("input.txt");
ArgumentNullException.ThrowIfNull(input);

var result = Day10.Part1(input);

Console.WriteLine(result);
