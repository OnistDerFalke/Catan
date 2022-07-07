using System;
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
        [Tooltip("Thief Player Choice Popup")]
        [SerializeField] private GameObject thiefPlayerChoicePopup;
        [Tooltip("Obligatory Action Info Popup")]
        [SerializeField] private GameObject obligatoryActionInfoPopup;
        [Tooltip("Bought Card Popup")]
        [SerializeField] private GameObject boughtCardPopup;
        [Tooltip("Land Trade Popup")]
        [SerializeField] private GameObject landTradePopup;
        
        void Start()
        {
            GameManager.PopupsShown.Add(GameManager.MONOPOL_POPUP, false);
            GameManager.PopupsShown.Add(GameManager.INVENTION_POPUP, false);
            GameManager.PopupsShown.Add(GameManager.THIEF_PAY_POPUP, false);
            GameManager.PopupsShown.Add(GameManager.THIEF_PLAYER_CHOICE_POPUP, false);
            GameManager.PopupsShown.Add(GameManager.BOUGHT_CARD_POPUP, false);
            GameManager.PopupsShown.Add(GameManager.LAND_TRADE_POPUP, false);

            //TESTING THIEF PAY POPUP
            //var test = new TestThiefPayPopup();
            //test.Invoke(false);
        }

        void Update()
        {
            monopolPopup.SetActive(GameManager.PopupsShown[GameManager.MONOPOL_POPUP]);
            inventionPopup.SetActive(GameManager.PopupsShown[GameManager.INVENTION_POPUP]);
            thiefPayPopup.SetActive(GameManager.PopupsShown[GameManager.THIEF_PAY_POPUP]);
            thiefPlayerChoicePopup.SetActive(GameManager.PopupsShown[GameManager.THIEF_PLAYER_CHOICE_POPUP]);
            boughtCardPopup.SetActive(GameManager.PopupsShown[GameManager.BOUGHT_CARD_POPUP]);
            landTradePopup.SetActive(GameManager.PopupsShown[GameManager.LAND_TRADE_POPUP]);
                
            obligatoryActionInfoPopup.SetActive(true);
        }
    }
}
