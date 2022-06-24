using DataStorage;
using UnityEngine;

namespace UI.Game.Popups
{
    public class PopupWindowsController : MonoBehaviour
    {
        [Header("Popups")][Space(5)]
        [Tooltip("Monopol Popup")]
        [SerializeField] private GameObject monopolPopup;
        [Tooltip("Invention Popup")]
        [SerializeField] private GameObject inventionPopup;

        void Start()
        {
            GameManager.MonopolPopupShown = false;
            GameManager.InventionPopupShown = false;
        }
        
        void Update()
        {
            monopolPopup.SetActive(GameManager.MonopolPopupShown);
            inventionPopup.SetActive(GameManager.InventionPopupShown);
        }
    }
}
