using DataStorage;
using UnityEngine;

namespace UI.Game.Popups
{
    public class PopupPositioner : MonoBehaviour
    {
        [Header("Background")][Space(5)]
        [Tooltip("Background")] [SerializeField] private GameObject background;
        
        void OnDisable()
        {
            background.SetActive(false);
        }

        void Update()
        {
            //Destiny: Changes position of the popup with offset of tabs menu has actually did
            var t = transform;
            var pos = t.localPosition;
            pos.x = GameManager.PopupManager.PopupOffset/2;
            t.localPosition = pos;
            background.SetActive(true);
        }
    }
}
