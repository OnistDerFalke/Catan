using DataStorage;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class BoughtCardPopupController : MonoBehaviour
    {
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")][SerializeField] 
        private Button confirmButton;
        
        [Header("Card Image")][Space(5)]
        [Tooltip("Card Image")][SerializeField] 
        private Image cardImage;

        [Header("Cards Sprites")] [Space(5)] 
        [Tooltip("Knight Card Sprite")][SerializeField]
        private Sprite knightCardSprite;
        [Tooltip("Road Build Card Sprite")][SerializeField]
        private Sprite roadBuildCardSprite;
        [Tooltip("Invention Card Sprite")][SerializeField]
        private Sprite inventionCardSprite;
        [Tooltip("Monopol Card Sprite")][SerializeField]
        private Sprite monopolCardSprite;
        [Tooltip("Victory Point Card Sprite")][SerializeField]
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
            cardImage.sprite = GameManager.PopupManager.LastBoughtCard switch
            {
                Cards.CardType.Knight => knightCardSprite,
                Cards.CardType.RoadBuild => roadBuildCardSprite,
                Cards.CardType.Invention => inventionCardSprite,
                Cards.CardType.Monopol => monopolCardSprite,
                Cards.CardType.VictoryPoint => victoryPointCardSprite,
                _ => null
            };
        }
    }
}
