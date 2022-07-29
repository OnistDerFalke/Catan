using Board;
using System.Collections.Generic;
using static Board.JunctionElement;
using static DataStorage.GameManager;
using static Player.Resources;

namespace Assets.Scripts.UI.Game.Managers
{
    public class BuildManager
    {
        //Destiny: Prices of elements
        public Dictionary<ResourceType, int> PathPrice = new();
        public Dictionary<ResourceType, int> VillagePrice = new();
        public Dictionary<ResourceType, int> CityPrice = new();

        //Destiny: Maximum values of player's elements
        public int MaxPathNumber = 15;
        public int MaxVillageNumber = 5;
        public int MaxCityNumber = 4;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if player can build building in given junction</returns>
        public bool CheckIfPlayerCanBuildBuilding(int junctionId)
        {
            if (!Players[CurrentPlayer].OwnsBuilding(junctionId) &&
                !BoardManager.Junctions[junctionId].canBuild)
                return false;

            //Destiny: check if player has already built a building this round
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingFirst &&
                Players[CurrentPlayer].properties.GetBuildingsNumber() == 1)
                return false;
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingSecond &&
                Players[CurrentPlayer].properties.GetBuildingsNumber() == 2)
                return false;

            //Destiny: checking conditions during game (when player has at least two buildings)
            if (SwitchingGameMode == SwitchingMode.GameSwitching &&
                Players[CurrentPlayer].properties.GetBuildingsNumber() >= 2)
            {
                //Destiny: checking conditions if player want to build village
                if (BoardManager.Junctions[junctionId].type == JunctionType.None)
                {
                    //Dwstiny: if player has not villages to build then cannot build village
                    if (Players[CurrentPlayer].properties.GetVillagesNumber() >= MaxVillageNumber)
                        return false;

                    //Destiny: if player has not enough resources to build village then player cannot build village
                    if (!Players[CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(VillagePrice))
                        return false;

                    //Destiny: if player has not path adjacent to building then player cannot build village
                    if (!Players[CurrentPlayer].CheckIfHasAdjacentPathToJunction(junctionId))
                        return false;
                }
                //Destiny: checking conditions if player want to build city replacing owned village
                else if (BoardManager.Junctions[junctionId].type == JunctionType.Village &&
                    Players[CurrentPlayer].OwnsBuilding(junctionId))
                {
                    //Dwstiny: if player has not cities to build then cannot build city
                    if (Players[CurrentPlayer].properties.GetCitiesNumber() >= MaxCityNumber)
                        return false;

                    //Destiny: if player has not enough resources to build city then player cannot build city
                    if (!Players[CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(CityPrice))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if player can build path in chosen place</returns>
        public bool CheckIfPlayerCanBuildPath(int pathId)
        {
            if (!BoardManager.Paths[pathId].canBuild)
                return false;

            //Destiny: checking conditions during first round
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingFirst)
            {
                //Destiny: if player already built a path in first round
                if (Players[CurrentPlayer].properties.GetPathsNumber() == 1)
                    return false;
                //Destiny: if path is adjacent to building owned by player
                if (!Players[CurrentPlayer].CheckIfHasAdjacentBuildingToPath(pathId))
                    return false;
            }

            //Destiny: checking conditions during second round
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: if player already built a path in second round
                if (Players[CurrentPlayer].properties.GetPathsNumber() == 2)
                    return false;
                //Destiny: if path is adjacent to building just built by player
                if (!Players[CurrentPlayer].CheckIfHasAdjacentBuildingToPath(pathId))
                    return false;
            }

            //Destiny: checking conditions during game (when player has at least two paths)
            if (SwitchingGameMode == SwitchingMode.GameSwitching &&
                Players[CurrentPlayer].properties.GetPathsNumber() >= 2)
            {
                //Destiny: if player has not enough paths cannot build path
                if (Players[CurrentPlayer].properties.GetPathsNumber() >= MaxPathNumber)
                    return false;

                //Destiny: check if path is adjacent to player's path and the junction between doesn't belong to another player
                if (!Players[CurrentPlayer].CheckIfHasAdjacentPathToPathWithoutBreak(pathId))
                    return false;

                //Destiny: if player has not enough resources during normal game to build path player cannot build it
                if (!Players[CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(PathPrice) &&
                    MovingUserMode == MovingMode.Normal)
                    return false;
            }

            return true;
        }
    }
}
