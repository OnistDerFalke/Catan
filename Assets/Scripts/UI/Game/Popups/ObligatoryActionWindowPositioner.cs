using DataStorage;
using UnityEngine;

namespace UI.Game.Popups
{
    public class ObligatoryActionWindowPositioner : MonoBehaviour
    {
        void Update()
        {
            //Destiny: Changes position of the popup with offset of tabs menu has actually did
            var t = transform;
            var pos = t.localPosition;
            pos.x = GameManager.PopupManager.PopupOffset/2;
            t.localPosition = pos;
        }
    }
}