using System.Collections.Generic;
using System.Linq;

namespace Player
{
    //Destiny: Keeps all properties of the player
    public class Properties
    {
        private List<int> buildings;
        private List<int> paths;
        private List<int> cards;

        public Properties()
        {
            buildings = new List<int>();
            paths = new List<int>();
            cards = new List<int>();
        }

        public void AddBuilding(int id)
        {
            buildings.Add(id);
        }
        
        public void AddPath(int id)
        {
            paths.Add(id);
        }
        
        public void AddCard(int id)
        {
            cards.Add(id);
        }
    }
}