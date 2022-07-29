﻿using Board;
using System.Linq;
using static Board.JunctionElement;
using static DataStorage.GameManager;
using static Player.Resources;

namespace Assets.Scripts.UI.Game.Managers
{
    public class ResourceManager
    {
        //Destiny: Maximum values of player's elements
        public int MaxResourcesNumber = 19;
        public int MaxResourceNumberWhenTheft = 7;

        /// <summary>
        /// Updates resources for each player who has the junction adjacent to the field with thrown number
        /// </summary>
        public void UpdatePlayersResources()
        {
            //Destiny: for each field with thrown number
            foreach (FieldElement field in BoardManager.Fields.Where(f => f.GetNumber() == CurrentDiceThrownNumber))
            {
                //Destiny: for each junction adjacent to this field
                field.junctionsID.ForEach(delegate (int fieldJunctionId) {
                    //Destiny: If thief is not over this field
                    if (!field.IfThief())
                    {
                        //Destiny: for each player
                        foreach (var player in Players)
                        {
                            //Destiny: if player owns adjacent junction then add proper number of resources
                            if (player.OwnsBuilding(fieldJunctionId))
                            {
                                int resourceNumber = BoardManager.Junctions[fieldJunctionId].type == JunctionType.Village ? 1 : 2;
                                if (CheckIfResourceExists(field.GetResourceType(), resourceNumber))
                                    player.resources.AddSpecifiedFieldResource(field.GetTypeInfo(), resourceNumber);
                                else if (CheckIfResourceExists(field.GetResourceType()))
                                    player.resources.AddSpecifiedFieldResource(field.GetTypeInfo(), 1);
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns>The number of all resources of given type belonging to all players</returns>
        public int CountPlayersResources(ResourceType resourceType)
        {
            int result = 0;

            foreach (var player in Players)
            {
                result += player.resources.GetResourceNumber(resourceType);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource">type of resource</param>
        /// <param name="neddedValue">number of resources needed of given type</param>
        /// <returns>true if resource exists in bank (players have less than 19 cards in total)</returns>
        public bool CheckIfResourceExists(ResourceType resource, int neddedValue = 1)
        {
            return CountPlayersResources(resource) + neddedValue <= MaxResourcesNumber;
        }
    }
}
