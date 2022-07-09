using System.Windows.Forms;
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
        [Tooltip("Clay Value")]
        [SerializeField] private Text[] clayValueText;
        [Tooltip("Iron Value")]
        [SerializeField] private Text[] ironValueText;
        [Tooltip("Wheat Value")]
        [SerializeField] private Text[] wheatValueText;
        [Tooltip("Wood Value")]
        [SerializeField] private Text[] woodValueText;
        [Tooltip("Wool Value")]
        [SerializeField] private Text[] woolValueText;
        
        //Destiny: Players in transaction (0 - offer giver, 1 - offer receiver)
        [Header("Transaction Sides")][Space(5)]
        [Tooltip("Images")]
        [SerializeField] private Image[] playersColors = new Image[2];
        [Tooltip("Texts")]
        [SerializeField] private Text[] playersNames = new Text[2];
        
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Offer Accept Button")]
        [SerializeField] private Button acceptButton;
        [Tooltip("Offer Refuse Button")]
        [SerializeField] private Button refuseButton;

        void Start()
        {
            acceptButton.onClick.AddListener(OnAcceptButton);
            refuseButton.onClick.AddListener(() => {
                GameManager.PopupsShown[GameManager.LAND_TRADE_ACCEPT_POPUP] = false;
            });
        }
        
        void OnEnable()
        {
            acceptButton.interactable = CheckIfCanAcceptOffer();
            SetResourcesContent();
            SetPlayersContent();
        }

        private void OnAcceptButton()
        {
            //Destiny: Exchange of resources between players
            GameManager.Players[GameManager.CurrentPlayer].resources.
                SubtractResources(GameManager.LandTradeOfferContent[0]);
            GameManager.Players[GameManager.LandTradeOfferTarget].resources.
                SubtractResources(GameManager.LandTradeOfferContent[1]);
            GameManager.Players[GameManager.LandTradeOfferTarget].resources.
                AddResources(GameManager.LandTradeOfferContent[0]);
            GameManager.Players[GameManager.CurrentPlayer].resources.
                AddResources(GameManager.LandTradeOfferContent[1]);
            
            //Destiny: Hiding the popup after all
            GameManager.PopupsShown[GameManager.LAND_TRADE_ACCEPT_POPUP] = false;
        }

        /// <summary>
        /// Setting info content in popup about the players (names, colors)
        /// </summary>
        private void SetPlayersContent()
        {
            playersNames[0].text = GameManager.Players[GameManager.CurrentPlayer].name;
            playersNames[1].text = GameManager.Players[GameManager.LandTradeOfferTarget].name;
            
            playersColors[0].color = GameManager.Players[GameManager.CurrentPlayer].color switch
            {
                Player.Player.Color.Blue => Color.blue,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.Yellow => Color.yellow,
                Player.Player.Color.White => Color.white,
                _ => playersColors[GameManager.CurrentPlayer].color
            };
            
            playersColors[1].color = GameManager.Players[GameManager.LandTradeOfferTarget].color switch
            {
                Player.Player.Color.Blue => Color.blue,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.Yellow => Color.yellow,
                Player.Player.Color.White => Color.white,
                _ => playersColors[GameManager.LandTradeOfferTarget].color
            };
        }

        /// <summary>
        /// Setting info content about resources number for the offer
        /// </summary>
        private void SetResourcesContent()
        {
            for (var i = 0; i < 2; i++)
            {
                clayValueText[i].text = GameManager.LandTradeOfferContent[i][Resources.ResourceType.Clay].ToString();
                ironValueText[i].text = GameManager.LandTradeOfferContent[i][Resources.ResourceType.Iron].ToString();
                wheatValueText[i].text = GameManager.LandTradeOfferContent[i][Resources.ResourceType.Wheat].ToString();
                woodValueText[i].text = GameManager.LandTradeOfferContent[i][Resources.ResourceType.Wood].ToString();
                woolValueText[i].text = GameManager.LandTradeOfferContent[i][Resources.ResourceType.Wool].ToString();
            }
        }

        /// <summary>
        /// Checks if offer can be accepted (if player has enough resources to afford it
        /// </summary>
        /// <returns>If player can afford offer</returns>
        private bool CheckIfCanAcceptOffer()
        {
            if (!CheckIsEnoughResource(Resources.ResourceType.Clay)) return false;
            if (!CheckIsEnoughResource(Resources.ResourceType.Iron)) return false;
            if (!CheckIsEnoughResource(Resources.ResourceType.Wheat)) return false;
            if (!CheckIsEnoughResource(Resources.ResourceType.Wood)) return false;
            if (!CheckIsEnoughResource(Resources.ResourceType.Wool)) return false;
            return true;
        }

        /// <summary>
        /// Check if player has enough resource of type given to afford offer
        /// </summary>
        /// <param name="resource">Type of resource to check</param>
        /// <returns>If player can afford offer with his number of given resource type</returns>
        private bool CheckIsEnoughResource(Resources.ResourceType resource)
        {
            return GameManager.Players[GameManager.LandTradeOfferTarget].resources
                .GetResourceNumber(resource) >= GameManager.LandTradeOfferContent[1][resource];
        }
    }
}
