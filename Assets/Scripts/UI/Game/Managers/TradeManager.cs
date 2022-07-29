using Assets.Scripts.Player;
using System.Collections.Generic;
using static Board.JunctionElement;
using static DataStorage.GameManager;
using static Player.Resources;

namespace Assets.Scripts.UI.Game.Managers
{
    public class TradeManager
    {
        //Destiny: Some things that need to be passed to popups
        public Dictionary<ResourceType, int>[] LandTradeOfferContent;
        public int LandTradeOfferTarget;

        public string RESOURCES_TO_BOUGHT_STRING = "Resources to bought";
        public string ADDITIONAL_RESOURCES = "Additional resources";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proposedResources">resources choosed by the player</param>
        /// <returns>For key RESOURCES_TO_BOUGHT_STRING: number of resources to bought based on proposed resources, 
        /// For key ADDITIONAL_RESOURCES: number of additional resources (above the norm)</returns>
        public Dictionary<string, int> CountTradeResources(Dictionary<ResourceType, int> proposedResources)
        {
            Dictionary<string, int> resourcesTrade = new();
            resourcesTrade.Add(RESOURCES_TO_BOUGHT_STRING, 0);
            resourcesTrade.Add(ADDITIONAL_RESOURCES, 0);

            //Destiny: checking special ports
            var portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Wood);
            Dictionary<string, int> resourceTrade = CountTradeResource(portTypePair, proposedResources[ResourceType.Wood]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Wool);
            resourceTrade = CountTradeResource(portTypePair, proposedResources[ResourceType.Wool]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Wheat);
            resourceTrade = CountTradeResource(portTypePair, proposedResources[ResourceType.Wheat]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Clay);
            resourceTrade = CountTradeResource(portTypePair, proposedResources[ResourceType.Clay]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Iron);
            resourceTrade = CountTradeResource(portTypePair, proposedResources[ResourceType.Iron]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];


            //Destiny: normal or standard exchange
            portTypePair = Players[CurrentPlayer].ports.GetPortKeyPair(PortType.Normal);
            portTypePair = portTypePair.Value ? portTypePair : 
                Players[CurrentPlayer].ports.GetPortKeyPair(PortType.None);
            if (portTypePair.Value)
            {
                int resourcesBoughtNumber = resourcesTrade[ADDITIONAL_RESOURCES] / portTypePair.Key.exchangeForOneResource;
                resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourcesBoughtNumber;
                resourcesTrade[ADDITIONAL_RESOURCES] -= resourcesBoughtNumber * portTypePair.Key.exchangeForOneResource;
            }

            return resourcesTrade;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId1">player id that proposed the exchange</param>
        /// <param name="playerId2">player id that accepted the proposition</param>
        /// <param name="resourcesToDonate">resources of player1 that he wants to give away</param>
        /// <param name="resourcesToTake">resources of player2 that player1 wants to get</param>
        public void ExchangeResourcesTwoPlayers(int playerId1, int playerId2,
            Dictionary<ResourceType, int> resourcesToDonate, Dictionary<ResourceType, int> resourcesToTake)
        {
            Players[playerId1].resources.SubtractResources(resourcesToDonate);
            Players[playerId2].resources.AddResources(resourcesToDonate);
            Players[playerId2].resources.SubtractResources(resourcesToTake);
            Players[playerId1].resources.AddResources(resourcesToTake);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId">player id that proposed the exchange</param>
        /// <param name="resourcesToDonate">resources of player1 that he wants to give away</param>
        /// <param name="resourcesToTake">resources that player1 wants to get</param>
        public void ExchangeResourcesOnePlayer(int playerId, Dictionary<ResourceType, int> resourcesToDonate,
            Dictionary<ResourceType, int> resourcesToTake)
        {
            Players[playerId].resources.SubtractResources(resourcesToDonate);
            Players[playerId].resources.AddResources(resourcesToTake);
        }

        private Dictionary<string, int> CountTradeResource(
            KeyValuePair<PortDetails, bool> portTypePair, int proposedResourceNumber)
        {
            Dictionary<string, int> resourcesTrade = new();
            resourcesTrade.Add(RESOURCES_TO_BOUGHT_STRING, 0);
            resourcesTrade.Add(ADDITIONAL_RESOURCES, 0);

            resourcesTrade[ADDITIONAL_RESOURCES] += proposedResourceNumber;

            if (portTypePair.Value)
            {
                int resourceTraceNumber = proposedResourceNumber / portTypePair.Key.exchangeForOneResource;
                resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTraceNumber;
                resourcesTrade[ADDITIONAL_RESOURCES] -= resourceTraceNumber * portTypePair.Key.exchangeForOneResource;
            }

            return resourcesTrade;
        }
    }
}
