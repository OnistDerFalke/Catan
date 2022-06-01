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

        private readonly Player player;
        public int knightCards;
        public int roadBuildCards;
        public int inventionCards;
        public int monopolCards;

        public Cards(Player player)
        {
            knightCards = 0;
            roadBuildCards = 0;
            inventionCards = 0;
            monopolCards = 0;
            this.player = player;
        }

        /// <summary>
        /// Add card to players properties
        /// </summary>
        /// <param name="type">type of the card added to player properties</param>
        /// <returns>true if player was able to buy a card</returns>
        public bool AddCard(CardType type)
        {
            if (!player.CanBuyCard())
                return false;

            player.resources.SubtractResources(GameManager.CardPrice);

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
                    player.score.AddPoints(Score.PointType.VictoryPoints);
                    break;
            }

            return true;
        }

        /// <summary>
        /// Use card
        /// </summary>
        /// <param name="type">type of the card used by player</param>
        public void UseCard(CardType type)
        {
            switch (type)
            {
                case CardType.Knight:
                    UseKnightCard();
                    knightCards--;
                    break;
                case CardType.RoadBuild:
                    roadBuildCards--;
                    break;
                case CardType.Invention:
                    inventionCards--;
                    break;
                case CardType.Monopol:
                    monopolCards--;
                    break;
            }
        }

        public void UseKnightCard()
        {

        }

        public void UseRoadBuildCard()
        {

        }

        public void UseInventionCard()
        {

        }

        public void UseMonopolCard()
        {
            // otwarcie okienka z wyborem surowca

            // temporarily assigned value
            ResourceType choosedResource = ResourceType.Wood;
            foreach(Player player in GameManager.Players)
            {
                this.player.resources.AddSpecifiedResource(choosedResource, player.resources.GetResourceNumber(choosedResource));
                player.resources.SubtractSpecifiedResource(choosedResource, player.resources.GetResourceNumber(choosedResource));
            }
        }
    }
}
