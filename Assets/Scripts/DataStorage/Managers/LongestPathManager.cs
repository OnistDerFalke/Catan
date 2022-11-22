using System.Collections.Generic;
using System.Linq;
using Board;
using DataStorage;
using Player;

namespace Assets.Scripts.DataStorage.Managers
{
    public class LongestPathManager
    {
        //Destiny: Minimum values to get reward
        public const int RewardedLongestPathLength = 5;

        /// <summary>
        /// Updates points for longest path
        /// </summary>
        /// <param name="playersWithLongestPathToCompare">Dictionary with player (who has the longest path) id as key and path length as value. 
        /// You can use it to compare if point state before deleting object is the same as after this action. Default value is null.</param>
        public void CheckLongestPath(Dictionary<int, int> playersWithLongestPathToCompare = null)
        {
            //Destiny: Get all players with the longest path
            Dictionary<int, int> longestPathPlayerIds = FindPlayerIdsWithLongestPath();
            int playerIdWithAwardedLongestPath = GetPlayerIdWithAwardedLongestPath();

            //Destiny: if checking longest path is after removing element and if current player wasn't awarded at the begging of his move
            if ((playersWithLongestPathToCompare != null) && (GameManager.State.CurrentPlayerId != GameManager.State.PlayerIdWithAwardedPathAtBegining))
            {
                //Destiny: if current player after deleting element has the longest path
                // and before removing element he had the longest path too
                // and now the player that had reward at the begging of move of current player has the longest path
                if (longestPathPlayerIds.ContainsKey(GameManager.State.CurrentPlayerId) &&
                    playersWithLongestPathToCompare.ContainsKey(GameManager.State.CurrentPlayerId) &&
                    longestPathPlayerIds.ContainsKey(GameManager.State.PlayerIdWithAwardedPathAtBegining))
                {
                    //Destiny: current player gives additional points back to the right player
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].score.RemovePoints(Score.PointType.LongestPath);
                    GameManager.Logs.Add($"Gracz {GameManager.State.Players[GameManager.State.CurrentPlayerId].name} cofnął ruch " +
                            ", przez co traci uzyskane 2 punkty w ramach nagrody za najdłuższą drogę");

                    if (GameManager.State.PlayerIdWithAwardedPathAtBegining >= 0 &&
                        GameManager.State.PlayerIdWithAwardedPathAtBegining < GameManager.State.Players.Length)
                    {
                        GameManager.State.Players[GameManager.State.PlayerIdWithAwardedPathAtBegining].score.AddPoints(Score.PointType.LongestPath);
                        GameManager.Logs.Add($"Gracz {GameManager.State.Players[GameManager.State.PlayerIdWithAwardedPathAtBegining].name} " +
                                "dostaje utracone 2 punkty w ramach nagrody za najdłuższą drogę");
                    }

                    return;
                }
                //Destiny: if before removing element current player had the longest path
                // and now he has not the longest path
                else if (playersWithLongestPathToCompare.ContainsKey(GameManager.State.CurrentPlayerId) &&
                    !longestPathPlayerIds.ContainsKey(GameManager.State.CurrentPlayerId))
                {
                    //Destiny: give back points to proper player
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].score.RemovePoints(Score.PointType.LongestPath);
                    GameManager.Logs.Add($"Gracz {GameManager.State.Players[GameManager.State.CurrentPlayerId].name} cofnął ruch " +
                            ", przez co traci uzyskane 2 punkty w ramach nagrody za najdłuższą drogę");

                    if (GameManager.State.PlayerIdWithAwardedPathAtBegining >=0 && 
                        GameManager.State.PlayerIdWithAwardedPathAtBegining < GameManager.State.Players.Length)
                    {
                        GameManager.State.Players[GameManager.State.PlayerIdWithAwardedPathAtBegining].score.AddPoints(Score.PointType.LongestPath);
                        GameManager.Logs.Add($"Gracz {GameManager.State.Players[GameManager.State.PlayerIdWithAwardedPathAtBegining].name} " +
                                "dostaje utracone 2 punkty w ramach nagrody za najdłuższą drogę");
                    }

                    return;
                }
            }

