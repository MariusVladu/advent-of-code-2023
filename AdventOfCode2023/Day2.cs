namespace AdventOfCode2023;

public class Day2
{
    public static string Part1(string[] input)
    {
        var maxRed = 12;
        var maxGreen = 13;
        var maxBlue = 14;

        var games = input.Select(line => new Game(line));

        var possibleGames = new List<Game>();

        foreach (var game in games)
        {
            var drawsArePossible = true;

            foreach(var draw in game.Draws)
            {
                if (draw.Red > maxRed) drawsArePossible = false;
                if (draw.Green > maxGreen) drawsArePossible = false;
                if (draw.Blue > maxBlue) drawsArePossible = false;
            }

            if (drawsArePossible)
                possibleGames.Add(game);
        }

        var sum = possibleGames.Sum(x => x.Id);

        return $"sum={sum}";
    }
}

file record Game
{
    public Game(string gameRaw)
    {
        var id = gameRaw.Split(':')[0].Split(' ')[1];
        var drawsRaw = gameRaw.Split(':')[1].Split(';');

        Id = int.Parse(id);
        Draws = drawsRaw.Select(drawRaw => new Draw(drawRaw)).ToList();
    }

    public int Id { get; init; }
    public List<Draw> Draws { get; init; }
}

file class Draw
{
    public Draw(string drawRaw)
    {
        var cubesRaw = drawRaw.Split(',').Select(x => x.Trim());

        foreach (var c in cubesRaw)
        {
            var count = int.Parse(c.Split(' ')[0]);
            var color = c.Split(' ')[1];

            switch (color.ToLower())
            {
                case "red": Red = count; break;
                case "green": Green = count; break;
                case "blue": Blue = count; break;
                default: break;
            }
        }
    }

    public int Red { get; init; }
    public int Green { get; init; }
    public int Blue { get; init; }
}