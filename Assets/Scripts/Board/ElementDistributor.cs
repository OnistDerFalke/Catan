using Assets.Scripts.Board.States;
using DataStorage;
using static Board.States.GameState;

namespace Board
{
    public class ElementDistributor
    {
        /// <summary>
        /// Set the initial distribution of elements depending on the mode
        /// </summary>
        public void SetupInitialDistribution()
        {
            if (GameManager.LoadingGame)
                SetupLoadedDistribution();
            else if (GameManager.State.Mode == CatanMode.Basic)
                SetupBasicDistribution();
        }

        private void SetupLoadedDistribution()
        {
            foreach(var player in GameManager.State.Players)
            {
                foreach(var building in player.properties.buildings)
                {
                    var upgraded = 
                        ((JunctionState)BoardManager.Junctions[building].State).type == JunctionState.JunctionType.City;
                    player.properties.AddBuilding(building, upgraded, true);
                }

                foreach (var path in player.properties.paths)
                {
                    player.properties.AddPath(path, true);
                }
            }
        }

        private void SetupBasicDistribution()
        {
            //Destiny: Set initial distribution of elements belonging to red player if there is four players
            if (GameManager.State.Players.Length == 4)
                SetupOnePlayerBuildings(Player.Player.Color.Red, 8, 28, 13, 41);

            SetupOnePlayerBuildings(Player.Player.Color.Yellow, 14, 40, 15, 58);
            SetupOnePlayerBuildings(Player.Player.Color.White, 17, 31, 25, 37);
            SetupOnePlayerBuildings(Player.Player.Color.Blue, 39, 41, 52, 56);
        }
        
        /// <summary>
        /// Set initial distribution of elements belonging to player with given color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="buildingId1"></param>
        /// <param name="buildingId2"></param>
        /// <param name="pathId1"></param>
        /// <param name="pathId2"></param>
        private void SetupOnePlayerBuildings(Player.Player.Color color, int buildingId1, int buildingId2, int pathId1, int pathId2)
        {
            GameManager.State.Players[GameManager.GetPlayerIdByColor(color)].properties.AddBuilding(buildingId1, false, true);
            GameManager.State.Players[GameManager.GetPlayerIdByColor(color)].properties.AddBuilding(buildingId2, false, true);
            GameManager.State.Players[GameManager.GetPlayerIdByColor(color)].properties.AddPath(pathId1, true);
            GameManager.State.Players[GameManager.GetPlayerIdByColor(color)].properties.AddPath(pathId2, true);
        }
    }
}