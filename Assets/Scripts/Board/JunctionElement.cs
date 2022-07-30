using System.Collections.Generic;
using static DataStorage.GameManager;

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
            Clay,
            Wheat
        }

        //Destiny: True if building can be built
        //(there is no enemy billage/city in neighbourhood and no one owns this path)
        public bool canBuild;

        //Destiny: Paths that are neighbors of the junction
        public List<int> pathsID;

        //Destiny: Junctions that are neighbors of the junction
        public List<int> junctionsID;

        //Destiny: Fields that are neighbors of the junction
        public List<int> fieldsID;

        //Destiny: Port type near the building (if there is no port - set to None)
        public PortType portType;

        //Destiny: Type of building on junction (if there is not building - set to None)
        public JunctionType type;

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

        /// <summary>
        /// Setting neighbors of field type
        /// </summary>
        /// <param name="fieldsID">List of neighbors of field type to set</param>
        public void SetFieldsID(List<int> fieldsID)
        {
            this.fieldsID = fieldsID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Id of a player who ownes given building
        /// if junction don't have an owner the function returns value equals to number of players</returns>
        public int GetOwnerId()
        {
            foreach(Player.Player player in Players)
            {
                if (player.properties.buildings.Contains(id))
                    return player.index;
            }

            return Players.Length;
        }

        void Awake()
        {
            boardElementType = BoardElementType.Junction;
            type = JunctionType.None;
            portType = PortType.None;
            canBuild = true;
        }

        public bool Available(dynamic element)
        {
            if (!PopupManager.CheckIfWindowShown() && element != null && element is JunctionElement)
            {
                var initialDistribution = SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                    SwitchingGameMode == SwitchingMode.InitialSwitchingSecond;

                if (initialDistribution)
                    return BuildManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)element).id);
                if (MovingUserMode == MovingMode.BuildVillage)
                    return BuildManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)element).id);
                if (BasicMovingUserMode != BasicMovingMode.TradePhase)
                    return BuildManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)element).id);
            }

            return false;
        }
    }
}