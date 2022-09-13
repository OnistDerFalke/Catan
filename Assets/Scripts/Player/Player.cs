using Assets.Scripts.Board.States;
using Assets.Scripts.DataStorage.Managers;
using Assets.Scripts.Player;
using Board;
using DataStorage;
using System;
using System.Linq;
using static Assets.Scripts.Board.States.JunctionState;
using static Board.States.GameState;
using static Player.Cards;

namespace Player
{
    [Serializable]
    //Destiny: Player class representing one player in game
    public class Player
    {
        public enum Color
        {
            White,
            Yellow,
            Red,
            Blue,
            Unset
        }

        public int index;
        public Color color;
        public string name;
        public Score score;
        public Properties properties;
        public Resources resources;
        public Ports ports;
        public bool canUseCard;

        public Player(int index, string name, Color color)
        {
            this.index = index;
            this.name = name;
            this.color = color;
            score = new Score();
            properties = new Properties(this);
            resources = new Resources();
            ports = new Ports();
            canUseCard = true;
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
        public CardType BuyCard()
        {
            var card = GameManager.CardsManager.Deck.First();
            if (!CanBuyCard() || !properties.cards.AddCard(card))
            {
                return CardType.None;
            }

            GameManager.CardsManager.Deck.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Checks if player can buy a card
        /// </summary>
        /// <returns>true if player has enough resources</returns>
        public bool CanBuyCard()
        {
            return GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                .CheckIfPlayerHasEnoughResources(GameManager.CardsManager.CardPrice);
        }

        /// <summary>
        /// Uses card
        /// </summary>
        /// <param name="type">type of the card used by player</param>
        public void UseCard(CardType type)
        {
            if (canUseCard)
            {
                properties.cards.UseCard(type);
                canUseCard = false;
            }
        }

        /// <summary>
        /// Player builds a building in a chosen junction
        /// </summary>
        /// <param name="building">building to build</param>
        public void BuildBuilding(JunctionElement building)
        {
            var buildingType = ((JunctionState)building.State).type;

            var initialDistribution = 
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond;

            if (initialDistribution || GameManager.BuildManager.CheckIfPlayerCanBuildBuilding(building.State.id))
            {
                properties.AddBuilding(building.State.id, buildingType == JunctionType.Village, initialDistribution);
            }

            if (initialDistribution)
            {
                GameManager.State.MovingUserMode = MovingMode.BuildPath;
            }

            //Destiny: Check longestPath and update values - building can break the longest path, but only when new building is there
            if (buildingType == JunctionType.None)
            {
                GameManager.LongestPathManager.CheckLongestPath();
            }
        }

        /// <summary>
        /// Player build a path in a chosen place
        /// </summary>
        /// <param name="path">path to build</param>
        public void BuildPath(PathElement path)
        {
            var initialDistribution = 
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond;

            if (initialDistribution || GameManager.BuildManager.CheckIfPlayerCanBuildPath(path.State.id))
            {
                properties.AddPath(path.State.id, initialDistribution);
            }

            if (initialDistribution)
            {
                GameManager.State.MovingUserMode = MovingMode.EndTurn;
            }

            //Destiny: Check longestPath and update values
            GameManager.LongestPathManager.CheckLongestPath();
        }

        /// <summary>
        /// Moves related to thief by player
        /// </summary>
        public void MoveThief(bool knightCard = false)
        {
            //Destiny: Unselect any selected element before thief phase
            GameManager.Selected.Element = null;
            
            //Destiny: Only move thief when player used knight card or any player has more resources than 7
            if (knightCard || !GameManager.State.Players.Any(player => 
                player.resources.GetResourceNumber() > ResourceManager.MaxResourceNumberWhenTheft))
            {
                GameManager.State.MovingUserMode = MovingMode.MovingThief;
            }
            //Destiny: Player rolled 7 on dices and at least one player has more resources than 7
            else
            {
                GameManager.PopupManager.PopupsShown[PopupManager.THIEF_PAY_POPUP] = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>true if given path is adjacent to any player's building</returns>
        public bool CheckIfHasAdjacentBuildingToPath(int pathId)
        {
            if (GameManager.State.SwitchingGameMode != SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: check if given path is adjacent to building owned by player
                return properties.buildings.Any(playerBuildingId =>
                    BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => adjacentPathId == pathId));
            }
            else
            {
                //Destiny: check if given path is adjacent to building owned by player and is adjacent to just built building
                return properties.buildings.Any(playerBuildingId =>
                    !BoardManager.Junctions[playerBuildingId].pathsID.Any(
                        adjacentPathId => properties.paths.Contains(adjacentPathId)) &&
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
                {
                    return true;
                }
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
            //Destiny: check if at least one adjacent path belongs to the player
            if (!BoardManager.Paths[pathId].pathsID.Any(adjacentPathId => properties.paths.Contains(adjacentPathId)))
            {
                return false;
            }

            //Destiny: for each adjacent path to the edge where player want to build his path
            foreach(var adjacentPathId in BoardManager.Paths[pathId].pathsID)
            {
                //Destiny: if adjacent path belongs to the player
                if (properties.paths.Contains(adjacentPathId))
                {
                    //Destiny: for each adjacent junction to the edge where player want to build his path
                    foreach(var adjacentJunctionId in BoardManager.Paths[pathId].junctionsID)
                    {
                        //Destiny: if junction between adjacent path and edge where player want to build his path 
                        //is empty then player can build

                        //Destiny: if another player owns junction adjacent to the edge where player want to build his path
                        if (((JunctionState)BoardManager.Junctions[adjacentJunctionId].State).type != JunctionType.None && 
                            BoardManager.Junctions[adjacentJunctionId].GetOwnerId() != index)
                        {
                            if (BoardManager.Junctions[adjacentJunctionId].pathsID.Contains(pathId) &&
                                BoardManager.Junctions[adjacentJunctionId].pathsID.Contains(adjacentPathId))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}