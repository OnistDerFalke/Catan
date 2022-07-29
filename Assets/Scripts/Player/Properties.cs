using System;
using System.Collections.Generic;
using Board;
using static DataStorage.GameManager;

namespace Player
{
    [Serializable]
    //Destiny: Keeps all properties of the player
    public class Properties
    {
        public readonly Player owner;
        public readonly List<int> buildings;
        public readonly List<int> paths;
        public readonly Cards cards;

        public Properties(Player owner)
        {
            buildings = new List<int>();
            paths = new List<int>();
            cards = new Cards();
            this.owner = owner;
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
            buildings.Add(id);

            //Destiny: Update ports
            owner.ports.UpdatePort(BoardManager.Junctions[id].portType);

            //Destiny: Add point
            owner.score.AddPoints(Score.PointType.Buildings);

            //Destiny: Update resources - add if initial distribution else subtract them
            if (initialDistribution) 
            {
                BoardManager.Junctions[id].fieldsID.ForEach(delegate (int fieldId)
                {
                    owner.resources.AddSpecifiedFieldResource(BoardManager.Fields[fieldId].GetTypeInfo());
                });
            }
            else
            {
                owner.resources.SubtractResources(upgraded ? BuildManager.CityPrice : BuildManager.VillagePrice);
            }

            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, owner.color, OwnerChangeRequest.ElementType.Junction, upgraded));
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the path added to player properties</param>
        /// <returns>True if operation was successful (path was built), otherwise false</returns>
        public bool AddPath(int id, bool initialDistribution = false)
        {
            paths.Add(id);

            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, owner.color, OwnerChangeRequest.ElementType.Path));

            //Destiny: If not initial distribution subtract resources unless the path is for free thanks to used card
            if (!initialDistribution)
            {
                if (MovingUserMode == MovingMode.TwoPathsForFree)
                    MovingUserMode = MovingMode.OnePathForFree;
                else if (MovingUserMode == MovingMode.OnePathForFree && CurrentDiceThrownNumber != 0)
                    MovingUserMode = MovingMode.Normal;
                else if (MovingUserMode == MovingMode.OnePathForFree && CurrentDiceThrownNumber == 0)
                    MovingUserMode = MovingMode.ThrowDice;
                else if (MovingUserMode == MovingMode.Normal)
                    owner.resources.SubtractResources(BuildManager.PathPrice);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of villages owned by the player</returns>
        public int GetVillagesNumber()
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
        public int GetCitiesNumber()
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
        /// <returns>number of paths owned by the player</returns>
        public int GetPathsNumber()
        {
            return paths.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of buildings (cities and villages) owned by the player</returns>
        public int GetBuildingsNumber()
        {
            return buildings.Count;
        }
    }
}