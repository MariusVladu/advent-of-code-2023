namespace AdventOfCode2023;

public class Day5
{
    public static string Part1(string[] input)
    {
        var seeds = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        var almanac = new Almanac(input);

        var locations = seeds.Select(almanac.GetSeedLocationNumber).ToArray();

        return $"closest location = {locations.Min()}";
    }
}

file record Almanac
{
    public Almanac(string[] input)
    {
        var lineIterator = input.AsEnumerable().GetEnumerator();
        
        lineIterator.MoveNext();

        while (lineIterator.MoveNext())
        {
            var currentLine = lineIterator.Current;

            switch (currentLine)
            {
                case "seed-to-soil map:": SeedToSoil.IterateAndLoad(lineIterator); break;
                case "soil-to-fertilizer map:": SoilToFertilizer.IterateAndLoad(lineIterator); break;
                case "fertilizer-to-water map:": FertilizerToWater.IterateAndLoad(lineIterator); break;
                case "water-to-light map:": WaterToLight.IterateAndLoad(lineIterator); break;
                case "light-to-temperature map:": LightToTemperature.IterateAndLoad(lineIterator); break;
                case "temperature-to-humidity map:": TemperatureToHumidity.IterateAndLoad(lineIterator); break;
                case "humidity-to-location map:": HumidityToLocation.IterateAndLoad(lineIterator); break;

                default: break;
            }
        }
    }

    public Map SeedToSoil { get; init; } = new();
    public Map SoilToFertilizer { get; init; } = new();
    public Map FertilizerToWater { get; init; } = new();
    public Map WaterToLight { get; init; } = new();
    public Map LightToTemperature { get; init; } = new();
    public Map TemperatureToHumidity { get; init; } = new();
    public Map HumidityToLocation { get; init; } = new();

    public long GetSeedLocationNumber(long seed)
    {
        var soil = SeedToSoil.GetCorrespondingDestinationNumber(seed);
        var fertilizer = SoilToFertilizer.GetCorrespondingDestinationNumber(soil);
        var water = FertilizerToWater.GetCorrespondingDestinationNumber(fertilizer);
        var light = WaterToLight.GetCorrespondingDestinationNumber(water);
        var temperature = LightToTemperature.GetCorrespondingDestinationNumber(light);
        var humidity = TemperatureToHumidity.GetCorrespondingDestinationNumber(temperature);
        var location = HumidityToLocation.GetCorrespondingDestinationNumber(humidity);

        return location;
    }
}

file record Map
{
    public List<Mapping> Mappings { get; init; } = new();

    public void IterateAndLoad(IEnumerator<string> lineIterator)
    {
        while (lineIterator.MoveNext())
        {
            var currentLine = lineIterator.Current;
            if (string.IsNullOrWhiteSpace(currentLine))
                break;

            Mappings.Add(new Mapping(currentLine));
        }
    }

    public long GetCorrespondingDestinationNumber(long sourceNumber)
    {
        foreach (var mapping in Mappings)
        {
            if (mapping.AppliesTo(sourceNumber))
                return mapping.GetCorrespondingDestinationNumber(sourceNumber);
        }

        return sourceNumber;
    }
}

file record Mapping
{
    public Mapping(string mappingLine)
    {
        var parts = mappingLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        DestinationRangeStart = parts[0];
        SourceRangeStart = parts[1];
        RangeLength = parts[2];
    }

    public long DestinationRangeStart { get; init; }
    public long SourceRangeStart { get; init; }
    public long RangeLength { get; init; }

    public bool AppliesTo(long sourceNumber)
    {
        return SourceRangeStart <= sourceNumber && sourceNumber < SourceRangeStart + RangeLength;
    }

    public long GetCorrespondingDestinationNumber(long sourceNumber)
    {
        var difference = sourceNumber - SourceRangeStart;

        return DestinationRangeStart + difference;
    }
}
