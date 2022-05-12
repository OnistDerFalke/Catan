using UnityEditor.Experimental.GraphView;

namespace Board
{
    public class JunctionElement : BoardElement
    {
        //Destiny: Types of the junction
        private enum JunctionType
        {
            None,
            Village,
            City
        }
        
        //Destiny: Types of the ports
        private enum PortType
        {
            None,
            Normal,
            Wool,
            Wood,
            Iron,
            Stone,
            Wheat
        }

        //Destiny: True if building can be built (there is no enemy billage/city in neighbourhood)
        private bool canBuild;

        //Destiny: Paths that are neighbours of the junction
        private int[] pathsID;
        
        //Destiny: Port type near the building (if there is no port - set to None)
        private PortType portType;

        //Destiny: Type of building on junction (if there is not building - set to None)
        private JunctionType type;

        void Start()
        {
            type = JunctionType.None;
            portType = PortType.None;
            canBuild = true;
        }
    }
}