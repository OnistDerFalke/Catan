using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using static Player.Cards;
using static Player.Resources;

namespace Assets.Scripts.DataStorage.Managers
{
    public class CardsManager
    {        
        //Destiny: Deck (pile of cards)
        public List<CardType> Deck = new();

        //Destiny: Minimum values to get reward
        public int RewardedKnightCardNumber = 3;

        //Destiny: Price of card
        public Dictionary<ResourceType, int> CardPrice = new();

        public void Setup()
        {
            //Destiny: Setting up price of card
            CardPrice = new Dictionary<ResourceType, int>();
            CardPrice.Add(ResourceType.Wheat, 1);
            CardPrice.Add(ResourceType.Wool, 1);
            CardPrice.Add(ResourceType.Iron, 1);

            //Destiny: Create Deck
            Shuffle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if current player can use card</returns>
        public bool CheckIfCurrentPlayerCanUseCard(CardType cardType)
        {
            //Destiny: if player already used a card this round
            if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].canUseCard)
            {
                return false;
            }

            //Destiny: if player has not card of given type
            if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.GetCardNumber(cardType) <= 0)
            {
                return false;
            }

            //Destiny: if player has blocked card of given type (player bought it this round)
            var currentPlayerBlockedCards = 
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.CheckIfMarkAsBlocked();
            if (currentPlayerBlockedCards[cardType])
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates deck and shuffles the cards
        /// </summary>
        private void Shuffle()
        {
            Dictionary<CardType, int> CardsNumber = new();
            CardsNumber.Add(CardType.Knight, 14);
            CardsNumber.Add(CardType.VictoryPoint, 5);
            CardsNumber.Add(CardType.RoadBuild, 2);
            CardsNumber.Add(CardType.Monopol, 2);
            CardsNumber.Add(CardType.Invention, 2);
            List<CardType> orderedDeck = new();

            //Destiny: Create deck with proper number of cards
            foreach (KeyValuePair<CardType, int> cardNumber in CardsNumber)
            {
                for (int i = 0; i < cardNumber.Value; i++)
                {
                    orderedDeck.Add(cardNumber.Key);
                }
            }

            //Destiny: Shuffle the cards
            Deck = orderedDeck.OrderBy(x => Guid.NewGuid().ToString()).ToList();
        }
    }
}
