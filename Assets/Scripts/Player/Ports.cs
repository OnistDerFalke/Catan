using System.Collections.Generic;
using System.Linq;
using static Board.JunctionElement;

namespace Assets.Scripts.Player
{
    public class PortDetails
    {
        public PortType portType;
        public int exchangeForOneResource;

        public PortDetails(PortType portType)
        {
            this.portType = portType;
            switch (portType)
            {
                case PortType.None:
                    exchangeForOneResource = 4;
                    break;
                case PortType.Normal:
                    exchangeForOneResource = 3;
                    break;
                case PortType.Iron:
                case PortType.Wheat:
                case PortType.Wood:
                case PortType.Wool:
                case PortType.Clay:
                    exchangeForOneResource = 2;
                    break;
            }
        }
    }

    public class Ports
    {
        public Dictionary<PortDetails, bool> ports;

        /// <summary>
        /// Creates ports
        /// </summary>
        /// <returns>Key: port exchange with details of sea trade; Value: true if player has a given type of port exchange available</returns>
        public Ports()
        {
            ports.Add(new PortDetails(PortType.None), true);
            ports.Add(new PortDetails(PortType.Normal), false);
            ports.Add(new PortDetails(PortType.Iron), false);
            ports.Add(new PortDetails(PortType.Wheat), false);
            ports.Add(new PortDetails(PortType.Wood), false);
            ports.Add(new PortDetails(PortType.Wool), false);
            ports.Add(new PortDetails(PortType.Clay), false);
        }

        /// <summary>
        /// Sets the port type as available
        /// </summary>
        /// <param name="portType"></param>
        public void UpdatePort(PortType portType)
        {
            var port = ports.Where(portDetail => portDetail.Key.portType == portType).FirstOrDefault().Key;
            ports[port] = true;
        }
    }
}
