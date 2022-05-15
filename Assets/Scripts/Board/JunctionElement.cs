using UnityEditor.Experimental.GraphView;

namespace Board
{
    public class JunctionElement : BoardElement
    {
        //Destiny: Types of the junction
        public enum JunctionType
        {
            None,
            Village,
            City
        }
        
        //Destiny: Types of the ports
        public enum PortType
        {
            None,
            Normal,
            Wool,
            Wood,
            Iron,
            Stone,
            Wheat
        }

        //Destiny: True if building can be built
        //(there is no enemy billage/city in neighbourhood and no one owns this path)
        public bool canBuild;

        //Destiny: Paths that are neighbours of the junction
        public int[] pathsID;
        
        //Destiny: Port type near the building (if there is no port - set to None)
        public PortType portType;

        //Destiny: Type of building on junction (if there is not building - set to None)
        public JunctionType type;

        void Start()
        {
            type = JunctionType.None;
            portType = PortType.None;
            canBuild = true;
        }
    }
}