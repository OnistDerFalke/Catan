﻿using System.Collections.Generic;
using Assets.Scripts.Board.States;
using Board;
using DataStorage;
using System.Linq;
using static Assets.Scripts.Board.States.JunctionState;
using static Player.Resources;

namespace Assets.Scripts.DataStorage.Managers
{
    public struct IncomingResourceRequest
    {
        public IncomingResourceRequest(FieldState.FieldType t, int n)
        {
            type = t;
            number = n;
        }

        public FieldState.FieldType type { get; }
        public int number { get; }
    }
    public class ResourceManager
    {
        //Destiny: Maximum values of player's elements
        public const int MaxResourcesNumber = 19;
        public const int MaxResourceNumberWhenTheft = 7;
        
        public List<IncomingResourceRequest> IncomingResourcesRequests = new();

        /// <summary>
        /// Updates resources for each player who has the junction adjacent to the field with thrown number
        /// </summary>
        public void UpdatePlayersResources()
        {
            //Destiny: for each field with thrown number
            foreach (FieldElement field in 
                BoardManager.Fields.Where(f => ((FieldState)f.State).number == GameManager.State.CurrentDiceThrownNumber))
            {
                //Destiny: for each junction adjacent to this field
                field.junctionsID.ForEach(delegate (int fieldJunctionId) 
                {
                    //Destiny: If thief is not over this field
                    if (!((FieldState)field.State).isThief)
                    {
                        field.ParticleAnimation();

                        //Destiny: for each player
                        foreach (var player in GameManager.State.Players)
                        {
                            //Destiny: if player owns adjacent junction then add proper number of resources
                            if (player.OwnsBuilding(fieldJunctionId))
                            {
                                int resourceNumber = ((JunctionState)BoardManager.Junctions[fieldJunctionId].State).type 
                                    == JunctionType.Village ? 1 : 2;

                                if (CheckIfResourceExists(field.GetResourceType(), resourceNumber))
                                {
                                    player.resources.AddSpecifiedFieldResource(field.type, resourceNumber);
                                    string resourceString = resourceNumber == 1 ? "1 sztukę" : "2 sztuki";
                                    string resourceType = GetResourceName(field.GetResourceType());
                                    GameManager.Logs.Add(
                                        $"{player.name} dostaje {resourceString} surowca typu {resourceType}");
                                }
                                else if (CheckIfResourceExists(field.GetResourceType()))
                                {
                                    player.resources.AddSpecifiedFieldResource(field.type, 1);
                                    string resourceType = GetResourceName(field.GetResourceType());
                                    GameManager.Logs.Add(
                                        $"{player.name} dostaje 1 sztukę surowca typu {resourceType}");
                                }
                            }
                        }
                    }
                });
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns>The number of all resources of given type belonging to all players</returns>
        public static int CountPlayersResources(ResourceType resourceType)
        {
            int result = 0;

            foreach (var player in GameManager.State.Players)
            {
                result += player.resources.GetResourceNumber(resourceType);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType">type of resource</param>
        /// <returns>name of resource as string</returns>
        public string GetResourceName(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Wood => "drewno",
                ResourceType.Clay => "glina",
                ResourceType.Wool => "wełna",
                ResourceType.Iron => "ruda żelaza",
                ResourceType.Wheat => "zboże",
                _ => ""
            };
        }
    }
}
