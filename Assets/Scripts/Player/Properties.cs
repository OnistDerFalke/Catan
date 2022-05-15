using System.Collections.Generic;
using System.Linq;
using Board;
using DataStorage;

namespace Player
{
    //Destiny: Keeps all properties of the player
    public class Properties
    {
        private readonly Player player;
        private readonly List<int> buildings;
        private readonly List<int> paths;
        private readonly List<int> cards;

        public Properties(Player player)
        {
            buildings = new List<int>();
            paths = new List<int>();
            cards = new List<int>();
            this.player = player;
        }

        /*
            Arguments:
            id: int -> id of the building added to player properties
            upgraded: bool -> true if city is being built, false if just a village
            
            Returns:
            bool -> if operation was successful (building was built) returns true, otherwise false
        */
        public bool AddBuilding(int id, bool upgraded)
        {
            if (!BoardManager.Junctions[id].canBuild && !buildings.Contains(id)) return false;
            
            buildings.Add(id);
            
            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, player.color, OwnerChangeRequest.ElementType.Junction,upgraded));
            
            return true;
        }
        
        /*
            Arguments:
            id: int -> id of the path added to player properties
            
            Returns:
            bool -> if operation was successful (path was built) returns true, otherwise false
        */
        public bool AddPath(int id)
        {
            if (!BoardManager.Paths[id].canBuild && !buildings.Contains(id)) return false;
            
            paths.Add(id);
            
            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, player.color, OwnerChangeRequest.ElementType.Path));

            return true;
        }
        
        //Destiny: Add card to players properties
        public void AddCard(int id)
        {
            cards.Add(id);
        }
    }
}