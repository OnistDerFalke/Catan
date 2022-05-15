using DataStorage;
using System.Collections.Generic;

namespace Board
{
    public class PathElement : BoardElement
    {
        //Destiny: True if path can be built (no one owns this path)
        public bool canBuild;

        //Destiny: Paths that are neighbours of the path
        public List<int> pathsID;

        //Destiny: Buildings that are neighbours of the path
        public List<int> buildingsID;

        //Destiny: Setting neighbours of path type
        public void SetPathsID(List<int> pathsID)
        {
            this.pathsID = pathsID;
        }

        //Destiny: Setting neighbours of buildings type
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