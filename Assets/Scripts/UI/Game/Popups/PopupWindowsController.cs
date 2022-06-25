using DataStorage;
using Tests;
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
        [Tooltip("Thief Pay Popup")]
        [SerializeField] private GameObject thiefPayPopup;

        void Start()
        {
            GameManager.MonopolPopupShown = false;
            GameManager.InventionPopupShown = false;
            GameManager.ThiefPayPopupShown = false;
            
            //TESTING THIEF PAY POPUP
            //var test = new TestThiefPayPopup();
            //test.Invoke(false);
        }
        
        void Update()
        {
            monopolPopup.SetActive(GameManager.MonopolPopupShown);
            inventionPopup.SetActive(GameManager.InventionPopupShown);
            thiefPayPopup.SetActive(GameManager.ThiefPayPopupShown);
        }
    }
}
