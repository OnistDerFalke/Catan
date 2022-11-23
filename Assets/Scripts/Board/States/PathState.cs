using System;

namespace Assets.Scripts.Board.States
{
    [Serializable]
    public class PathState : ElementState
    {
        //Destiny: True if path can be built (no one owns this path)
        public bool canBuild;

        //Destiny: True if path was built for free
        public bool forFree = false;
    }
}
