using System;

namespace Assets.Scripts.Board.States
{
    [Serializable]
    public class JunctionState : ElementState
    {
        //Destiny: Types of the junction
        public enum JunctionType
        {
            None,
            Village,
            City
        }

        //Destiny: True if building can be built
        //(there is no enemy billage/city in neighbourhood and no one owns this path)
        public bool canBuild;

        //Destiny: Type of building on junction (if there is not building - set to None)
        public JunctionType type;
    }
}
