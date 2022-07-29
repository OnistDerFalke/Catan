using Board;
using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Player
{
    [Serializable]
    public class Resources
    {
        //Destiny: Types of resources
        public enum ResourceType
        {
            Wood,
            Clay,
            Wool,
            Iron,
            Wheat,
            None
        }

        private int woodNumber;
        private int clayNumber;
        private int woolNumber;
        private int ironNumber;
        private int wheatNumber;

        public Resources()
        {
            woodNumber = 0;
            clayNumber = 0;
            woolNumber = 0;
            ironNumber = 0;
            wheatNumber = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType">type of resource, if null then return sum of them</param>
        /// <returns>number of resources owned by player</returns>
        public int GetResourceNumber(ResourceType resourceType = ResourceType.None)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    return woodNumber;
                case ResourceType.Clay:
                    return clayNumber;
                case ResourceType.Wool:
                    return woolNumber;
                case ResourceType.Iron:
                    return ironNumber;
                case ResourceType.Wheat:
                    return wheatNumber;
                case ResourceType.None:
                    return woodNumber + clayNumber + woolNumber + ironNumber + wheatNumber;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>dictionary - key: resource type, value: number of resources of given type</returns>
        public Dictionary<ResourceType, int> GetResourcesNumber()
        {
            Dictionary<ResourceType, int> result = new();

            result.Add(ResourceType.Wood, woodNumber);
            result.Add(ResourceType.Clay, clayNumber);
            result.Add(ResourceType.Wheat, wheatNumber);
            result.Add(ResourceType.Wool, woolNumber);
            result.Add(ResourceType.Iron, ironNumber);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Randomly selected resource from among those owned by the player</returns>
        public ResourceType GetRandomResource()
        {
            if (GetResourceNumber() == 0)
                return ResourceType.None;

            List<ResourceType> playerResources = new();
            if (woodNumber > 0)
                playerResources.Add(ResourceType.Wood);
            if (clayNumber > 0)
                playerResources.Add(ResourceType.Clay);
            if (woolNumber > 0)
                playerResources.Add(ResourceType.Wool);
            if (ironNumber > 0)
                playerResources.Add(ResourceType.Iron);
            if (wheatNumber > 0)
                playerResources.Add(ResourceType.Wheat);

            Random random = new Random();
            int resourceNumber = random.Next(playerResources.Count);

            return playerResources[resourceNumber];
        }

        /// <summary>
        /// Adds given number of specified type of field 
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="number"></param>
        public void AddSpecifiedFieldResource(FieldElement.FieldType fieldType, int number = 1)
        {
            switch(fieldType)
            {
                case FieldElement.FieldType.Forest:
                    woodNumber += number;
                    break;
                case FieldElement.FieldType.Hills:
                    clayNumber += number;
                    break;
                case FieldElement.FieldType.Pasture:
                    woolNumber += number;
                    break;
                case FieldElement.FieldType.Mountains:
                    ironNumber += number;
                    break;
                case FieldElement.FieldType.Field:
                    wheatNumber += number;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds given number of specified type of field 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="number"></param>
        public void AddSpecifiedResource(ResourceType resourceType, int number = 1)
        {
            if (GameManager.ResourceManager.CheckIfResourceExists(resourceType, number))
            {
                switch (resourceType)
                {
                    case ResourceType.Wood:
                        woodNumber += number;
                        break;
                    case ResourceType.Clay:
                        clayNumber += number;
                        break;
                    case ResourceType.Wool:
                        woolNumber += number;
                        break;
                    case ResourceType.Iron:
                        ironNumber += number;
                        break;
                    case ResourceType.Wheat:
                        wheatNumber += number;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Adds given resources number to player resources 
        /// </summary>
        /// <param name="resources">Dictionary with resource type as key and number of them as value</param>
        public void AddResources(Dictionary<ResourceType, int> resources)
        {
            if (resources != null && resources.Count() > 0)
            {
                foreach (var resource in resources)
                    AddSpecifiedResource(resource.Key, resource.Value);
            }
        }

        /// <summary>
        /// Subtracts given number of specified type of resource owned by player
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="number"></param>
        public void SubtractSpecifiedResource(ResourceType resourceType, int number = 1)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    woodNumber -= number;
                    break;
                case ResourceType.Clay:
                    clayNumber -= number;
                    break;
                case ResourceType.Wool:
                    woolNumber -= number;
                    break;
                case ResourceType.Iron:
                    ironNumber -= number;
                    break;
                case ResourceType.Wheat:
                    wheatNumber -= number;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Subtracts resources based on given price
        /// </summary>
        /// <param name="price"></param>
        public void SubtractResources(Dictionary<ResourceType, int> price)
        {
            if (CheckIfPlayerHasEnoughResources(price))
            {
                foreach (KeyValuePair<ResourceType, int> p in price)
                {
                    if (GetResourceNumber(p.Key) >= p.Value)
                        SubtractSpecifiedResource(p.Key, p.Value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resources">key: resource type, value: number of resource which player should have</param>
        /// <returns>true if player has enough resource</returns>
        public bool CheckIfPlayerHasEnoughResources(Dictionary<ResourceType, int> resources)
        {
            foreach (KeyValuePair<ResourceType, int> resource in resources)
            {
                if (GetResourceNumber(resource.Key) < resource.Value)
                    return false;
            }

            return true;
        }
    }
}