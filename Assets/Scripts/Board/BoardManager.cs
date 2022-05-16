using DataStorage;
using System.Collections.Generic;

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

        /// <summary>
        /// Set the initial distribution of elements depending on the mode
        /// </summary>
        public static void SetupInitialDistribution()
        {
            if (GameManager.Mode == GameManager.CatanMode.Basic)
                SetupBasicDistribution();
            else if (GameManager.Mode == GameManager.CatanMode.Advanced)
                SetupAdvancedDistribution();
        }

        private static void SetupBasicDistribution()
        {
            //Destiny: Set initial distribution of elements belonging to red player if there is four players
            if (GameManager.PlayersNumber == 4)
            {
                OwnerChangeRequest.Add(new OwnerChangeRequest(8, Player.Player.Color.Red, Board.OwnerChangeRequest.ElementType.Junction));
                OwnerChangeRequest.Add(new OwnerChangeRequest(28, Player.Player.Color.Red, Board.OwnerChangeRequest.ElementType.Junction));
                OwnerChangeRequest.Add(new OwnerChangeRequest(13, Player.Player.Color.Red, Board.OwnerChangeRequest.ElementType.Path));
                OwnerChangeRequest.Add(new OwnerChangeRequest(41, Player.Player.Color.Red, Board.OwnerChangeRequest.ElementType.Path));
            }

            //Destiny: Set initial distribution of elements belonging to yellow player
            OwnerChangeRequest.Add(new OwnerChangeRequest(14, Player.Player.Color.Yellow, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(40, Player.Player.Color.Yellow, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(15, Player.Player.Color.Yellow, Board.OwnerChangeRequest.ElementType.Path));
            OwnerChangeRequest.Add(new OwnerChangeRequest(58, Player.Player.Color.Yellow, Board.OwnerChangeRequest.ElementType.Path));

            //Destiny: Set initial distribution of elements belonging to white player
            OwnerChangeRequest.Add(new OwnerChangeRequest(17, Player.Player.Color.White, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(31, Player.Player.Color.White, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(25, Player.Player.Color.White, Board.OwnerChangeRequest.ElementType.Path));
            OwnerChangeRequest.Add(new OwnerChangeRequest(37, Player.Player.Color.White, Board.OwnerChangeRequest.ElementType.Path));

            //Destiny: Set initial distribution of elements belonging to blue player
            OwnerChangeRequest.Add(new OwnerChangeRequest(39, Player.Player.Color.Blue, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(41, Player.Player.Color.Blue, Board.OwnerChangeRequest.ElementType.Junction));
            OwnerChangeRequest.Add(new OwnerChangeRequest(56, Player.Player.Color.Blue, Board.OwnerChangeRequest.ElementType.Path));
            OwnerChangeRequest.Add(new OwnerChangeRequest(52, Player.Player.Color.Blue, Board.OwnerChangeRequest.ElementType.Path));
        }

        private static void SetupAdvancedDistribution()
        {

        }
    }
}