using System;
using UnityEngine;
using static Player.Resources;
using System.Collections.Generic;

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

        //Destiny: Price of building one path
        public static Dictionary<ResourceType, int> PathPrice = new();
        public static Dictionary<ResourceType, int> VillagePrice = new();
        public static Dictionary<ResourceType, int> CityPrice = new();
        public static Dictionary<ResourceType, int> CardPrice = new();

        //Destiny: Maximum values of player's elements
        public const int MaxPathNumber = 15;
        public const int MaxVillageNumber = 5;
        public const int MaxCityNumber = 4;

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
        public static void Setup(string modeText, int playerNumber)
        {
            Mode = modeText == "PODSTAWOWY" ? CatanMode.Basic : CatanMode.Advanced;
            SwitchingGameMode = Mode == CatanMode.Basic ? SwitchingMode.GameSwitching : SwitchingMode.InitialSwitchingFirst;
            PlayersNumber = playerNumber;
            Players = new Player.Player[PlayersNumber];

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
        }
    }
}