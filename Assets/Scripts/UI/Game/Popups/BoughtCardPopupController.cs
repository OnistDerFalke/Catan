using DataStorage;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class BoughtCardPopupController : MonoBehaviour
    {
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")]
        [SerializeField] private Button confirmButton;
        
        [Header("Card Image")][Space(5)]
        [Tooltip("Card Image")]
        [SerializeField] private Image cardImage;

        [Header("Cards Sprites")] [Space(5)] 
        [Tooltip("Knight Card Sprite")] [SerializeField]
        private Sprite knightCardSprite;
        [Tooltip("Road Build Card Sprite")] [SerializeField]
        private Sprite roadBuildCardSprite;
        [Tooltip("Invention Card Sprite")] [SerializeField]
        private Sprite inventionCardSprite;
        [Tooltip("Monopol Card Sprite")] [SerializeField]
        private Sprite monopolCardSprite;
        [Tooltip("Victory Point Card Sprite")] [SerializeField]
        private Sprite victoryPointCardSprite;
        
        void Start()
        {
            //Destiny: Hiding the window after clicking confirm button
            confirmButton.onClick.AddListener(() =>
            {
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.BOUGHT_CARD_POPUP] = false;
            });
        }

        private void OnEnable()
        {
            ShowProperCard();
        }

        /// <summary>
        /// Shows proper card image on the popop
        /// </summary>
        private void ShowProperCard()
        {
            switch(GameManager.PopupManager.LastBoughtCard) {
                case Cards.CardType.Knight:
                    cardImage.sprite = knightCardSprite;
                    break;
                case Cards.CardType.RoadBuild:
                    cardImage.sprite = roadBuildCardSprite;
                    break;
                case Cards.CardType.Invention:
                    cardImage.sprite = inventionCardSprite;
                    break;
                case Cards.CardType.Monopol:
                    cardImage.sprite = monopolCardSprite;
                    break;
                case Cards.CardType.VictoryPoint:
                    cardImage.sprite = victoryPointCardSprite;
                    break;
                case Cards.CardType.None:
                    cardImage.sprite = null;
                    break;
            }
        }
    }
}
