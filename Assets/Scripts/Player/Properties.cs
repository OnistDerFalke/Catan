using System.Collections.Generic;
using Board;

namespace Player
{
    //Destiny: Keeps all properties of the player
    public class Properties
    {
        private readonly Player player;
        public readonly List<int> buildings;
        public readonly List<int> paths;
        private readonly List<int> cards;

        public Properties(Player player)
        {
            buildings = new List<int>();
            paths = new List<int>();
            cards = new List<int>();
            this.player = player;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the building added to player properties</param>
        /// <param name="upgraded">true if city is being built, false if just a village</param>
        /// <returns>True if operation was successful (building was built), otherwise false</returns>
        public bool AddBuilding(int id, bool upgraded)
        {
            if (!BoardManager.Junctions[id].canBuild && !buildings.Contains(id)) return false;
            
            buildings.Add(id);
            
            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, player.color, OwnerChangeRequest.ElementType.Junction,upgraded));
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the path added to player properties</param>
        /// <returns>True if operation was successful (path was built), otherwise false</returns>
        public bool AddPath(int id)
        {
            if (!BoardManager.Paths[id].canBuild && !buildings.Contains(id)) return false;
            
            paths.Add(id);
            
            //Destiny: Send ownership change requests to board manager
            BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(
                id, player.color, OwnerChangeRequest.ElementType.Path));

            return true;
        }

        /// <summary>
        /// Add card to players properties
        /// </summary>
        /// <param name="id">id of the card added to player properties</param>
        public void AddCard(int id)
        {
            cards.Add(id);
        }
    }
}