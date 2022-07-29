using Tests;
using UnityEngine;
using static DataStorage.GameManager;

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
        [Tooltip("Thief Player Choice Popup")]
        [SerializeField] private GameObject thiefPlayerChoicePopup;
        [Tooltip("Obligatory Action Info Popup")]
        [SerializeField] private GameObject obligatoryActionInfoPopup;
        [Tooltip("Bought Card Popup")]
        [SerializeField] private GameObject boughtCardPopup;
        [Tooltip("Land Trade Popup")]
        [SerializeField] private GameObject landTradePopup;
        [Tooltip("Land Trade Accept Popup")]
        [SerializeField] private GameObject landTradeAcceptPopup;
        [Tooltip("Land Trade Accept Popup")]
        [SerializeField] private GameObject seaTradePopup;
        [Tooltip("End Game Popup")]
        [SerializeField] private GameObject endGamePopup;
        
        void Start()
        {
            //TESTING THIEF PAY POPUP
            //var test = new TestThiefPayPopup();
            //test.Invoke(false);
        }

        void Update()
        {
            monopolPopup.SetActive(PopupManager.PopupsShown[PopupManager.MONOPOL_POPUP]);
            inventionPopup.SetActive(PopupManager.PopupsShown[PopupManager.INVENTION_POPUP]);
            thiefPayPopup.SetActive(PopupManager.PopupsShown[PopupManager.THIEF_PAY_POPUP]);
            thiefPlayerChoicePopup.SetActive(PopupManager.PopupsShown[PopupManager.THIEF_PLAYER_CHOICE_POPUP]);
            boughtCardPopup.SetActive(PopupManager.PopupsShown[PopupManager.BOUGHT_CARD_POPUP]);
            landTradePopup.SetActive(PopupManager.PopupsShown[PopupManager.LAND_TRADE_POPUP]);
            landTradeAcceptPopup.SetActive(PopupManager.PopupsShown[PopupManager.LAND_TRADE_ACCEPT_POPUP]);
            seaTradePopup.SetActive(PopupManager.PopupsShown[PopupManager.SEA_TRADE_POPUP]);
            endGamePopup.SetActive(PopupManager.PopupsShown[PopupManager.END_GAME_POPUP]);    
            
            obligatoryActionInfoPopup.SetActive(true);
        }
    }
}
