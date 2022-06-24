using System;
using System.Collections.Generic;
using System.Linq;
using Board;
using UnityEngine;
using static Player.Cards;
using static Player.Resources;

namespace DataStorage
{
    //Destiny: Storage for all important information about the game
    public static class GameManager
    {
        public enum CatanMode
        {
            Basic,
            Advanced
        }

        public enum SwitchingMode
        {
            InitialSwitchingFirst,
            InitialSwitchingSecond,
            GameSwitching
        }

        public enum MovingMode
        {
            Normal,
            OnePathForFree,
            TwoPathsForFree,
            MovingThief
        }

        //Destiny: Popups flow control (if popup is shown or not)
        public static bool MonopolPopupShown;
        public static bool InventionPopupShown;
        
        //Destiny: Element selected by player right now
        public static SelectedElement Selected = new();
        
        //Destiny: Number of players in the game
        public static int PlayersNumber;

        //Players nicknames
        public static Player.Player[] Players;
        
        //Current player that has a move
        public static int CurrentPlayer;
        
        //Current thrown dice number
        public static int CurrentDiceThrownNumber;
        
        //Destiny: Mode chosen for the game
        public static CatanMode Mode;

        //Destiny: User order changing mode 
        public static SwitchingMode SwitchingGameMode;

        //Destiny: User moving mode depending on card used
        public static MovingMode MovingUserMode;

        //Destiny: Price of building one path
        public static Dictionary<ResourceType, int> PathPrice = new();
        public static Dictionary<ResourceType, int> VillagePrice = new();
        public static Dictionary<ResourceType, int> CityPrice = new();
        public static Dictionary<ResourceType, int> CardPrice = new();

        //Destiny: Maximum values of player's elements
        public const int MaxPathNumber = 15;
        public const int MaxVillageNumber = 5;
        public const int MaxCityNumber = 4;
        public const int MaxResourcesNumber = 19;
        public const int MaxResourceNumberWhenTheft = 7;

        //Destiny: Deck (pile of cards)
        public static List<CardType> Deck = new();

        /// <summary>
        /// Switches current player to the next player
        /// </summary>
        public static void SwitchToNextPlayer()
        {
            CurrentPlayer = (CurrentPlayer + 1) % PlayersNumber;
        }

