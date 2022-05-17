using DataStorage;

namespace Board
{
    public class ElementDistributor
    {
         /// <summary>
        /// Set the initial distribution of elements depending on the mode
        /// </summary>
        public void SetupInitialDistribution()
        {
            if (GameManager.Mode == GameManager.CatanMode.Basic)
                SetupBasicDistribution();
            else if (GameManager.Mode == GameManager.CatanMode.Advanced)
                SetupAdvancedDistribution();
        }

         private void SetupBasicDistribution()
         {
             //Destiny: Set initial distribution of elements belonging to red player if there is four players
             if (GameManager.PlayersNumber == 4)
             {
                 BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(8, Player.Player.Color.Red,
                     OwnerChangeRequest.ElementType.Junction));
                 BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(28, Player.Player.Color.Red,
                     OwnerChangeRequest.ElementType.Junction));
                 BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(13, Player.Player.Color.Red,
                     OwnerChangeRequest.ElementType.Path));
                 BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(41, Player.Player.Color.Red,
                     OwnerChangeRequest.ElementType.Path));
             }

             //Destiny: Set initial distribution of elements belonging to yellow player
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(14, Player.Player.Color.Yellow,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(40, Player.Player.Color.Yellow,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(15, Player.Player.Color.Yellow,
                 OwnerChangeRequest.ElementType.Path));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(58, Player.Player.Color.Yellow,
                 OwnerChangeRequest.ElementType.Path));

             //Destiny: Set initial distribution of elements belonging to white player
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(17, Player.Player.Color.White,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(31, Player.Player.Color.White,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(25, Player.Player.Color.White,
                 OwnerChangeRequest.ElementType.Path));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(37, Player.Player.Color.White,
                 OwnerChangeRequest.ElementType.Path));

             //Destiny: Set initial distribution of elements belonging to blue player
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(39, Player.Player.Color.Blue,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(41, Player.Player.Color.Blue,
                 OwnerChangeRequest.ElementType.Junction));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(56, Player.Player.Color.Blue,
                 OwnerChangeRequest.ElementType.Path));
             BoardManager.OwnerChangeRequest.Add(new OwnerChangeRequest(52, Player.Player.Color.Blue,
                 OwnerChangeRequest.ElementType.Path));
         }
         
        private void SetupAdvancedDistribution()
        {

        }
    }
}