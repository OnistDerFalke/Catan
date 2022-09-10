using DataStorage;
using System.Collections.Generic;
using static Board.JunctionElement;
using static Player.Resources;

namespace Assets.Scripts.DataStorage.Managers
{
    public class TradeManager
    {
        //Destiny: Some things that need to be passed to popups
        public Dictionary<ResourceType, int>[] LandTradeOfferContent;
        public int LandTradeOfferTarget;

        public const string RESOURCES_TO_BOUGHT_STRING = "Resources to bought";
        public const string ADDITIONAL_RESOURCES = "Additional resources";

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
            Dictionary<string, int> resourceTrade = CountTradeResource(PortType.Wood, proposedResources[ResourceType.Wood]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            resourceTrade = CountTradeResource(PortType.Wool, proposedResources[ResourceType.Wool]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            resourceTrade = CountTradeResource(PortType.Wheat, proposedResources[ResourceType.Wheat]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            resourceTrade = CountTradeResource(PortType.Clay, proposedResources[ResourceType.Clay]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            resourceTrade = CountTradeResource(PortType.Iron, proposedResources[ResourceType.Iron]);
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTrade[RESOURCES_TO_BOUGHT_STRING];
            resourcesTrade[ADDITIONAL_RESOURCES] += resourceTrade[ADDITIONAL_RESOURCES];

            //Destiny: normal or standard exchange
            PortType basicTradePortType = GameManager.State.Players[GameManager.State.CurrentPlayerId].ports
                .GetPortKeyPair(PortType.Normal) ? PortType.Normal : PortType.None;
            int exchangeForOneResourceValue = GameManager.PortManager.portsInfo[basicTradePortType];

            int resourcesBoughtNumber = resourcesTrade[ADDITIONAL_RESOURCES] / exchangeForOneResourceValue;
            resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourcesBoughtNumber;
            resourcesTrade[ADDITIONAL_RESOURCES] -= resourcesBoughtNumber * exchangeForOneResourceValue;

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
            GameManager.State.Players[playerId1].resources.SubtractResources(resourcesToDonate);
            GameManager.State.Players[playerId2].resources.AddResources(resourcesToDonate);
            GameManager.State.Players[playerId2].resources.SubtractResources(resourcesToTake);
            GameManager.State.Players[playerId1].resources.AddResources(resourcesToTake);
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
            GameManager.State.Players[playerId].resources.SubtractResources(resourcesToDonate);
            GameManager.State.Players[playerId].resources.AddResources(resourcesToTake);
        }

        private Dictionary<string, int> CountTradeResource(PortType portType, int proposedResourceNumber)
        {
            bool playerHasPort = GameManager.State.Players[GameManager.State.CurrentPlayerId].ports.GetPortKeyPair(portType);
            int exchangeForOneResourceValue = GameManager.PortManager.portsInfo[portType];

            Dictionary<string, int> resourcesTrade = new();
            resourcesTrade.Add(RESOURCES_TO_BOUGHT_STRING, 0);
            resourcesTrade.Add(ADDITIONAL_RESOURCES, 0);

            resourcesTrade[ADDITIONAL_RESOURCES] += proposedResourceNumber;

            if (playerHasPort)
            {
                int resourceTraceNumber = proposedResourceNumber / exchangeForOneResourceValue;
                resourcesTrade[RESOURCES_TO_BOUGHT_STRING] += resourceTraceNumber;
                resourcesTrade[ADDITIONAL_RESOURCES] -= resourceTraceNumber * exchangeForOneResourceValue;
            }

            return resourcesTrade;
        }
    }
}
