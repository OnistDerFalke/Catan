using DataStorage;
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

        public Cards()
        {
            knightCards = 0;
            roadBuildCards = 0;
            inventionCards = 0;
            monopolCards = 0;
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
                    break;
                case CardType.RoadBuild:
                    roadBuildCards++;
                    break;
                case CardType.Invention:
                    inventionCards++;
                    break;
                case CardType.Monopol:
                    monopolCards++;
                    break;
                case CardType.VictoryPoint:
                    GameManager.Players[GameManager.CurrentPlayer].score.AddPoints(Score.PointType.VictoryPoints);
                    break;
            }

            return true;
        }

        public void UseKnightCard()
        {
            knightCards--;

        }

        public void UseRoadBuildCard()
        {
            roadBuildCards--;

        }

        public void UseInventionCard()
        {
            inventionCards--;

            // TODO: open the window with resources to choose two pieces of them
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
                GameManager.Players[GameManager.CurrentPlayer].resources
                    .AddSpecifiedResource(choosedResource, player.resources.GetResourceNumber(choosedResource));
                player.resources
                    .SubtractSpecifiedResource(choosedResource, player.resources.GetResourceNumber(choosedResource));
            }
        }
    }
}
