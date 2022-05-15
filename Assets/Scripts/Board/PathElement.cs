using DataStorage;

namespace Board
{
    public class PathElement : BoardElement
    {
        //Destiny: True if path can be built (no one owns this path)
        public bool canBuild;

        void Start()
        {
            canBuild = true;
        }
    }
}