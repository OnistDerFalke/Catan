using DataStorage;
using UnityEngine;

namespace UI.Game.Popups
{
    public class PopupWindowsController : MonoBehaviour
    {
        [Header("Popups")][Space(5)]
        [Tooltip("Monopol Popup")][SerializeField]
        private GameObject monopolPopup;
        [Tooltip("Invention Popup")][SerializeField] 
        private GameObject inventionPopup;
        [Tooltip("Thief Pay Popup")][SerializeField] 
        private GameObject thiefPayPopup;
        [Tooltip("Thief Player Choice Popup")][SerializeField]
        private GameObject thiefPlayerChoicePopup;
        [Tooltip("Obligatory Action Info Popup")][SerializeField]
        private GameObject obligatoryActionInfoPopup;
        [Tooltip("Bought Card Popup")][SerializeField] 
        private GameObject boughtCardPopup;
        [Tooltip("Land Trade Popup")][SerializeField] 
        private GameObject landTradePopup;
        [Tooltip("Land Trade Accept Popup")][SerializeField] 
        private GameObject landTradeAcceptPopup;
        [Tooltip("Land Trade Accept Popup")][SerializeField] 
        private GameObject seaTradePopup;
        [Tooltip("End Game Popup")][SerializeField] 
        private GameObject endGamePopup;
        
        void Start()
        {

        }

        void Update()
        {
            monopolPopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.MONOPOL_POPUP]);
            inventionPopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.INVENTION_POPUP]);
            thiefPayPopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.THIEF_PAY_POPUP]);
            thiefPlayerChoicePopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.THIEF_PLAYER_CHOICE_POPUP]);
            boughtCardPopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.BOUGHT_CARD_POPUP]);
            landTradePopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.LAND_TRADE_POPUP]);
            landTradeAcceptPopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.LAND_TRADE_ACCEPT_POPUP]);
            seaTradePopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.SEA_TRADE_POPUP]);
            endGamePopup.SetActive(GameManager.PopupManager.PopupsShown[GameManager.PopupManager.END_GAME_POPUP]);    
            
            obligatoryActionInfoPopup.SetActive(true);
        }
    }
}
