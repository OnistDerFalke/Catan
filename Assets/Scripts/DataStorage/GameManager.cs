using System;
using Board;
using UnityEngine;
using Player;


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

        //Destiny: Element selected by player right now
        public static SelectedElement Selected = new();
        
        //Destiny: Number of players in the game
        public static int PlayersNumber;

        //Players nicknames
        public static Player.Player[] Players;
        
        //Current player that has a move
        public static int CurrentPlayer;
        
        //Destiny: Mode chosen for the game
        public static CatanMode Mode;

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
            if (CurrentPlayer == 0) CurrentPlayer = PlayersNumber - 1;
            else CurrentPlayer = (CurrentPlayer - 1);
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
                    if (Players[i].color == color)
                        return i;
                throw new Exception();
            }
            catch(Exception e)
            {
                Debug.LogError("Player with color given could not be found.");
                return -1;
            }
        }
    }
}