using Assets.Scripts.Board.States;
using Board;
using DataStorage;
using System;
using System.Collections.Generic;
using static Assets.Scripts.Board.States.JunctionState;
using static Board.BoardElement;
using static Board.States.GameState;
using static Player.Resources;

namespace Assets.Scripts.DataStorage.Managers
{
    public class BuildManager
    {
        //Destiny: Prices of elements
        public Dictionary<ResourceType, int> PathPrice = new();
        public Dictionary<ResourceType, int> VillagePrice = new();
        public Dictionary<ResourceType, int> CityPrice = new();

        //Destiny: Maximum values of player's elements
        public const int MaxPathNumber = 15;
        public const int MaxVillageNumber = 5;
        public const int MaxCityNumber = 4;

        public List<bool> BuildRequests = new();
        public List<bool> ThiefMoveRequests = new();

        public List<Tuple<ElementType, int>> BuildingHistory = new();
        
        public void Setup()
        {
            //Destiny: Setting up price of path
            PathPrice = new Dictionary<ResourceType, int>();
            PathPrice.Add(ResourceType.Wood, 1);
            PathPrice.Add(ResourceType.Clay, 1);

            //Destiny: Setting up price of village
            VillagePrice = new Dictionary<ResourceType, int>();
            VillagePrice.Add(ResourceType.Wood, 1);
            VillagePrice.Add(ResourceType.Clay, 1);
            VillagePrice.Add(ResourceType.Wheat, 1);
            VillagePrice.Add(ResourceType.Wool, 1);

            //Destiny: Setting up price of city
            CityPrice = new Dictionary<ResourceType, int>();
            CityPrice.Add(ResourceType.Wheat, 2);
            CityPrice.Add(ResourceType.Iron, 3);

            BuildingHistory = new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if player can build building in given junction</returns>
        public bool CheckIfPlayerCanBuildBuilding(int junctionId)
        {
            if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].OwnsBuilding(junctionId) &&
                !((JunctionState)BoardManager.Junctions[junctionId].State).canBuild)
            {
                return false;
            }

            //Destiny: check if player has already built a building this round
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst &&
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 1)
            {
                return false;
            }
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond &&
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 2)
            {
                return false;
            }
            if (GameManager.State.SwitchingGameMode != SwitchingMode.GameSwitching &&
                GameManager.State.Players[GameManager.State.CurrentPlayerId].OwnsBuilding(junctionId))
            {
                return false;
            }

            //Destiny: checking conditions during game (when player has at least two buildings)
            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() >= 2)
            {
                //Destiny: checking conditions if player want to build village
                if (((JunctionState)BoardManager.Junctions[junctionId].State).type == JunctionType.None)
                {
                    //Dwstiny: if player has not villages to build then cannot build village
                    if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetVillagesNumber() >= MaxVillageNumber)
                    {
                        return false;
                    }

                    //Destiny: if player has not enough resources to build village then player cannot build village
                    if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                        .CheckIfPlayerHasEnoughResources(VillagePrice))
                    {
                        return false;
                    }

                    //Destiny: if player has not path adjacent to building then player cannot build village
                    if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].CheckIfHasAdjacentPathToJunction(junctionId))
                    {
                        return false;
                    }
                }
                //Destiny: checking conditions if player want to build city replacing owned village
                else if (((JunctionState)BoardManager.Junctions[junctionId].State).type == JunctionType.Village &&
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].OwnsBuilding(junctionId))
                {
                    //Dwstiny: if player has not cities to build then cannot build city
                    if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetCitiesNumber() >= MaxCityNumber)
                    {
                        return false;
                    }

                    //Destiny: if player has not enough resources to build city then player cannot build city
                    if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                        .CheckIfPlayerHasEnoughResources(CityPrice))
                    {
                        return false;
                    }
                }
                else if (((JunctionState)BoardManager.Junctions[junctionId].State).type == JunctionType.City)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionType">type of junction</param>
        /// <returns>Returns true if player can build any building of given type</returns>
        public bool CheckIfPlayerCanBuildAnyBuilding(JunctionType junctionType)
        {
            JunctionType typeToBuild = junctionType == JunctionType.Village ? JunctionType.None : JunctionType.Village;

            foreach(var junction in BoardManager.Junctions)
            {
                if (((JunctionState)junction.State).type == typeToBuild && CheckIfPlayerCanBuildBuilding(junction.State.id))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if player can build path in chosen place</returns>
        public bool CheckIfPlayerCanBuildPath(int pathId)
        {
            if (!((PathState)BoardManager.Paths[pathId].State).canBuild)
            {
                return false;
            }

            //Destiny: checking conditions during first round
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst)
            {
                //Destiny: if player already built a path in first round
                if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 1)
                {
                    return false;
                }
                //Destiny: if path is adjacent to building owned by player
                if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].CheckIfHasAdjacentBuildingToPath(pathId))
                {
                    return false;
                }
            }

            //Destiny: checking conditions during second round
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: if player already built a path in second round
                if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 2)
                {
                    return false;
                }
                //Destiny: if path is adjacent to building just built by player
                if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].CheckIfHasAdjacentBuildingToPath(pathId))
                {
                    return false;
                }
            }

            //Destiny: checking conditions during game (when player has at least two paths)
            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() >= 2)
            {
                //Destiny: if player has not enough paths cannot build path
                if (GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() >= MaxPathNumber)
                {
                    return false;
                }

                //Destiny: check if path is adjacent to player's path and the junction between doesn't belong to another player
                if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].CheckIfHasAdjacentPathToPathWithoutBreak(pathId))
                {
                    return false;
                }

                //Destiny: if player has not enough resources during normal game to build path player cannot build it
                if (!GameManager.State.Players[GameManager.State.CurrentPlayerId].resources
                    .CheckIfPlayerHasEnoughResources(PathPrice) &&
                    GameManager.State.MovingUserMode == MovingMode.Normal)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if player can build any path</returns>
        public bool CheckIfPlayerCanBuildAnyPath()
        {
            foreach (var path in BoardManager.Paths)
            {
                if (CheckIfPlayerCanBuildPath(path.State.id))
                    return true;
            }

            return false;
        }
    }
}
