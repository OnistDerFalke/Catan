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
            knightCards = 1;
            roadBuildCards = 1;
            inventionCards = 1;
            monopolCards = 1;
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

            //Destiny: Show invention popup window
            GameManager.InventionPopupShown = true;
        }

        /// <summary>
        /// Method to invoke after choosing resources by player for invention card
        /// </summary>
        /// <param name="choosedResource">types of chosen resources</param>
        public void InventionCardEffect(List<ResourceType> choosenResources)
        {
            //Destiny: Add chosen resources to player
            if (choosenResources.Count >= 2)
            {
                GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosenResources[0]);
                GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosenResources[1]);
            }
        }

        /// <summary>
        /// Opens the window and decrement number of cards of given type
        /// </summary>
        public void UseMonopolCard()
        {
            monopolCards--;

            //Destiny: Show monopol popup window
            GameManager.MonopolPopupShown = true;
        }

        /// <summary>
        /// Method to invoke after choosing resource by player for monopol card
        /// </summary>
        /// <param name="choosedResource">type of chosen resource</param>
        public void MonopolCardEffect(ResourceType choosenResource)
        {
            //Destiny: Giving the current player resources of a given type from other players
            foreach (Player player in GameManager.Players)
            {
                int playerResourceNumber = player.resources.GetResourceNumber(choosenResource);
                GameManager.Players[GameManager.CurrentPlayer].resources.AddSpecifiedResource(choosenResource, playerResourceNumber);
                player.resources.SubtractSpecifiedResource(choosenResource, playerResourceNumber);
            }
        }
    }
}
