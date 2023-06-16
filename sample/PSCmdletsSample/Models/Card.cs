// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace PSCmdletsSample.Models;

public class Card
{
    public Rank Rank { get; set; }

    public Suit Suit { get; set; }

    public override string ToString() =>
        $"{Rank} of {Suit}s";
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}

public enum Suit
{
    Club,
    Diamond,
    Heart,
    Spade
}
