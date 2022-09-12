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

        public readonly int Id;
        public readonly Player.Player.Color Color;
        public readonly bool Upgraded;
        public readonly ElementType Type;
        

        public OwnerChangeRequest(int id, Player.Player.Color color, ElementType type, bool upgraded = false)
        {
            Id = id;
            Color = color;
            Upgraded = upgraded;
            Type = type;
        }
    }
    
    //Destiny: Static board manager with requests list
    public static class BoardManager
    {
        //Destiny: Number of elements of any type
        public const int FieldsNumber = 19;
        public const int JunctionsNumber = 54;
        public const int PathsNumber = 72;
        public const int PortsNumber = 18;

        //Destiny: Number of levels of any elements
        public const int FieldLevelsNumber = 5;
        public const int JunctionLevelsNumber = 12;
        public const int PathLevelsNumber = 11;

        //Destiny: Requests list
        public static readonly List<OwnerChangeRequest> OwnerChangeRequest = new();
        
        //Destiny: Board elements info - kind of "interface" to get info for external classes
        public static FieldElement[] Fields = new FieldElement[FieldsNumber];
        public static JunctionElement[] Junctions = new JunctionElement[JunctionsNumber];
        public static PathElement[] Paths = new PathElement[PathsNumber];

        //Destiny: Number of fields/junctions/paths above or on the same level
        public static int[] sf;                   // 0, 3, 7, 12, 16, 19
        public static int[] sj;                   // 0, 3, 7, 11, 16, 21, 27, 33, 38, 43, 47, 51, 54
        public static int[] sp;                   // 0, 6, 10, 18, 23, 33, 39, 49, 54, 62, 66, 72

        //Destiny: Number of fields/junctions/paths on level given
        public static int[] f = { 0, 3, 4, 5, 4, 3 };
        public static int[] j = { 0, 3, 4, 4, 5, 5, 6, 6, 5, 5, 4, 4, 3 };
        public static int[] p = { 0, 6, 4, 8, 5, 10, 6, 10, 5, 8, 4, 6 };

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
            return Fields.Where(field => field.IfThief()).FirstOrDefault().State.id;
        }

        public static void Setup()
        {
            //Destiny: fields
            sf = new int[FieldLevelsNumber + 1];
            sf[0] = f[0];
            for (int i = 0; i < FieldLevelsNumber; i++)
            {
                sf[i + 1] = sf[i] + f[i + 1];
            }

            //Destiny: junctions
            sj = new int[JunctionLevelsNumber + 1];
            sj[0] = j[0];
            for (int i = 0; i < JunctionLevelsNumber; i++)
            {
                sj[i + 1] = sj[i] + j[i + 1];
            }

            //Destiny: paths
            sp = new int[PathLevelsNumber + 1];
            sp[0] = p[0];
            for (int i = 0; i < PathLevelsNumber; i++)
            {
                sp[i + 1] = sp[i] + p[i + 1];
            }
        }
    }
}