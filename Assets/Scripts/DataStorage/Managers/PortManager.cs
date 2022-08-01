using System.Collections.Generic;
using static Board.JunctionElement;

namespace Assets.Scripts.DataStorage.Managers
{
    public class PortManager
    {
        public Dictionary<PortType, int> portsInfo;

        public PortManager()
        {
            portsInfo = new();

            portsInfo.Add(PortType.None, 4);
            portsInfo.Add(PortType.Normal, 3);
            portsInfo.Add(PortType.Iron, 2);
            portsInfo.Add(PortType.Wheat, 2);
            portsInfo.Add(PortType.Wood, 2);
            portsInfo.Add(PortType.Wool, 2);
            portsInfo.Add(PortType.Clay, 2);
        }
    }
}
