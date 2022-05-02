using Board;
using UnityEngine;

namespace DataStorage
{
    public static class GameManager
    {
        public enum CatanMode
        {
            Basic,
            Advanced
        }
        
        
        public static BoardElement SelectedElement;
        
        public static int PlayersNumber;
        public static CatanMode Mode;
        public static string[] PlayersNames;

    }
}