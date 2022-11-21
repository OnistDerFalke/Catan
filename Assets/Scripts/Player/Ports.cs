using Board;
using DataStorage;
using System;
using System.Collections.Generic;
using static Board.JunctionElement;

namespace Assets.Scripts.Player
{
    [Serializable]
    public class Ports
    {
        public Dictionary<PortType, bool> ports = new();

        /// <summary>
        /// Creates ports
        /// </summary>
        /// <returns>Key: port exchange with details of sea trade; 
        /// Value: true if player has a given type of port exchange available</returns>
        public Ports()
        {
            SetupPorts();
        }

        /// <summary>
        /// Basic setup of player's ports
        /// </summary>
        private void SetupPorts()
        {
            ports = new();
            ports.Add(PortType.None, true);
            ports.Add(PortType.Normal, false);
            ports.Add(PortType.Iron, false);
            ports.Add(PortType.Wheat, false);
            ports.Add(PortType.Wood, false);
            ports.Add(PortType.Wool, false);
            ports.Add(PortType.Clay, false);
        }

        /// <summary>
        /// Sets the port type as available
        /// </summary>
        /// <param name="portType"></param>
        public void UpdatePort(PortType portType)
        {
            ports[portType] = true;
        }

        /// <summary>
        /// Updates current player's ports
        /// </summary>
        public void UpdatePorts()
        {
            SetupPorts();

            GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.buildings
                .ForEach(delegate (int junctionId)
            {
                UpdatePort(BoardManager.Junctions[junctionId].portType);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portType"></param>
        /// <returns>Key value pair according to given port type</returns>
        public bool GetPortKeyPair(PortType portType)
        {
            return ports[portType];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Dictionary of special ports owned by player</returns>
        public Dictionary<PortType, bool> GetSpecialPortsInfo()
        {
            Dictionary<PortType, bool> specialPorts = new();

            foreach(var port in ports)
            {
                if (port.Key != PortType.None && port.Key != PortType.Normal)
                {
                    specialPorts.Add(port.Key, port.Value);
                }
            }

            return specialPorts;
        }
    }
}
