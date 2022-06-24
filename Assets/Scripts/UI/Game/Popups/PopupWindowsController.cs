using DataStorage;
using UnityEngine;

namespace UI.Game.Popups
{
    public class PopupWindowsController : MonoBehaviour
    {
        [Header("Popups")][Space(5)]
        [Tooltip("Monopol Popup")]
        [SerializeField] private GameObject monopolPopup;

        void Start()
        {
            GameManager.MonopolPopupShown = false;
        }
        
        void Update()
        {
            monopolPopup.SetActive(GameManager.MonopolPopupShown);
        }
    }
}
