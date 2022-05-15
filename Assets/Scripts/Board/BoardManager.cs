using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    //Destiny: Class for holding info about the owner change request
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
    }
}