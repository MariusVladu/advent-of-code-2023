namespace AdventOfCode2023;

public class Day7
{
    public static string Part1(string[] input)
    {
        var camelCardsHands = input.Select(x => new CamelCards(x)).ToList();
        var orderedCamelCardsHands = camelCardsHands.OrderBy(x => x.Hand).ToList();

        long totalWinnings = 0;

        for (int i = 0; i < orderedCamelCardsHands.Count; i++)
        {
            var rank = i + 1;
            var bid = orderedCamelCardsHands[i].Bid;

            totalWinnings += bid * rank;
        }

        return $"Total winnings: {totalWinnings}";
    }
}

file record CamelCards
{
    public CamelCards(string line)
    {
        var parts = line.Split(" ");

        var cards = parts[0].Select(x => new CamelCard(x)).ToArray();

        Hand = new Hand(cards);
        Bid = int.Parse(parts[1]);
    }

    public Hand Hand { get; init; }
    public int Bid { get; }
}

file class Hand : IComparable<Hand>
{
    public Hand(CamelCard[] cards)
    {
        Cards = cards;
        HandType = GetHandType();
        HandRank = Enum.GetValues<HandType>().Length - (int)HandType;
    }

    public CamelCard[] Cards { get; init; }
    public HandType HandType { get; init; }
    public int HandRank { get; init; }

    public int CompareTo(Hand? other)
    {
        if (other is null)
            return 1;

        if (this.HandRank > other.HandRank)
            return 1;
        else if (this.HandRank < other.HandRank)
            return -1;
        else
        {
            for (int i = 0; i < this.Cards.Length; i++)
            {
                if (this.Cards[i].Strength > other.Cards[i].Strength)
                    return 1;
                else if (this.Cards[i].Strength < other.Cards[i].Strength)
                    return -1;
            }

            return 0;
        }
    }

    private HandType GetHandType()
    {
        var cardGroups = Cards
            .GroupBy(card => card)
            .Select(g => new { Card = g.Key, Count = g.Count() })
            .ToList();

        if (cardGroups.Count == 1)
        {
            return HandType.FiveOfAKind;
        }
        else if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 4))
        {
            return HandType.FourOfAKind;
        }
        else if (cardGroups.Count == 2 && cardGroups.Any(g => g.Count == 3))
        {
            return HandType.FullHouse;
        }
        else if (cardGroups.Count == 3 && cardGroups.Any(g => g.Count == 3))
        {
            return HandType.ThreeOfAKind;
        }
        else if (cardGroups.Count == 3 && cardGroups.Count(g => g.Count == 2) == 2)
        {
            return HandType.TwoPair;
        }
        else if (cardGroups.Count == 4 && cardGroups.Any(g => g.Count == 2))
        {
            return HandType.OnePair;
        }
        else if (cardGroups.Count == 5)
        {
            return HandType.HighCard;
        }
        else
        {
            throw new Exception("Invalid hand");
        }
    }
}

file record CamelCard
{
    private static readonly char[] Cards = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

    public CamelCard(char card)
    {
        if (!Cards.Contains(card))
            throw new Exception("Invalid card");

        Card = card;
        Strength = Cards.Length - Array.IndexOf(Cards, card);
    }

    public char Card { get; init; }
    public int Strength { get; init; }
}

file enum HandType
{
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPair,
    OnePair,
    HighCard
}
