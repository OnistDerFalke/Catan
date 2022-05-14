namespace Player
{
    //Destiny: Player class representing one player in game
    public class Player
    {
        private enum Color
        {
            White,
            Yellow,
            Red,
            Blue
        }

        private int index;
        private readonly Color color;
        private string name;
        private Score score;
        private Properties properties;

        public Player(int index, string name)
        {
            this.index = index;
            this.name = name;
            score = new Score();
            properties = new Properties();

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