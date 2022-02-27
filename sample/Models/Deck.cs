using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellFocused.Sample.Models
{
    public class Deck
    {
        private readonly Stack<Card> cards;

        public Deck()
        {
            cards = new Stack<Card>();
            Load();
        }

        public Card Draw()
        {
            return cards.Pop();
        }

        public void Load()
        {
            Rank[] ranks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToArray();
            Suit[] suits = Enum.GetValues(typeof(Suit)).Cast<Suit>().ToArray();

            foreach (Suit suit in suits)
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