        /// <summary>
        /// Switches current player to the previous player
        /// </summary>
        public static void SwitchToPreviousPlayer()
        {
            if (CurrentPlayer == 0) 
                CurrentPlayer = PlayersNumber - 1;
            else 
                CurrentPlayer = (CurrentPlayer - 1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">Player color</param>
        /// <returns>Player index</returns>
        /// <exception cref="Exception">Player with color given could not be found</exception>
        public static int GetPlayerIdByColor(Player.Player.Color color)
        {
            try
            {
                for (var i = 0; i < PlayersNumber; i++)
                {
                    if (Players[i].color == color)
                        return i;
                }
                throw new Exception();
            }
            catch(Exception e)
            {
                Debug.LogError("Player with color given could not be found.");
                return -1;
            }
        }

        /// <summary>
        /// Setting up game manager basic information based on popups inputs
        /// </summary>
        /// <param name="modeText">game mode choosed by players</param>
        /// <param name="playerNumber">game mode choosed by players</param>
        public static void Setup(string modeText)
        {
            Mode = modeText == "PODSTAWOWY" ? CatanMode.Basic : CatanMode.Advanced;
            SwitchingGameMode = Mode == CatanMode.Basic ? SwitchingMode.GameSwitching : SwitchingMode.InitialSwitchingFirst;
            MovingUserMode = MovingMode.Normal;

            //Destiny: Setting up price of path
            PathPrice.Add(ResourceType.Wood, 1);
            PathPrice.Add(ResourceType.Clay, 1);

            //Destiny: Setting up price of village
            VillagePrice.Add(ResourceType.Wood, 1);
            VillagePrice.Add(ResourceType.Clay, 1);
            VillagePrice.Add(ResourceType.Wheat, 1);
            VillagePrice.Add(ResourceType.Wool, 1);

            //Destiny: Setting up price of city
            CityPrice.Add(ResourceType.Wheat, 2);
            CityPrice.Add(ResourceType.Iron, 3);

            //Destiny: Setting up price of card
            CardPrice.Add(ResourceType.Wheat, 1);
            CardPrice.Add(ResourceType.Wool, 1);
            CardPrice.Add(ResourceType.Iron, 1);

            //Destiny: Create Deck
            Shuffle();
        }

        /// <summary>
        /// Updates resources for each player who has the junction adjacent to the field with thrown number
        /// </summary>
        public static void UpdatePlayersResources()
        {
            if (CurrentDiceThrownNumber != 7)
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
                            foreach (Player.Player player in Players)
                            {
                                //Destiny: if player owns adjacent junction then add proper number of resources
                                if (player.OwnsBuilding(fieldJunctionId))
                                {
                                    int resourceNumber = BoardManager.Junctions[fieldJunctionId].type == JunctionElement.JunctionType.Village ? 1 : 2;
                                    if (ResourceExists(field.GetResourceType(), resourceNumber))
                                        player.resources.AddSpecifiedFieldResource(field.GetTypeInfo(), resourceNumber);
                                    else if (ResourceExists(field.GetResourceType()))
                                        player.resources.AddSpecifiedFieldResource(field.GetTypeInfo(), 1);
                                }
                            }
                        }
                    });
                }
            } 
            //Destiny: move the thief
            else
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns>The number of all resources of given type belonging to all players</returns>
        public static int CountPlayersResources(ResourceType resourceType)
        {
            int result = 0;

            foreach(Player.Player player in Players)
            {
                result += player.resources.GetResourceNumber(resourceType);
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="junctionId"></param>
        /// <returns>true if player can build building in given junction</returns>
        public static bool CheckIfPlayerCanBuildBuilding(int junctionId)
        {
            if (!Players[CurrentPlayer].OwnsBuilding(junctionId) && !BoardManager.Junctions[junctionId].canBuild)
                return false;

            //Destiny: check if player has already built a building this round
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingFirst && Players[CurrentPlayer].properties.GetBuildingsNumber() == 1)
                return false;
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingSecond && Players[CurrentPlayer].properties.GetBuildingsNumber() == 2)
                return false;

            //Destiny: checking conditions during game (when player has at least two buildings)
            if (SwitchingGameMode == SwitchingMode.GameSwitching && Players[CurrentPlayer].properties.GetBuildingsNumber() >= 2)
            {
                //Destiny: checking conditions if player want to build village
                if (BoardManager.Junctions[junctionId].type == JunctionElement.JunctionType.None)
                {
                    //Dwstiny: if player has not villages to build then cannot build village
                    if (Players[CurrentPlayer].properties.GetVillagesNumber() >= MaxVillageNumber)
                        return false;

                    //Destiny: if player has not enough resources to build village then player cannot build village
                    if (Players[CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(VillagePrice))
                        return false;

                    //Destiny: if player has not path adjacent to building then player cannot build village
                    if (!Players[CurrentPlayer].CheckIfHasAdjacentPathToJunction(junctionId))
                        return false;
                }
                //Destiny: checking conditions if player want to build city replacing owned village
                else if (BoardManager.Junctions[junctionId].type == JunctionElement.JunctionType.Village && 
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
        public static bool CheckIfPlayerCanBuildPath(int pathId)
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
            if (SwitchingGameMode == SwitchingMode.GameSwitching && Players[CurrentPlayer].properties.GetPathsNumber() >= 2)
            {
                //Destiny: if player has not enough paths cannot build path
                if (Players[CurrentPlayer].properties.GetPathsNumber() >= MaxPathNumber)
                    return false;

                //Destiny: check if path is adjacent to player's path and the junction between doesn't belong to another player
                if (!Players[CurrentPlayer].CheckIfHasAdjacentPathToPathWithoutBreak(pathId))
                    return false;

                //Destiny: if player has not enough resources during normal game to build path player cannot build it
                if (!Players[CurrentPlayer].resources.CheckIfPlayerHasEnoughResources(PathPrice) && MovingUserMode == MovingMode.Normal)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource">type of resource</param>
        /// <param name="neddedValue">number of resources needed of given type</param>
        /// <returns>true if resource exists in bank (players have less than 19 cards in total)</returns>
        public static bool ResourceExists(ResourceType resource, int neddedValue = 1)
        {
            return CountPlayersResources(resource) + neddedValue <= MaxResourcesNumber;
        }

        /// <summary>
        /// Creates deck and shuffles the cards
        /// </summary>
        private static void Shuffle()
        {
            Dictionary<CardType, int> CardsNumber = new();
            CardsNumber.Add(CardType.Knight, 14);
            CardsNumber.Add(CardType.VictoryPoint, 5);
            CardsNumber.Add(CardType.RoadBuild, 2);
            CardsNumber.Add(CardType.Monopol, 2);
            CardsNumber.Add(CardType.Invention, 2);
            List<CardType> orderedDeck = new();

            //Destiny: Create deck with proper number of cards
            foreach(KeyValuePair<CardType, int> cardNumber in CardsNumber)
            {
                for (int i = 0; i < cardNumber.Value; i++)
                    orderedDeck.Add(cardNumber.Key);
            }

            //Destiny: Shuffle the cards
            Deck = orderedDeck.OrderBy(x => Guid.NewGuid().ToString()).ToList();
        }
    }
}