            //Destiny: if actual longest path should be rewarded - length is at least 5
            if (longestPathPlayerIds.Values.First() >= RewardedLongestPathLength)
            {
                //Destiny: if one player already has reward
                if (playerIdWithAwardedLongestPath < GameManager.State.Players.Count())
                {
                    //Destiny: if player with awarded longest path still has longest path then end the function
                    if (longestPathPlayerIds.Keys.Contains(playerIdWithAwardedLongestPath))
                    {
                        return;
                    }

                    //Destiny: else clear his points
                    GameManager.State.Players[playerIdWithAwardedLongestPath].score.RemovePoints(Score.PointType.LongestPath);
                    
                    //Destiny: give points to proper player if he's the only player who has the longest path
                    if (longestPathPlayerIds.Count() == 1)
                    {
                        GameManager.State.Players[longestPathPlayerIds.Keys.First()].score.AddPoints(Score.PointType.LongestPath);
                        GameManager.Logs.Add($"{GameManager.State.Players[longestPathPlayerIds.Keys.First()].name} " +
                            $"wybudował najdłuższą drogę i zabiera 2 punkty graczowi " +
                            $"{GameManager.State.Players[playerIdWithAwardedLongestPath].name}");
                    }
                    else
                    {
                        GameManager.Logs.Add($"{GameManager.State.Players[playerIdWithAwardedLongestPath].name} " +
                            "traci 2 punkty, ponieważ jego droga nie jest już najdłuższa");
                    }
                }
                //Destiny: if no one has reward and now is one player with the longest path
                else if (longestPathPlayerIds.Count() == 1)
                {
                    GameManager.State.Players[longestPathPlayerIds.Keys.First()].score.AddPoints(Score.PointType.LongestPath);
                    GameManager.Logs.Add($"{GameManager.State.Players[longestPathPlayerIds.Keys.First()].name} " +
                        "zdobywa 2 punkty za wybudowanie najdłuższej drogi");
                }
            }
            //Destiny: if actual longest path shouldn't be rewarded - length is below 5 than remove points from the proper player
            else if (playerIdWithAwardedLongestPath < GameManager.State.Players.Count())
            {
                GameManager.State.Players[playerIdWithAwardedLongestPath].score.RemovePoints(Score.PointType.LongestPath);
                GameManager.Logs.Add($"Najdłuższa droga gracza {GameManager.State.Players[playerIdWithAwardedLongestPath].name} " +
                        "została przerwana, przez co traci 2 punkty");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Id of a player who now has points for the longest path</returns>
        public int GetPlayerIdWithAwardedLongestPath()
        {
            foreach (var player in GameManager.State.Players)
            {
                if (player.score.GetPoints(Score.PointType.LongestPath) != 0)
                {
                    return player.index;
                }
            }

            return GameManager.State.Players.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Dictionary of players ids who have the longest path and the values of their longest path</returns>
        public Dictionary<int, int> FindPlayerIdsWithLongestPath()
        {
            Dictionary<int, int> playersLongestPath = new();
            List<int> longestPathIds;
            List<int> junctionIds; ;
            int longestPathLength;
            int tempLongestPathLength;

            //Destiny: for each player count the length of the longest path
            foreach (var player in GameManager.State.Players)
            {
                longestPathLength = 0;
                tempLongestPathLength = 0;

                foreach (int edgePathId in player.properties.paths)
                {
                    longestPathIds = new();
                    junctionIds = new();
                    longestPathIds.Add(edgePathId);

                    tempLongestPathLength = FindLongestPath(player.index, longestPathIds, junctionIds);
                    if (longestPathLength == 0 || longestPathLength < tempLongestPathLength)
                    {
                        longestPathLength = tempLongestPathLength;
                    }
                }

                playersLongestPath[player.index] = longestPathLength;
            }

            //Destiny: Find player with the longest path
            Dictionary<int, int> result = new();
            longestPathLength = playersLongestPath.Values.Max();
            foreach (var playerLongestPath in playersLongestPath)
            {
                if (playerLongestPath.Value == longestPathLength)
                {
                    result.Add(playerLongestPath.Key, playerLongestPath.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Recursive function that finds the longest path and returns its length
        /// </summary>
        /// <param name="playerId">Id of the player for whom the function searches for his longest path</param>
        /// <param name="pathIds">The way so far consisting of path ids</param>
        /// <param name="junctionIds">The way so far consisting of junction ids</param>
        /// <returns>Length of the longest path belonging to given player</returns>
        private int FindLongestPath(int playerId, List<int> pathIds, List<int> junctionIds)
        {
            int longestPathLength = pathIds.Count();

            //Destiny: for each adjacent path to the last path in the longest path so far
            foreach (int adjacentPath in BoardManager.Paths[pathIds.Last()].pathsID)
            {
                int commonJunctionId = BoardManager.Paths[pathIds.Last()].FindCommonJunction(adjacentPath);

                //Destiny: if we haven't yet get through this junction and the junction is neutral or
                //belongs to given player then keep counting
                if (!junctionIds.Contains(commonJunctionId) &&
                    (BoardManager.Junctions[commonJunctionId].GetOwnerId() == GameManager.State.Players.Count() ||
                    BoardManager.Junctions[commonJunctionId].GetOwnerId() == playerId))
                {
                    junctionIds.Add(commonJunctionId);

                    //Destiny: if adjacent path belongs to the player and it is not in current path already then add it
                    if (BoardManager.Paths[adjacentPath].GetOwnerId() == playerId && !pathIds.Contains(adjacentPath))
                    {
                        pathIds.Add(adjacentPath);

                        int tempLongestPathLength = FindLongestPath(playerId, pathIds, junctionIds);
                        if (longestPathLength == 0 || longestPathLength < tempLongestPathLength)
                        {
                            longestPathLength = tempLongestPathLength;
                        }

                        pathIds.Remove(pathIds.Last());
                        junctionIds.Remove(junctionIds.Last());
                    }
                    else
                    {
                        junctionIds.Remove(junctionIds.Last());
                    }
                }
            }

            return longestPathLength;
        }
    }
}
