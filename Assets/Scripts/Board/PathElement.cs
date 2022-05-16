using System.Collections.Generic;

namespace Board
{
    public class PathElement : BoardElement
    {
        //Destiny: True if path can be built (no one owns this path)
        public bool canBuild;

        //Destiny: Paths that are neighbors of the path
        public List<int> pathsID;

        //Destiny: Buildings that are neighbors of the path
        public List<int> buildingsID;

        /// <summary>
        /// Setting neighbors of path type
        /// </summary>
        /// <param name="pathsID">List of neighbors of path type to set</param>
        public void SetPathsID(List<int> pathsID)
        {
            this.pathsID = pathsID;
        }

        /// <summary>
        /// Setting neighbors of building type
        /// </summary>
        /// <param name="buildingsID">List of neighbors of building type to set</param>
        public void SetBuildingsID(List<int> buildingsID)
        {
            this.buildingsID = buildingsID;
        }

        void Start()
        {
            canBuild = true;
        }
    }
}