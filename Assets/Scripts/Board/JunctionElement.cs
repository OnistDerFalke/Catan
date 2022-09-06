using Assets.Scripts.Board.States;
using DataStorage;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Board.States.JunctionState;
using static Board.States.GameState;

namespace Board
{
    [Serializable]
    public class JunctionElement : BoardElement
    {        
        [SerializeField]
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

        //Destiny: Paths that are neighbors of the junction
        public List<int> pathsID;

        //Destiny: Junctions that are neighbors of the junction
        public List<int> junctionsID;

        //Destiny: Fields that are neighbors of the junction
        public List<int> fieldsID;

        //Destiny: Port type near the building (if there is no port - set to None)
        public PortType portType;

        public JunctionElement()
        {
            State = new JunctionState();
        }

        public void SetState(JunctionState state)
        {
            ((JunctionState)State).id = state.id;
            ((JunctionState)State).canBuild = state.canBuild;
            ((JunctionState)State).type = state.type;
        }

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
            foreach(Player.Player player in GameManager.State.Players)
            {
                if (player.properties.buildings.Contains(((JunctionState)State).id))
                {
                    return player.index;
                }
            }

            return GameManager.State.Players.Length;
        }

        public bool Available(dynamic element)
        {
            if (!GameManager.PopupManager.CheckIfWindowShown() && element != null && element is JunctionElement)
            {
                var initialDistribution =
                    GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                    GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond;

                if (initialDistribution)
                {
                    return GameManager.BuildManager
                        .CheckIfPlayerCanBuildBuilding(((JunctionState)((JunctionElement)element).State).id);
                }
                if (GameManager.State.MovingUserMode == MovingMode.BuildVillage)
                {
                    return GameManager.BuildManager
                        .CheckIfPlayerCanBuildBuilding(((JunctionState)((JunctionElement)element).State).id);
                }
                if (GameManager.State.BasicMovingUserMode != BasicMovingMode.TradePhase &&
                    GameManager.State.MovingUserMode == MovingMode.Normal)
                {
                    return GameManager.BuildManager
                        .CheckIfPlayerCanBuildBuilding(((JunctionState)((JunctionElement)element).State).id);
                }
            }

            return false;
        }

        void Awake()
        {
            boardElementType = BoardElementType.Junction;
            ((JunctionState)State).type = JunctionType.None;
            portType = PortType.None;
            ((JunctionState)State).canBuild = true;
        }
    }
}