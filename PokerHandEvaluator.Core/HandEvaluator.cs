using System.Text;

namespace PokerHandEvaluator.Core;

public enum HandKind
{
    HighCard = 1, OnePair, TwoPair, ThreeOfAKind,
    Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
}

public sealed class HandEvaluation
{
    public HandKind Kind { get; init; }
    public string Description { get; init; } = "";
}

public sealed class HandEvaluator
{
    public HandEvaluation Evaluate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Enter five cards, e.g. 'Ah As 10c 7d 6s'");

        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length != 5)
            throw new ArgumentException("A hand must contain exactly five cards.");

        var cards = tokens.Select(t =>
        {
            if (!Card.TryParse(t, out var c))
                throw new ArgumentException($"Invalid card: {t}");
            return c;
        }).ToList();

        bool flush = cards.All(c => c.Suit == cards[0].Suit);

        var ranks = cards.Select(c => (int)c.Rank).OrderBy(x => x).ToList();
        bool wheel = ranks.SequenceEqual(new[] { 2, 3, 4, 5, 14 }); // A-2-3-4-5
        bool straight = wheel || ranks.Zip(ranks.Skip(1)).All(p => p.Second - p.First == 1);
        int high = wheel ? 5 : ranks.Last();

        var groups = ranks.GroupBy(r => r).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList();
        int maxCount = groups.First().Count();
        int pairCount = groups.Count(g => g.Count() == 2);

        HandKind kind;
        string desc;

        if (straight && flush && high == 14) { kind = HandKind.RoyalFlush; desc = "Royal Flush"; }
        else if (straight && flush) { kind = HandKind.StraightFlush; desc = $"Straight Flush ({Name(high)}-high)"; }
        else if (maxCount == 4) { kind = HandKind.FourOfAKind; desc = $"Four of a Kind ({Name(groups[0].Key)}s)"; }
        else if (maxCount == 3 && pairCount == 1)
        { kind = HandKind.FullHouse; desc = $"Full House ({Name(groups[0].Key)}s over {Name(groups[1].Key)}s)"; }
        else if (flush) { kind = HandKind.Flush; desc = "Flush"; }
        else if (straight) { kind = HandKind.Straight; desc = $"Straight ({Name(high)}-high)"; }
        else if (maxCount == 3) { kind = HandKind.ThreeOfAKind; desc = $"Three of a Kind ({Name(groups[0].Key)}s)"; }
        else if (pairCount == 2) { kind = HandKind.TwoPair; desc = $"Two Pair ({Name(groups[0].Key)}s and {Name(groups[1].Key)}s)"; }
        else if (pairCount == 1) { kind = HandKind.OnePair; desc = $"Pair of {Name(groups[0].Key)}s"; }
        else { kind = HandKind.HighCard; desc = $"High Card ({Name(high)})"; }

        return new HandEvaluation { Kind = kind, Description = desc };
    }

    private static string Name(int r) => r switch
    {
        11 => "Jacks",
        12 => "Queens",
        13 => "Kings",
        14 => "Aces",
        _ => r.ToString()
    };
}
