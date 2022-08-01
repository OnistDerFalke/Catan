using DataStorage;
using Player;
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
        
        [Header("Selected Zoom Scale")][Space(5)]
        [Tooltip("Selected Zoom Scale")]
        [SerializeField] private float selectedZoomScale;
        
        private Cards.CardType cardChosen;
        private Vector3 standardCardScale;

        void Start()
        {
            useCardButton.interactable = false;
            cardChosen = Cards.CardType.None;
            standardCardScale = knightCardButton.gameObject.transform.localScale;
            
            knightCardButton.onClick.AddListener(() => { ChooseCardButton(Cards.CardType.Knight);});
            roadBuildCardButton.onClick.AddListener(() => { ChooseCardButton(Cards.CardType.RoadBuild);});
            inventionCardButton.onClick.AddListener(() => { ChooseCardButton(Cards.CardType.Invention);});
            monopolCardButton.onClick.AddListener(() => { ChooseCardButton(Cards.CardType.Monopol);});
            useCardButton.onClick.AddListener(OnCardUseButtonClick);
        }
        
        void Update()
        {
            BlockCardsIfCannotBeUsed();
            UpdateCardsAvailable();
            ZoomCardIfChosen();
        }

        private void ZoomCardIfChosen()
        {
            if(knightCardButton.transform.localScale == selectedZoomScale * standardCardScale)
                knightCardButton.transform.localScale = standardCardScale;
            if(roadBuildCardButton.transform.localScale == selectedZoomScale * standardCardScale)
                roadBuildCardButton.transform.localScale = standardCardScale;
            if(inventionCardButton.transform.localScale == selectedZoomScale * standardCardScale)
                inventionCardButton.transform.localScale = standardCardScale;
            if(monopolCardButton.transform.localScale == selectedZoomScale * standardCardScale)
                monopolCardButton.transform.localScale = standardCardScale;
            
            switch (cardChosen)
            {
                case Cards.CardType.Knight:
                {
                    knightCardButton.transform.localScale = selectedZoomScale * standardCardScale;
                    break;
                }
                case Cards.CardType.RoadBuild:
                {
                    roadBuildCardButton.transform.localScale = selectedZoomScale * standardCardScale;
                    break;
                }
                case Cards.CardType.Invention:
                {
                    inventionCardButton.transform.localScale = selectedZoomScale * standardCardScale;
                    break;
                }
                case Cards.CardType.Monopol:
                {
                    monopolCardButton.transform.localScale = selectedZoomScale * standardCardScale;
                    break;
                }
                case Cards.CardType.None:
                {
                    knightCardButton.transform.localScale = standardCardScale;
                    roadBuildCardButton.transform.localScale = standardCardScale;
                    inventionCardButton.transform.localScale = standardCardScale;
                    monopolCardButton.transform.localScale = standardCardScale;
                    break;
                }
            }
        }

        private void UpdateCardsAvailable()
        {
            var currentPlayerCards = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards;
            var currentPlayerBlockedCards = 
                GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.CheckIfMarkAsBlocked();

            knightCardNumber.text = currentPlayerCards.GetCardNumber(Cards.CardType.Knight).ToString();
            roadBuildCardNumber.text = currentPlayerCards.GetCardNumber(Cards.CardType.RoadBuild).ToString();
            inventionCardNumber.text = currentPlayerCards.GetCardNumber(Cards.CardType.Invention).ToString();
            monopolCardNumber.text = currentPlayerCards.GetCardNumber(Cards.CardType.Monopol).ToString();

            //Destiny: if last card is blocked then set number of cards in red color
            //else set number of cards in white color
            if (currentPlayerBlockedCards[Cards.CardType.Knight])
                knightCardNumber.color = Color.red;
            else
                knightCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Cards.CardType.RoadBuild])
                roadBuildCardNumber.color = Color.red;
            else
                roadBuildCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Cards.CardType.Invention])
                inventionCardNumber.color = Color.red;
            else
                inventionCardNumber.color = Color.white;

            if (currentPlayerBlockedCards[Cards.CardType.Monopol])
                monopolCardNumber.color = Color.red;
            else
                monopolCardNumber.color = Color.white;
        }

        /// <summary>
        /// Blocks cards if it cannot be used
        /// </summary>
        private void BlockCardsIfCannotBeUsed()
        {
            knightCardButton.interactable = GameManager.CardsManager.CheckIfCurrentPlayerCanUseCard(Cards.CardType.Knight);
            roadBuildCardButton.interactable = GameManager.CardsManager.CheckIfCurrentPlayerCanUseCard(Cards.CardType.RoadBuild);
            inventionCardButton.interactable = GameManager.CardsManager.CheckIfCurrentPlayerCanUseCard(Cards.CardType.Invention);
            monopolCardButton.interactable = GameManager.CardsManager.CheckIfCurrentPlayerCanUseCard(Cards.CardType.Monopol);
        }

        /// <summary>
        /// Event on choosing the card
        /// </summary>
        /// <param name="type">Type of card clicked</param>
        private void ChooseCardButton(Cards.CardType type)
        {
            //Destiny: if card is blocked or it's not available, it cannot be used
            if (!GameManager.CardsManager.CheckIfCurrentPlayerCanUseCard(type) || GameManager.PopupManager.CheckIfWindowShown())
                return;

            //Destiny: Un-click card if is currently chosen
            if (type == cardChosen)
            {
                cardChosen = Cards.CardType.None;
                useCardButton.interactable = false;
                return;
            }

            cardChosen = type;

            //Destiny: if card not blocked, it is now chosen and use card button is getting unlocked
            useCardButton.interactable = true;
        }

        /// <summary>
        /// Handles click on use button of the card
        /// </summary>
        private void OnCardUseButtonClick()
        {
            //Destiny: Blocks the use button after clicking it
            useCardButton.interactable = false;

            //Destiny: Handles card use event of card chosen
            GameManager.State.Players[GameManager.State.CurrentPlayerId].UseCard(cardChosen);
            
            cardChosen = Cards.CardType.None;
        }
    }
}
