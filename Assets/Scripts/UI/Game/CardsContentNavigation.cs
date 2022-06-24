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
        
        [Header("Available Cards Buttons")][Space(5)]
        [Tooltip("Knight Card Button")]
        [SerializeField] private Button knightCardButton;
        [Tooltip("Road Build Card Button")]
        [SerializeField] private Button roadBuildCardButton;
        [Tooltip("Invention Card Button")]
        [SerializeField] private Button inventionCardButton;
        [Tooltip("Monopol Card Button")]
        [SerializeField] private Button monopolCardButton;

        [Header("Use Card Button")][Space(5)]
        [Tooltip("Use Card Button")]
        [SerializeField] private Button useCardButton;
        
        private Player.Cards.CardType cardChosen;
        void Start()
        {
            useCardButton.enabled = false;
            
            knightCardButton.onClick.AddListener(() => { ChooseCardButton(Player.Cards.CardType.Knight);});
            roadBuildCardButton.onClick.AddListener(() => { ChooseCardButton(Player.Cards.CardType.RoadBuild);});
            inventionCardButton.onClick.AddListener(() => { ChooseCardButton(Player.Cards.CardType.Invention);});
            monopolCardButton.onClick.AddListener(() => { ChooseCardButton(Player.Cards.CardType.Monopol);});
            useCardButton.onClick.AddListener(OnCardUseButtonClick);
        }
        
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

        /// <summary>
        /// Event on choosing the card
        /// </summary>
        /// <param name="type">Type of card clicked</param>
        private void ChooseCardButton(Player.Cards.CardType type)
        {
            cardChosen = type;
            
            //Destiny: if card is blocked or it's not available, it cannot be used
            var currentPlayerCards = GameManager.Players[GameManager.CurrentPlayer].properties.cards;
            var currentPlayerBlockedCards = GameManager.Players[GameManager.CurrentPlayer].properties.cards.CheckIfMarkAsBlocked();
            if (currentPlayerBlockedCards[type] || currentPlayerCards.GetCardNumber(type)<=0) return;
            
            //Destiny: if card not blocked, it is now chosen and use card button is getting unlocked
            useCardButton.enabled = true;
        }

        /// <summary>
        /// Handles click on use button of the card
        /// </summary>
        private void OnCardUseButtonClick()
        {
            //Destiny: Blocks the use button after clicking it
            useCardButton.enabled = false;
            
            //Destiny: Handles card use event of card chosen
            switch (cardChosen)
            {
                case Player.Cards.CardType.Knight:
                    //TODO: Card use handle (dont forget to remove card from player)
                    break;
                case Player.Cards.CardType.RoadBuild:
                    //TODO: Card use handle (dont forget to remove card from player)
                    break;
                case Player.Cards.CardType.Invention:
                    //TODO: Card use handle (dont forget to remove card from player)
                    break;
                case Player.Cards.CardType.Monopol:
                    //TODO: Card use handle (dont forget to remove card from player)
                    break;
            }
        }
    }
}
