using Board;
using UnityEngine;

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
        public static BoardElement SelectedElement;
        
        //Destiny: Number of players in the game
        public static int PlayersNumber;

        //Players nicknames
        public static Player.Player[] Players;
        
        //Destiny: Mode chosen for the game
        public static CatanMode Mode;
    }
}