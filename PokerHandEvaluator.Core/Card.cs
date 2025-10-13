namespace PokerHandEvaluator.Core;

public enum Suit { Clubs, Diamonds, Hearts, Spades }
public enum Rank
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

public readonly record struct Card(Rank Rank, Suit Suit)
{
    public static bool TryParse(string token, out Card card)
    {
        card = default;
        if (string.IsNullOrWhiteSpace(token)) return false;
        token = token.Trim().ToLowerInvariant();

        string rankPart = token[..^1];
        char suitChar = token[^1];

        Suit suit = suitChar switch
        {
            'c' => Suit.Clubs,
            'd' => Suit.Diamonds,
            'h' => Suit.Hearts,
            's' => Suit.Spades,
            _ => (Suit)(-1)
        };
        if ((int)suit == -1) return false;

        Rank rank = rankPart switch
        {
            "2" => Rank.Two,
            "3" => Rank.Three,
            "4" => Rank.Four,
            "5" => Rank.Five,
            "6" => Rank.Six,
            "7" => Rank.Seven,
            "8" => Rank.Eight,
            "9" => Rank.Nine,
            "10" or "t" => Rank.Ten,
            "j" => Rank.Jack,
            "q" => Rank.Queen,
            "k" => Rank.King,
            "a" => Rank.Ace,
            _ => (Rank)0
        };
        if ((int)rank == 0) return false;

        card = new Card(rank, suit);
        return true;
    }
}
