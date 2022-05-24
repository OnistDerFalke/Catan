using System.Linq;

namespace Player
{
    //Destiny: Player class representing one player in game
    public class Player
    {
        public enum Color
        {
            White,
            Yellow,
            Red,
            Blue
        }

        private int index;
        public readonly Color color;
        public readonly string name;
        public Score score { get; }
        public Properties properties { get; }
        public Resources resources;

        public Player(int index, string name)
        {
            this.index = index;
            this.name = name;
            score = new Score();
            properties = new Properties(this);
            resources = new Resources();

            color = index switch
            {
                0 => Color.White,
                1 => Color.Yellow,
                2 => Color.Red,
                3 => Color.Blue,
                _ => color
            };
        }

        /// <summary>
        /// Checks if player owns the path.
        /// </summary>
        /// <param name="id">id of the path</param>
        /// <returns>if player is owner of the path</returns>
        public bool OwnsPath(int id)
        {
            return properties.paths.Any(pathID => pathID == id);
        }
        
        /// <summary>
        /// Checks if player owns the building.
        /// </summary>
        /// <param name="id">id of the building</param>
        /// <returns>if player is owner of the building</returns>
        public bool OwnsBuilding(int id)
        {
            return properties.buildings.Any(buildingID => buildingID == id);
        }
    }
}