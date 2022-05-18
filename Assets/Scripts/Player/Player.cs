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
        private Score score;
        public Properties properties;

        public Player(int index, string name)
        {
            this.index = index;
            this.name = name;
            score = new Score();
            properties = new Properties(this);

            color = index switch
            {
                0 => Color.White,
                1 => Color.Yellow,
                2 => Color.Red,
                3 => Color.Blue,
                _ => color
            };
        }
    }
}