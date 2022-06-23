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
            var currentPlayerBlockedCards = GameManager.Players[GameManager.CurrentPlayer].properties.cards.CheckIfMarkAsBlocked();


            knightCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Knight).ToString();
            roadBuildCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.RoadBuild).ToString();
            inventionCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Invention).ToString();
            monopolCardNumber.text = currentPlayerCards.GetCardNumber(Player.Cards.CardType.Monopol).ToString();

            //Destiny: if last card is blocked then set number of cards in red color
            //else set number of cards in white color
            if (currentPlayerBlockedCards[Player.Cards.CardType.Knight])
                knightCardNumber.color = Color.red;
            else
                knightCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Player.Cards.CardType.RoadBuild])
                roadBuildCardNumber.color = Color.red;
            else
                roadBuildCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Player.Cards.CardType.Invention])
                inventionCardNumber.color = Color.red;
            else
                inventionCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Player.Cards.CardType.Monopol])
                monopolCardNumber.color = Color.red;
            else
                monopolCardNumber.color = Color.white;
        }
    }
}
