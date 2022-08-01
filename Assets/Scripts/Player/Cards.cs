using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using static Board.States.GameState;
using static Player.Resources;

namespace Player
{
    [Serializable]
    public class Cards
    {
        //Destiny: Types of cards
        public enum CardType
        {
            Knight,
            RoadBuild,
            Invention,
            Monopol,
            VictoryPoint,
            None
        }

        private int knightCards;
        private int roadBuildCards;
        private int inventionCards;
        private int monopolCards;
        private int usedKnightCards;
        private List<CardType> blockedCards;

        public Cards()
        {
            knightCards = 0;
            roadBuildCards = 0;
            inventionCards = 0;
            monopolCards = 0;
            usedKnightCards = 0;
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
            if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].CanBuyCard())
                return false;

            GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                .SubtractResources(GameManager.CardsManager.CardPrice);

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
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].score.AddPoints(Score.PointType.VictoryPoints);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Number of knight cards used by the player</returns>
        public int GetUsedKnightCardsNumber()
        {
            return usedKnightCards;
        }

        public void UseKnightCard()
        {
            knightCards--;
            usedKnightCards++;

            //TODO: sprawdzic te warunki bo są złe - coś nie działa!!!

            //Destiny: If player used more than 3 knight cards or exactly 3 knight cards and 
            //any player didn't use more knight cards then give him points
            if (usedKnightCards >= GameManager.CardsManager.RewardedKnightCardNumber && !GameManager.State.Players.Any(player => 
                player.index != GameManager.State.CurrentPlayerId && player.score.GetPoints(Score.PointType.Knights) != 0))
            {
                GameManager.State.Players[GameManager.State.CurrentPlayerId].score.AddPoints(Score.PointType.Knights);
            }
            //Destiny: If player used more than 3 knight cards or exactly 3 knight cards and 
            //at least one player used more knight cards then give him points and subtract points from the proper player
            else if (usedKnightCards >= GameManager.CardsManager.RewardedKnightCardNumber && 
                !GameManager.State.Players.Any(player => 
                player.index != GameManager.State.CurrentPlayerId && 
                player.properties.cards.GetUsedKnightCardsNumber() >= usedKnightCards))
            {
                GameManager.State.Players.Where(player => player.score.GetPoints(Score.PointType.Knights) != 0).FirstOrDefault()
                    .score.RemovePoints(Score.PointType.Knights);
                GameManager.State.Players[GameManager.State.CurrentPlayerId].score.AddPoints(Score.PointType.Knights);
            }

            GameManager.State.Players[GameManager.State.CurrentPlayerId].MoveThief(true);
        }

        public void UseRoadBuildCard()
        {
            roadBuildCards--;

            //Destiny: check if player has enough paths to build more
            if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() + 2 <= 
                GameManager.BuildManager.MaxPathNumber)
                GameManager.State.MovingUserMode = MovingMode.TwoPathsForFree;
            else if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() + 1 <=
                GameManager.BuildManager.MaxPathNumber)
                GameManager.State.MovingUserMode = MovingMode.OnePathForFree;
            else
                GameManager.State.MovingUserMode = MovingMode.Normal;
        }

        public void UseInventionCard()
        {
            inventionCards--;

            //Destiny: Show invention popup window
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.INVENTION_POPUP] = true;
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
                GameManager.State.Players[GameManager.State.CurrentPlayerId].resources.AddSpecifiedResource(choosenResources[0]);
                GameManager.State.Players[GameManager.State.CurrentPlayerId].resources.AddSpecifiedResource(choosenResources[1]);
            }
        }

        /// <summary>
        /// Opens the window and decrement number of cards of given type
        /// </summary>
        public void UseMonopolCard()
        {
            monopolCards--;

            //Destiny: Show monopol popup window
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.MONOPOL_POPUP] = true;
        }

        /// <summary>
        /// Method to invoke after choosing resource by player for monopol card
        /// </summary>
        /// <param name="choosedResource">type of chosen resource</param>
        public void MonopolCardEffect(ResourceType choosenResource)
        {
            //Destiny: Giving the current player resources of a given type from other players
            foreach (Player player in GameManager.State.Players)
            {
                int playerResourceNumber = player.resources.GetResourceNumber(choosenResource);
                GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                    .AddSpecifiedResource(choosenResource, playerResourceNumber);
                player.resources.SubtractSpecifiedResource(choosenResource, playerResourceNumber);
            }
        }
    }
}
