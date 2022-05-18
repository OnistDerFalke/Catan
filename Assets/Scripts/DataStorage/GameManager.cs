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

        public struct SelectedElement
        {
            public JunctionElement SelectedJunction;
            public PathElement SelectedPath;
            public BoardElement.BoardElementType Type;
            public bool IsSelected;
        }

        //Destiny: Element selected by player right now
        public static SelectedElement Selected;
        
        //Destiny: Number of players in the game
        public static int PlayersNumber;

        //Players nicknames
        public static Player.Player[] Players;
        
        //Current player that has a move
        public static int CurrentPlayer;
        
        //Destiny: Mode chosen for the game
        public static CatanMode Mode;

        /// <summary>
        /// Unselects element, no element is now selected
        /// </summary>
        public static void UnselectElement()
        {
            Selected.SelectedJunction = null;
            Selected.SelectedPath = null;
            Selected.IsSelected = false;
        }
        
        /// <summary>
        /// Sets selected element
        /// </summary>
        /// <param name="junction">Junction element (if element is not a junction - leave null)</param>
        /// <param name="path">Path element (if element is not a path - leave null)</param>
        public static void SetSelectedElement(JunctionElement junction = null, PathElement path = null)
        {
            Selected.SelectedJunction = null;
            Selected.SelectedPath = null;
            Selected.IsSelected = false;

            var count = 0;

            if (junction != null)
            {
                Selected.SelectedJunction = junction;
                Selected.Type = BoardElement.BoardElementType.Junction;
                Selected.IsSelected = true;
            }
            else count++;

            if (path != null)
            {
                Selected.SelectedPath = path;
                Selected.Type = BoardElement.BoardElementType.Path;
                Selected.IsSelected = true;
            }
            else count++;
            
            switch (count)
            {
                case < 1:
                    Debug.LogError("No element has been given.");
                    break;
                case > 1:
                    Debug.LogError("More than one element has been given.");
                    break;
            }
        }

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