using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Board
{
    //Destiny: Class for holding info about the owner change request
    [Serializable]
    public class OwnerChangeRequest
    {
        public enum ElementType
        {
            Path,
            Junction
        }

        public readonly int ID;
        public readonly Player.Player.Color Color;
        public readonly bool Upgraded;
        public readonly ElementType Type;
        

        public OwnerChangeRequest(int id, Player.Player.Color color, ElementType type, bool upgraded = false)
        {
            ID = id;
            Color = color;
            Upgraded = upgraded;
            Type = type;
        }
    }
    
    //Destiny: Static board manager with requests list
    public static class BoardManager
    {
        //Destiny: Requests list
        public static readonly List<OwnerChangeRequest> OwnerChangeRequest = new();
        
        //Destiny: Board elements info - kind of "interface" to get info for external classes
        public static FieldElement[] Fields;
        public static JunctionElement[] Junctions;
        public static PathElement[] Paths;

        /// <summary>
        /// Removes thief form the field he is currently standing on and places the thief in a new filed
        /// </summary>
        public static void UpdateThief()
        {
            Fields.Where(field => field.IfThief()).FirstOrDefault().SetThief(false);
            (GameManager.Selected.Element as FieldElement).SetThief(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Field id with thief</returns>
        public static int FindThief()
        {
            return Fields.Where(field => field.IfThief()).FirstOrDefault().id;
        }
    }
}