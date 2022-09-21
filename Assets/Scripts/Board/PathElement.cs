using Assets.Scripts.Board.States;
using DataStorage;
using System.Collections.Generic;
using System.Linq;
using static Board.States.GameState;

namespace Board
{
    public class PathElement : BoardElement
    {
        //Destiny: Paths that are neighbors of the path
        public List<int> pathsID;

        //Destiny: Junctions that are neighbors of the path
        public List<int> junctionsID;

        public PathElement()
        {
            State = new PathState();
        }

        public void SetState(PathState state)
        {
            ((PathState)State).id = state.id;
            ((PathState)State).canBuild = state.canBuild;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Id of player who owns given path, 
        /// if path don't have an owner the function returns value equals to number of players</returns>
        public int GetOwnerId()
        {
            foreach (Player.Player player in GameManager.State.Players)
            {
                if (player.properties.paths.Contains(((PathState)State).id))
                {
                    return player.index;
                }
            }

            return GameManager.State.Players.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathId"></param>
        /// <returns>Id of the junction which is located between the given path and the path with id = pathId
        /// If paths don't have common junction the function returns value equals to number of junctions</returns>
        public int FindCommonJunction(int pathId)
        {
            foreach (int junctionId1 in junctionsID)
            {
                foreach (int junctionId2 in BoardManager.Paths[pathId].junctionsID)
                {
                    if (junctionId1 == junctionId2)
                    {
                        return junctionId1;
                    }
                }
            }

            return BoardManager.Junctions.Count();
        }

        public bool Available(dynamic element)
        {
            if (!GameManager.PopupManager.CheckIfWindowShown() && element != null && element is PathElement)
            {
                var initialDistribution = 
                    GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                    GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond;

                if (initialDistribution)
                {
                    return GameManager.BuildManager.CheckIfPlayerCanBuildPath(((PathState)((PathElement)element).State).id);
                }

                if (GameManager.State.MovingUserMode == MovingMode.BuildPath)
                {
                    return GameManager.BuildManager.CheckIfPlayerCanBuildPath(((PathState)((PathElement)element).State).id);
                }

                if (GameManager.State.MovingUserMode == MovingMode.OnePathForFree ||
                    GameManager.State.MovingUserMode == MovingMode.TwoPathsForFree)
                {
                    return GameManager.BuildManager.CheckIfPlayerCanBuildPath(((PathState)((PathElement)element).State).id);
                }

                if (GameManager.State.BasicMovingUserMode != BasicMovingMode.TradePhase &&
                    GameManager.State.MovingUserMode == MovingMode.Normal)
                {
                    return GameManager.BuildManager.CheckIfPlayerCanBuildPath(((PathState)((PathElement)element).State).id);
                }
            }

            return false;
        }

        void Awake()
        {
            elementType = ElementType.Path;
            ((PathState)State).canBuild = true;
        }
    }
}