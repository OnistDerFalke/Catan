using DataStorage;
using System.Collections.Generic;
using static Player.Resources;

namespace Player
{
    public class Cards
    {
        //Destiny: Types of cards
        public enum CardType
        {
            Knight,
            RoadBuild,
            Invention,
            Monopol,
            VictoryPoint
        }

        private int knightCards;
        private int roadBuildCards;
        private int inventionCards;
        private int monopolCards;
        private List<CardType> blockedCards;

        public Cards()
        {
            knightCards = 0;
            roadBuildCards = 0;
            inventionCards = 0;
            monopolCards = 0;
            blockedCards = new List<CardType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">type of the card</param>
        /// <returns>number of cards of given type</returns>
        public int GetCardNumber(CardType type)
        {
            switch (type)
            {
                case CardType.Knight:
                    return knightCards;
                case CardType.RoadBuild:
                    return roadBuildCards;
                case CardType.Invention:
                    return inventionCards;
                case CardType.Monopol:
                    return monopolCards;
            }

            return 0;
        }

        /// <summary>
        /// Add card to players properties
        /// </summary>
        /// <param name="type">type of the card added to player properties</param>
        /// <returns>true if player was able to buy a card</returns>
        public bool AddCard(CardType type)
        {
            if (!GameManager.Players[GameManager.CurrentPlayer].CanBuyCard())
                return false;

            GameManager.Players[GameManager.CurrentPlayer].resources.SubtractResources(GameManager.CardPrice);

            switch (type)
            {
                case CardType.Knight:
                    knightCards++;
                    blockedCards.Add(CardType.Knight);
                    break;
                case CardType.RoadBuild:
                    roadBuildCards++;
                    blockedCards.Add(CardType.RoadBuild);
                    break;
                case CardType.Invention:
                    inventionCards++;
                    blockedCards.Add(CardType.Invention);
                    break;
                case CardType.Monopol:
                    monopolCards++;
                    blockedCards.Add(CardType.Monopol);
                    break;
                case CardType.VictoryPoint:
                    GameManager.Players[GameManager.CurrentPlayer].score.AddPoints(Score.PointType.VictoryPoints);
                    break;
            }

            return true;
        }

        /// <summary>
        /// Unblocks all blocked cards
        /// </summary>
        public void UnblockCards()
        {
            blockedCards = new List<CardType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Key: type of card, Value: true if player has one card of given type and it's blocked</returns>
        public Dictionary<CardType, bool> CheckIfMarkAsBlocked()
        {
            Dictionary<CardType, bool> isLastBlocked = new Dictionary<CardType, bool>();

            isLastBlocked.Add(CardType.Invention, 
                (blockedCards.Contains(CardType.Invention) && inventionCards == 1) ? true : false);
            isLastBlocked.Add(CardType.Monopol,
                (blockedCards.Contains(CardType.Monopol) && monopolCards == 1) ? true : false);
            isLastBlocked.Add(CardType.Knight,
                (blockedCards.Contains(CardType.Knight) && knightCards == 1) ? true : false);
            isLastBlocked.Add(CardType.RoadBuild,
                (blockedCards.Contains(CardType.RoadBuild) && roadBuildCards == 1) ? true : false);

            return isLastBlocked;
        }

        public void UseKnightCard()
        {
            knightCards--;

            GameManager.Players[GameManager.CurrentPlayer].MoveThief(true);
        }

        public void UseRoadBuildCard()
        {
            roadBuildCards--;

            GameManager.MovingUserMode = GameManager.MovingMode.TwoPathsForFree;
        }

        public void UseInventionCard()
        {
            inventionCards--;

            // TODO: open the window with resources to choose two of them (they can be the same)
            //temporarily assigned values
            ResourceType choosedResource1 = ResourceType.Wood;
            ResourceType choosedResource2 = ResourceType.Wool;

            //Destiny: Add chosen resources to player
            GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosedResource1);
            GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosedResource2);
        }

        public void UseMonopolCard()
        {
            monopolCards--;

            // TODO: open the window with resources to choose one kind
            //temporarily assigned value
            ResourceType choosedResource = ResourceType.Wood;

            //Destiny: Giving the current player resources of a given type from other players
            foreach (Player player in GameManager.Players)
            {
                int playerResourceNumber = player.resources.GetResourceNumber(choosedResource);
                GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosedResource, playerResourceNumber);
                player.resources.SubtractSpecifiedResource(choosedResource, playerResourceNumber);
            }
        }
    }
}
