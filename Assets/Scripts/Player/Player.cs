using Assets.Scripts.Player;
using Board;
using DataStorage;
using System.Linq;
using static Player.Cards;

namespace Player
{
    //Destiny: Player class representing one player in game
    public class Player
    {
        public enum Color
        {
            White,
            Yellow,
            Red,
            Blue
        }

        public int index;        
        public readonly Color color;
        public readonly string name;
        public Score score { get; }
        public Properties properties { get; }
        public Resources resources;
        public Ports ports;
        public bool canUseCard;

        public Player(int index, string name)
        {
            this.index = index;
            this.name = name;
            score = new Score();
            properties = new Properties(this);
            resources = new Resources();
            ports = new Ports();
            canUseCard = true;

            color = index switch
            {
                0 => Color.White,
                1 => Color.Yellow,
                2 => Color.Blue,
                3 => Color.Red,
                _ => color
            };
        }

        /// <summary>
        /// Checks if player owns the path.
        /// </summary>
        /// <param name="id">id of the path</param>
        /// <returns>if player is owner of the path</returns>
        public bool OwnsPath(int id)
        {
            return properties.paths.Any(pathID => pathID == id);
        }
        
        /// <summary>
        /// Checks if player owns the building.
        /// </summary>
        /// <param name="id">id of the building</param>
        /// <returns>if player is owner of the building</returns>
        public bool OwnsBuilding(int id)
        {
            return properties.buildings.Any(buildingID => buildingID == id);
        }

        /// <summary>
        /// Player buys a card from the deck
        /// </summary>
        public void BuyCard()
        {
            if (CanBuyCard() && properties.cards.AddCard(GameManager.Deck.First()))
                GameManager.Deck.RemoveAt(0);
        }

        /// <summary>
        /// Checks if player can buy a card
        /// </summary>
        /// <returns>true if player has enough resources</returns>
        public bool CanBuyCard()
        {
            return GameManager.Players[GameManager.CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(GameManager.CardPrice);
        }

        /// <summary>
        /// Uses card
        /// </summary>
        /// <param name="type">type of the card used by player</param>
        public void UseCard(CardType type)
        {
            if (canUseCard)
            {
                switch (type)
                {
                    case CardType.Knight:
                        properties.cards.UseKnightCard();
                        break;
                    case CardType.RoadBuild:
                        properties.cards.UseRoadBuildCard();
                        break;
                    case CardType.Invention:
                        properties.cards.UseInventionCard();
                        break;
                    case CardType.Monopol:
                        properties.cards.UseMonopolCard();
                        break;
                }

                canUseCard = false;
            }
        }

        /// <summary>
        /// Player builds a building in a chosen junction
        /// </summary>
        /// <param name="building">building to build</param>
        public void BuildBuilding(JunctionElement building)
        {
            var initialDistribution = GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching;

            if (initialDistribution || GameManager.CheckIfPlayerCanBuildBuilding(building.id))
                properties.AddBuilding(building.id, building.type == JunctionElement.JunctionType.Village, initialDistribution);
        }

        /// <summary>
        /// Player build a path in a chosen place
        /// </summary>
        /// <param name="path">path to build</param>
        public void BuildPath(PathElement path)
        {
            var initialDistribution = GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching;

            if (initialDistribution || GameManager.CheckIfPlayerCanBuildPath(path.id))
                properties.AddPath(path.id, initialDistribution);
        }

        /// <summary>
        /// Moves related to thief by player
        /// </summary>
        public void MoveThief(bool knightCard = false)
        {
            //Destiny: Unselect any selected element before thief phase
            GameManager.Selected.Element = null;
            
            //Destiny: Only move thief when player used knight card or any player has more resources than 7
            if (knightCard || !GameManager.Players.Any(player => player.resources.GetResourceNumber() > GameManager.MaxResourceNumberWhenTheft))
            {
                GameManager.MovingUserMode = GameManager.MovingMode.MovingThief;
            }
            //Destiny: Player rolled 7 on dices and at least one player has more resources than 7
            else
            {
                GameManager.ThiefPayPopupShown = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>true if given path is adjacent to any player's building</returns>
        public bool CheckIfHasAdjacentBuildingToPath(int pathId)
        {
            if (GameManager.SwitchingGameMode != GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: check if given path is adjacent to building owned by player
                return properties.buildings.Any(playerBuildingId =>
                    BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => adjacentPathId == pathId));
            }
            else
            {
                //Destiny: check if given path is adjacent to building owned by player and is adjacent to just built building
                return properties.buildings.Any(playerBuildingId =>
                    !BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => properties.paths.Contains(adjacentPathId)) &&
                    BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => adjacentPathId == pathId));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if given junction is adjacent to any player's path</returns>
        public bool CheckIfHasAdjacentPathToJunction(int junctionId)
        {
            //Destiny: for each path owned by the player
            foreach (int playerPathId in properties.paths)
            {
                if (BoardManager.Paths[playerPathId].junctionsID.Contains(junctionId))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>true if given path is adjacent to any player's path 
        /// and another player has not the junction between the given path and adjacent path</returns>
        public bool CheckIfHasAdjacentPathToPathWithoutBreak(int pathId)
        {
            return BoardManager.Paths[pathId].pathsID.Any(adjacentPathId =>
                properties.paths.Contains(adjacentPathId) &&
                !BoardManager.Paths[pathId].junctionsID.Any(adjacentJunctionId =>
                    GameManager.Players.Any(player => player.color != color &&
                        BoardManager.Junctions[adjacentJunctionId].pathsID.Any(junctionPathId => player.OwnsPath(junctionPathId) &&
                        BoardManager.Junctions[adjacentJunctionId].pathsID.Contains(adjacentPathId)))));
        }
    }
}