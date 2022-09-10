using Assets.Scripts.DataStorage.Managers;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    public class LandTradeAcceptPopupController : MonoBehaviour
    {
        //Destiny: Resources offer elements (index: 0 - to give, 1 - to take)
        [Header("Values Texts")][Space(5)]
        [Tooltip("Clay Value")][SerializeField] 
        private Text[] clayValueText;
        [Tooltip("Iron Value")][SerializeField]
        private Text[] ironValueText;
        [Tooltip("Wheat Value")][SerializeField]
        private Text[] wheatValueText;
        [Tooltip("Wood Value")][SerializeField] 
        private Text[] woodValueText;
        [Tooltip("Wool Value")][SerializeField] 
        private Text[] woolValueText;
        
        //Destiny: Players in transaction (0 - offer giver, 1 - offer receiver)
        [Header("Transaction Sides")][Space(5)]
        [Tooltip("Images")][SerializeField]
        private Image[] playersColors = new Image[2];
        [Tooltip("Texts")][SerializeField]
        private Text[] playersNames = new Text[2];
        
        //Destiny: Buttons for accepting or refusing the offer
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Offer Accept Button")][SerializeField]
        private Button acceptButton;
        [Tooltip("Offer Refuse Button")][SerializeField] 
        private Button refuseButton;

        //Destiny: Texts showing how many resources offer receiver has
        [Header("Availability Texts")][Space(5)]
        [Tooltip("Availability Prefix")][SerializeField] 
        private string availabilityPrefix;
        [Tooltip("Clay Availability")][SerializeField]
        private Text clayAvailabilityText;
        [Tooltip("Iron Availability")][SerializeField] 
        private Text ironAvailabilityText;
        [Tooltip("Wheat Availability")][SerializeField]
        private Text wheatAvailabilityText;
        [Tooltip("Wood Availability")][SerializeField]
        private Text woodAvailabilityText;
        [Tooltip("Wool Availability")][SerializeField]
        private Text woolAvailabilityText;

        //Destiny: Info that offer cannot be accepted
        [Header("Cannot Accept Info")][Space(5)]
        [Tooltip("Cannot Accept Info")][SerializeField]
        private GameObject cannotAcceptInfo;
        
        void Start()
        {
            //Destiny: Adding click listeners to flow control buttons
            acceptButton.onClick.AddListener(OnAcceptButton);
            refuseButton.onClick.AddListener(OnRefuseButton);
        }
        
        void OnEnable()
        {
            //Destiny: If offer receiver has not enough resources, accept button is blocked and show info
            //that offer cannot be accepted
            acceptButton.interactable = CheckIfCanAcceptOffer();
            cannotAcceptInfo.SetActive(!acceptButton.interactable);
            
            //Destiny: Setting resources and players info in a popup (numbers, images, names, etc.)
            SetResourcesContent();
            SetPlayersContent();
            
            //Destiny: Updates numbers of resources that offer receiver actually has
            UpdateAvailabilityTexts();
        }

        /// <summary>
        /// Event starting on accept button click
        /// </summary>
        private void OnAcceptButton()
        {
            //Destiny: Exchange of resources between players
            GameManager.TradeManager.ExchangeResourcesTwoPlayers(
                GameManager.State.CurrentPlayerId,
                GameManager.TradeManager.LandTradeOfferTarget,
                GameManager.TradeManager.LandTradeOfferContent[0],
                GameManager.TradeManager.LandTradeOfferContent[1]);

            //Destiny: Hiding the popup after clicking the button
            GameManager.PopupManager.PopupsShown[PopupManager.LAND_TRADE_ACCEPT_POPUP] = false;
        }

        /// <summary>
        /// Event starting on refuse button click
        /// </summary>
        private void OnRefuseButton()
        {
            GameManager.PopupManager.PopupsShown[PopupManager.LAND_TRADE_ACCEPT_POPUP] = false;
        }

        /// <summary>
        /// Setting info content in popup about the players (names, colors)
        /// </summary>
        private void SetPlayersContent()
        {
            //Destiny: Setting name of offer giver (index 0) and offer receiver (index 1)
            playersNames[0].text = GameManager.State.Players[GameManager.State.CurrentPlayerId].name;
            playersNames[1].text = GameManager.State.Players[GameManager.TradeManager.LandTradeOfferTarget].name;
            
            //Destiny: Setting colors of offer giver (index 0) and offer receiver (index 1)
            playersColors[0].color = GameManager.State.Players[GameManager.State.CurrentPlayerId].color switch
            {
                Player.Player.Color.Blue => Color.blue,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.Yellow => Color.yellow,
                Player.Player.Color.White => Color.white,
                _ => playersColors[GameManager.State.CurrentPlayerId].color
            };
            playersColors[1].color = GameManager.State.Players[GameManager.TradeManager.LandTradeOfferTarget].color switch
            {
                Player.Player.Color.Blue => Color.blue,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.Yellow => Color.yellow,
                Player.Player.Color.White => Color.white,
                _ => playersColors[GameManager.TradeManager.LandTradeOfferTarget].color
            };
        }

        /// <summary>
        /// Setting info content about resources number for the offer:
        /// - offer receiver takes (index 0)
        /// - offer receiver gives (index 1)
        /// </summary>
        private void SetResourcesContent()
        {
            for (var i = 0; i < 2; i++)
            {
                var resources = GameManager.TradeManager.LandTradeOfferContent[i];
                clayValueText[i].text = resources[Resources.ResourceType.Clay].ToString();
                ironValueText[i].text = resources[Resources.ResourceType.Iron].ToString();
                wheatValueText[i].text = resources[Resources.ResourceType.Wheat].ToString();
                woodValueText[i].text = resources[Resources.ResourceType.Wood].ToString();
                woolValueText[i].text = resources[Resources.ResourceType.Wool].ToString();
            }
        }

        /// <summary>
        /// Checks if offer can be accepted (if player has enough resources to afford it)
        /// </summary>
        /// <returns>If player can afford offer</returns>
        private bool CheckIfCanAcceptOffer()
        {
            return GameManager.State.Players[GameManager.TradeManager.LandTradeOfferTarget].resources
                .CheckIfPlayerHasEnoughResources(GameManager.TradeManager.LandTradeOfferContent[1]);
        }
        
        /// <summary>
        /// Updates the texts with available resources
        /// </summary>
        private void UpdateAvailabilityTexts()
        {
            var resources = GameManager.State.Players[GameManager.TradeManager.LandTradeOfferTarget].resources.GetResourcesNumber();
            clayAvailabilityText.text = $"{availabilityPrefix} {resources[Resources.ResourceType.Clay]}";
            ironAvailabilityText.text = $"{availabilityPrefix} {resources[Resources.ResourceType.Iron]}";
            wheatAvailabilityText.text = $"{availabilityPrefix} {resources[Resources.ResourceType.Wheat]}";
            woodAvailabilityText.text = $"{availabilityPrefix} {resources[Resources.ResourceType.Wood]}";
            woolAvailabilityText.text = $"{availabilityPrefix} {resources[Resources.ResourceType.Wool]}";
        }
    }
}
