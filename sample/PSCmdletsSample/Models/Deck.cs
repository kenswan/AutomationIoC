﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace PSCmdletsSample.Models;

public class Deck : IDeck
{
    private readonly Stack<Card> cards;

    public Deck()
    {
        cards = new Stack<Card>();
        Load();
    }

    public Card Draw() => cards.Pop();

    public void Load()
    {
        Rank[] ranks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToArray();
        Suit[] suits = Enum.GetValues(typeof(Suit)).Cast<Suit>().ToArray();

        foreach (Suit suit in suits)
        {
            foreach (Rank rank in ranks)
            {
                var card = new Card()
                {
                    Rank = rank,
                    Suit = suit
                };

                cards.Push(card);
            }
        }
    }
}
