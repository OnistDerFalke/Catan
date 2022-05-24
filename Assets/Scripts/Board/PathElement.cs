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
    }
}