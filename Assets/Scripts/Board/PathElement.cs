using DataStorage;
using System.Collections.Generic;

namespace Board
{
    public class PathElement : BoardElement
    {
        //Destiny: True if path can be built (no one owns this path)
        public bool canBuild;

        //Destiny: Paths that are neighbors of the path
        public List<int> pathsID;

        //Destiny: Junctions that are neighbors of the path
        public List<int> junctionsID;

        /// <summary>
        /// Setting neighbors of path type
        /// </summary>
        /// <param name="pathsID">List of neighbors of path type to set</param>
        public void SetPathsID(List<int> pathsID)
        {
            this.pathsID = pathsID;
        }

        /// <summary>
        /// Setting neighbors of junction type
        /// </summary>
        /// <param name="junctionsID">List of neighbors of junction type to set</param>
        public void SetJunctionsID(List<int> junctionsID)
        {
            this.junctionsID = junctionsID;
        }

        void Awake()
        {
            boardElementType = BoardElementType.Path;
            canBuild = true;
        }

        public bool Available()
        {
            var initialDistribution = GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst ||
                GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond;

            if (!GameManager.CheckIfWindowShown())
            {
                if (initialDistribution)
                    return GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
                if (GameManager.MovingUserMode == GameManager.MovingMode.BuildPath)
                    return GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
                if (GameManager.MovingUserMode == GameManager.MovingMode.OnePathForFree ||
                    GameManager.MovingUserMode == GameManager.MovingMode.TwoPathsForFree)
                    return GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
                if (GameManager.BasicMovingUserMode != GameManager.BasicMovingMode.TradePhase)
                    return GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
            }                

            return false;
        }
    }
}