using System;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class SeaTradePopupController : MonoBehaviour
    {
        //Destiny: Buttons for exchanging or aborting transaction
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Offer Button")] [SerializeField] private Button exchangeButton;
        [Tooltip("Abort Button")] [SerializeField] private Button abortButton;

        void Start()
        {
            //Destiny: Adding click listeners to flow control buttons
            exchangeButton.onClick.AddListener(OnExchangeButton);
            abortButton.onClick.AddListener(OnAbortButton);
        }

        private void OnExchangeButton()
        {
            //TODO: Implement what happens on exchange
        }

        private void OnAbortButton()
        {
            GameManager.PopupsShown[GameManager.SEA_TRADE_POPUP] = false;
        }
    }
}
