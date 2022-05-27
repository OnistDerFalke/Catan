using System.Collections.Generic;
using System.Linq;
using Board;
using DataStorage;

namespace Player
{
    //Destiny: Keeps all properties of the player
    public class Properties
    {
        private readonly Player player;
        public readonly List<int> buildings;
        public readonly List<int> paths;
        private readonly List<int> cards;

        public Properties(Player player)
        {
            buildings = new List<int>();
            paths = new List<int>();
            cards = new List<int>();
            this.player = player;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the building added to player properties</param>
        /// <param name="upgraded">true if city is being built, false if just a village</param>
        /// <param name="initialDistribution">true if initial building, default value: false</param>
        /// <returns>True if operation was successful (building was built), otherwise false</returns>
        public bool AddBuilding(int id, bool upgraded, bool initialDistribution = false)
        {
            if (!CheckIfPlayerCanBuildBuilding(id))
                return false;
            
            buildings.Add(id);

            //Destiny: Add point
            GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].score.AddPoints(Score.PointType.Buildings);

            //Destiny: Update resources - add if initial distribution else subtract them
            if (initialDistribution) {
                BoardManager.Junctions[id].fieldsID.ForEach(delegate (int fieldId)
                {
                    GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                        .AddSpecifiedFieldResource(BoardManager.Fields[fieldId].GetTypeInfo());
                });
            }
            else
            {
                //Destiny: player build village
                if (!upgraded)
                    GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                        .SubtractResources(GameManager.VillagePrice);
                //Destiny: player build city
                else
                    GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                        .SubtractResources(GameManager.CityPrice);
            }

            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, player.color, OwnerChangeRequest.ElementType.Junction, upgraded));
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the path added to player properties</param>
        /// <returns>True if operation was successful (path was built), otherwise false</returns>
        public bool AddPath(int id, bool initialDistribution = false)
        {
            if (!CheckIfPlayerCanBuildPath(id))
                return false;
            
            paths.Add(id);
            
            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(id, player.color, OwnerChangeRequest.ElementType.Path));

            //Destiny: If not initial distribution subtract resources
            if (!initialDistribution)
            {
                GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                    .SubtractResources(GameManager.PathPrice);
            }

            return true;
        }

        /// <summary>
        /// Add card to players properties
        /// </summary>
        /// <param name="id">id of the card added to player properties</param>
        public void AddCard(int id)
        {
            cards.Add(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if player can build building in given junction</returns>
        public bool CheckIfPlayerCanBuildBuilding(int junctionId)
        {
            if (!BoardManager.Junctions[junctionId].canBuild)
                return false;

            //Destiny: check if player has already built a building this round
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst && buildings.Count == 1)
                return false;
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond && buildings.Count == 2)
                return false;

            //Destiny: checking conditions during game (when player has at least two buildings)
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && buildings.Count >= 2)
            {
                //Destiny: checking conditions if player want to build village
                if (BoardManager.Junctions[junctionId].type == JunctionElement.JunctionType.None) 
                {
                    //Dwstiny: if player has not villages to build then cannot build village
                    if (GetVillageNumber() >= GameManager.MaxVillageNumber) 
                        return false;

                    //Destiny: if player has not enough resources to build village then player cannot build village
                    if (!GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                        .CheckIfPlayerHasEnoughResources(GameManager.VillagePrice))
                        return false;

                    //Destiny: if player has not path adjacent to building then player cannot build village
                    if (!CheckIfPlayerHasAdjacentPathToJunction(junctionId))
                        return false;
                }
                //Destiny: checking conditions if player want to build city replacing owned village
                else if (BoardManager.Junctions[junctionId].type == JunctionElement.JunctionType.Village &&
                    GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].OwnsBuilding(junctionId))
                {
                    //Dwstiny: if player has not cities to build then cannot build city
                    if (GetCityNumber() >= GameManager.MaxCityNumber)
                        return false;

                    //Destiny: if player has not enough resources to build city then player cannot build city
                    if (!GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                        .CheckIfPlayerHasEnoughResources(GameManager.CityPrice))
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
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst)
            {
                //Destiny: if player already built a path in first round
                if (paths.Count == 1)
                    return false;
                //Destiny: if path is adjacent to building owned by player
                if (!CheckIfPlayerHasAdjacentBuildingToPath(pathId))
                    return false;
            }

            //Destiny: checking conditions during second round
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: if player already built a path in second round
                if (paths.Count == 2)
                    return false;
                //Destiny: if path is adjacent to building just built by player
                if (!CheckIfPlayerHasAdjacentBuildingToPath(pathId))
                    return false;
            }


            //Destiny: checking conditions during game (when player has at least two paths)
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && paths.Count >= 2)
            {
                //Destiny: if player has not enough paths cannot build path
                if (paths.Count >= GameManager.MaxPathNumber)
                    return false;

                //Destiny: if player has not enough resources during game to build path player cannot build it
                if (!GameManager.Players[GameManager.GetPlayerIdByColor(player.color)].resources
                    .CheckIfPlayerHasEnoughResources(GameManager.PathPrice))
                    return false;

                //Destiny: check if path is adjacent to player's path and the junction between doesn't belong to another player
                if (!CheckIfPlayerHasAdjacentPathToPathWithoutBreak(pathId))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of villages owned by the player</returns>
        public int GetVillageNumber()
        {
            var villageNumber = 0;

            buildings.ForEach(delegate (int buildingId)
            {
                if (BoardManager.Junctions[buildingId].type == JunctionElement.JunctionType.Village)
                    villageNumber++;
            });

            return villageNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of cities owned by the player</returns>
        public int GetCityNumber()
        {
            var cityNumber = 0;

            buildings.ForEach(delegate (int buildingId)
            {
                if (BoardManager.Junctions[buildingId].type == JunctionElement.JunctionType.City)
                    cityNumber++;
            });

            return cityNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>true if given path is adjacent to any player's path 
        /// and another player has not the junction between the given path and adjacent path</returns>
        public bool CheckIfPlayerHasAdjacentPathToPathWithoutBreak(int pathId)
        {
            return BoardManager.Paths[pathId].pathsID.Any(adjacentPathId => 
                paths.Contains(adjacentPathId) &&
                !BoardManager.Paths[pathId].junctionsID.Any(adjacentJunctionId => 
                    GameManager.Players.Any(player => player.color != this.player.color && 
                        BoardManager.Junctions[adjacentJunctionId].pathsID.Any(junctionPathId => player.OwnsPath(junctionPathId) &&
                        BoardManager.Junctions[adjacentJunctionId].pathsID.Contains(adjacentPathId)))));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>true if given path is adjacent to any player's building</returns>
        public bool CheckIfPlayerHasAdjacentBuildingToPath(int pathId)
        {
            if (GameManager.SwitchingGameMode != GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                //Destiny: check if given path is adjacent to building owned by player
                return buildings.Any(playerBuildingId =>
                    BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => adjacentPathId == pathId));
            }
            else
            {
                //Destiny: check if given path is adjacent to building owned by player and is adjacent to just built building
                return buildings.Any(playerBuildingId =>
                    !BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => paths.Contains(adjacentPathId)) &&
                    BoardManager.Junctions[playerBuildingId].pathsID.Any(adjacentPathId => adjacentPathId == pathId));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if given junction is adjacent to any player's path</returns>
        public bool CheckIfPlayerHasAdjacentPathToJunction(int junctionId)
        {
            //Destiny: for each path owned by the player
            foreach (int playerPathId in paths)
            {
                if (BoardManager.Paths[playerPathId].junctionsID.Contains(junctionId))
                    return true;
            }

            return false;
        }
    }
}