using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class CardsContentNavigation : MonoBehaviour
    {
        [Header("Available Cards Numbers")][Space(5)]
        [Tooltip("Knight Card Number")]
        [SerializeField] private Text knightCardNumber;
        [Tooltip("Road Build Card Number")]
        [SerializeField] private Text roadBuildCardNumber;
        [Tooltip("Invention Card Number")]
        [SerializeField] private Text inventionCardNumber;
        [Tooltip("Monopol Card Number")]
        [SerializeField] private Text monopolCardNumber;
        
        void Update()
        {
            UpdateCardsAvailable();
        }

        private void UpdateCardsAvailable()
        {
            var currentPlayerCards = GameManager.Players[GameManager.CurrentPlayer].properties.cards;
            knightCardNumber.text = currentPlayerCards.knightCards.ToString();
            roadBuildCardNumber.text = currentPlayerCards.roadBuildCards.ToString();
            inventionCardNumber.text = currentPlayerCards.inventionCards.ToString();
            monopolCardNumber.text = currentPlayerCards.monopolCards.ToString();
        }
    }
}
