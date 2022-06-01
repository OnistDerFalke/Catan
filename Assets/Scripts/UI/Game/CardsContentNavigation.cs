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
            knightCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Knight).ToString();
            roadBuildCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.RoadBuild).ToString();
            inventionCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Invention).ToString();
            monopolCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Monopol).ToString();
        }
    }
}
