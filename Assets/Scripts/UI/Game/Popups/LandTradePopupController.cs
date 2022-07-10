using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Web.Razor.Parser.SyntaxTree;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    public class LandTradePopupController : MonoBehaviour
    {
        //Destiny: Buttons for sending or aborting an offer
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Offer Button")] [SerializeField] private Button offerButton;
        [Tooltip("Abort Button")] [SerializeField] private Button abortButton;
        
        //Destiny: Buttons, names and colors of players part of popup
        [Header("Players UI Elements")][Space(5)]
        [Tooltip("Players Buttons")] [SerializeField] private Button[] playersButtons = new Button[4];
        [Tooltip("Players Names")] [SerializeField] private Text[] playersNames = new Text[4];
        [Tooltip("Players Colors")] [SerializeField] private Image[] playersColors = new Image[4];

        //Destiny: Resources values chosen in popup (index: 0 - to give, 1 - to take)
        [Header("Values Texts")][Space(5)]
        [Tooltip("Clay Value")] [SerializeField] private Text[] clayValueText;
        [Tooltip("Iron Value")] [SerializeField] private Text[] ironValueText;
        [Tooltip("Wheat Value")] [SerializeField] private Text[] wheatValueText;
        [Tooltip("Wood Value")] [SerializeField] private Text[] woodValueText;
        [Tooltip("Wool Value")] [SerializeField] private Text[] woolValueText;

        //Destiny: Buttons for adding resources to offer (index: 0 - to give, 1 - to take)
        [Header("Add Buttons")][Space(5)]
        [Tooltip("Clay Add")] [SerializeField] private Button[] clayAdd;
        [Tooltip("Iron Add")] [SerializeField] private Button[] ironAdd;
        [Tooltip("Wheat Add")] [SerializeField] private Button[] wheatAdd;
        [Tooltip("Wood Add")] [SerializeField] private Button[] woodAdd;
        [Tooltip("Wool Add")] [SerializeField] private Button[] woolAdd;
        
        //Destiny: Buttons for removing resources from offer (index: 0 - to give, 1 - to take)
        [Header("Remove Buttons")][Space(5)] 
        [Tooltip("Clay Remove")] [SerializeField] private Button[] clayRemove;
        [Tooltip("Iron Remove")] [SerializeField] private Button[] ironRemove;
        [Tooltip("Wheat Remove")] [SerializeField] private Button[] wheatRemove;
        [Tooltip("Wood Remove")] [SerializeField] private Button[] woodRemove;
        [Tooltip("Wool Remove")] [SerializeField] private Button[] woolRemove;
        
        //Destiny: Texts showing how many resources offer giver has
        [Header("Availability Texts")][Space(5)]
        [Tooltip("Availability Prefix")] [SerializeField] private string availabilityPrefix;
        [Tooltip("Clay Availability")] [SerializeField] private Text clayAvailabilityText;
        [Tooltip("Iron Availability")] [SerializeField] private Text ironAvailabilityText;
        [Tooltip("Wheat Availability")] [SerializeField] private Text wheatAvailabilityText;
        [Tooltip("Wood Availability")] [SerializeField] private Text woodAvailabilityText;
        [Tooltip("Wool Availability")] [SerializeField] private Text woolAvailabilityText;
        
        //Destiny: Player that is actually chosen (-1 == no player has been chosen)
        private int chosenPlayer;
        
        //Destiny: Values of offer resources (index: 0 - to give, 1 - to take)
        private int[] clayValue, ironValue, wheatValue, woodValue, woolValue;
        
        //Destiny: Vertical placement - array of positions available for player choice buttons
        private float[] verticalPlacement;

        void Start()
        {
            //Destiny: Chosen player is unset on start
            chosenPlayer = -1;
            
            //Destiny: Adding click listeners to flow control buttons
            offerButton.onClick.AddListener(OnOfferButton);
            abortButton.onClick.AddListener(OnAbortButton);

            //Destiny: Adding click listeners for player choice buttons
            for (var i = 0; i < playersButtons.Length; i++)
            {
                var index = i;
                //Destiny: Setting clicked button player as chosen player
                playersButtons[i].onClick.AddListener(() => { chosenPlayer = index;});
            }

            //Destiny: Setting click listeners for + and - buttons near the values
            for (var i = 0; i < 2; i++)
            {
                var type = i;
                
                //Destiny: Clicking + button increases value
                clayAdd[i].onClick.AddListener(() => { clayValue[type]++; });
                ironAdd[i].onClick.AddListener(() => { ironValue[type]++; });
                wheatAdd[i].onClick.AddListener(() => { wheatValue[type]++; });
                woodAdd[i].onClick.AddListener(() => { woodValue[type]++; });
                woolAdd[i].onClick.AddListener(() => { woolValue[type]++; });
                
                //Destiny: Clicking - button decreases value
                clayRemove[i].onClick.AddListener(() => { clayValue[type]--; });
                ironRemove[i].onClick.AddListener(() => { ironValue[type]--; });
                wheatRemove[i].onClick.AddListener(() => { wheatValue[type]--; });
                woodRemove[i].onClick.AddListener(() => { woodValue[type]--; });
                woolRemove[i].onClick.AddListener(() => { woolValue[type]--; });
            }
        }

        void OnEnable()
        { 
            //Destiny: Get vertical placement heights if it was not set before
            if (verticalPlacement == null)
            {
                verticalPlacement = new float[playersButtons.Length];
                for (var i = 0; i < playersButtons.Length; i++)
                    verticalPlacement[i] = playersButtons[i].gameObject.transform.localPosition.y;
            }

            //Destiny: Offer button is default disabled
            offerButton.interactable = false;
            
            //Destiny: Clearing all values
            ClearValues();
            
            //Destiny: Player is not chosen on default
            chosenPlayer = -1;
            
            //Destiny: Setting info about players in popup (buttons, images, names)
            SetPlayersInfo();
            
            //Destiny: Updates numbers of resources that offer giver actually has
            UpdateAvailabilityTexts();
        }

        void Update()
        {
            //Destiny: Zooms the player button if it has been clicked
            for (var i = 0; i < playersButtons.Length; i++)
                playersButtons[i].gameObject.transform.localScale = i == chosenPlayer ?
                    new Vector3(1.2f, 1.2f, 1.2f) : Vector3.one;
            
            //Destiny: Values of the resources chosen needs to be updated continuously
            UpdateValuesTexts();
            
            //Destiny: Blocking + and - buttons if more or less of any resource cannot be chosen
            BlockIfLimit();
            BlockIfZero();
            
            //Destiny: Offer button unlocks if there was min 1 resource to give and to take and player has been chosen
            offerButton.interactable = CheckIfNotDonation() && chosenPlayer >= 0;
        }
        
        /// <summary>
        /// Clears all chosen resources values
        /// </summary>
        private void ClearValues()
        {
            clayValue = new int[2];
            ironValue = new int[2];
            wheatValue = new int[2];
            woodValue = new int[2];
            woolValue = new int[2];
        }
        
        /// <summary>
        /// Blocks - button when chosen value is 0 or less (the second one should not have happen)
        /// </summary>
        private void BlockIfZero()
        {
            for (var i = 0; i < 2; i++)
            {
                clayRemove[i].gameObject.SetActive(clayValue[i] > 0);
                ironRemove[i].gameObject.SetActive(ironValue[i] > 0);
                wheatRemove[i].gameObject.SetActive(wheatValue[i] > 0);
                woodRemove[i].gameObject.SetActive(woodValue[i] > 0);
                woolRemove[i].gameObject.SetActive(woolValue[i] > 0);
            }
        }

        /// <summary>
        /// Blocks + button when max value has been reached
        /// </summary>
        private void BlockIfLimit()
        {
            //Destiny: Player cannot offer more resources than he has
            clayAdd[0].gameObject.SetActive(clayValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Clay));
            ironAdd[0].gameObject.SetActive(ironValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Iron));
            wheatAdd[0].gameObject.SetActive(wheatValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wheat));
            woodAdd[0].gameObject.SetActive(woodValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wood));
            woolAdd[0].gameObject.SetActive(woolValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wool));
            
            //Destiny: Player can ask only max resources that can be asked
            clayAdd[1].gameObject.SetActive(clayValue[1] < GameManager.MaxResourcesNumber);
            ironAdd[1].gameObject.SetActive(ironValue[1] < GameManager.MaxResourcesNumber);
            wheatAdd[1].gameObject.SetActive(wheatValue[1] < GameManager.MaxResourcesNumber);
            woodAdd[1].gameObject.SetActive(woodValue[1] < GameManager.MaxResourcesNumber);
            woolAdd[1].gameObject.SetActive(woolValue[1] < GameManager.MaxResourcesNumber);
        }

        /// <summary>
        /// Checks if player don't want to exchange not giving something from resources
        /// </summary>
        /// <returns>If exchange is legal</returns>
        private bool CheckIfNotDonation()
        {
            for (var i = 0; i < 2; i++)
            {
                //Destiny: Min 1 resource of any type must be offered to give and to receive
                if (clayValue[i] + ironValue[i] + wheatValue[i] + woodValue[i] + woolValue[i] <= 0)
                    return false;
            }
            return true;
        }
        
        
        /// <summary>
        /// Updates the value texts under resources
        /// </summary>
        private void UpdateValuesTexts()
        {
            for (var i = 0; i < 2; i++)
            {
                clayValueText[i].text = clayValue[i].ToString();
                ironValueText[i].text = ironValue[i].ToString();
                wheatValueText[i].text = wheatValue[i].ToString();
                woodValueText[i].text = woodValue[i].ToString();
                woolValueText[i].text = woolValue[i].ToString();
            }
        }

        /// <summary>
        /// Sets info about actual players (colors, names) on the popup view
        /// </summary>
        private void SetPlayersInfo()
        {
            //Destiny: Hide all buttons on start
            foreach (var button in playersButtons) 
                button.gameObject.SetActive(false);

            //Destiny: Not every player can be chosen so available players are placed in order in vertical grid
            var placeInVerticalGrid = 0;
            foreach (var player in GameManager.Players)
            {
                //Destiny: Player cannot choose himself
                if (player.index == GameManager.Players[GameManager.CurrentPlayer].index)
                    continue;
                
                //Destiny: Placing player button in next empty place in vertical grid
                var pos = playersButtons[player.index].gameObject.transform.localPosition;
                pos.y = verticalPlacement[placeInVerticalGrid];
                playersButtons[player.index].gameObject.transform.localPosition = pos;
                placeInVerticalGrid++;
                
                //Destiny: Setting names
                playersNames[player.index].text = player.name;
                
                //Destiny: Setting colors
                playersColors[player.index].color = GameManager.Players[player.index].color switch
                {
                    Player.Player.Color.Blue => Color.blue,
                    Player.Player.Color.Red => Color.red,
                    Player.Player.Color.Yellow => Color.yellow,
                    Player.Player.Color.White => Color.white,
                    _ => playersColors[player.index].color
                };

                //Destiny: Showing only info about players that play (if 3 players, fourth is not shown)
                playersButtons[player.index].gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// Updates the texts with available resources
        /// </summary>
        private void UpdateAvailabilityTexts()
        {
            clayAvailabilityText.text = $"{availabilityPrefix} {GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Clay)}";
            ironAvailabilityText.text = $"{availabilityPrefix} {GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Iron)}";
            wheatAvailabilityText.text = $"{availabilityPrefix} {GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wheat)}";
            woodAvailabilityText.text = $"{availabilityPrefix} {GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wood)}";
            woolAvailabilityText.text = $"{availabilityPrefix} {GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wool)}";
        }

        /// <summary>
        /// Closing the popup and sending an offer
        /// </summary>
        private void OnOfferButton()
        {
            //Destiny: Offer button is now disabled again
            offerButton.interactable = false;
            
            //Destiny: Setting info for second popup and transaction
            GameManager.LandTradeOfferContent = GetOfferContent();
            GameManager.LandTradeOfferTarget = chosenPlayer;
            
            //Destiny: This popup closes and new popup for offer receiver to accept offer appears
            GameManager.PopupsShown[GameManager.LAND_TRADE_ACCEPT_POPUP] = true;
            GameManager.PopupsShown[GameManager.LAND_TRADE_POPUP] = false;
        }
        
        /// <summary>
        /// Closing the popup on abort event
        /// </summary>
        private void OnAbortButton()
        {
            GameManager.PopupsShown[GameManager.LAND_TRADE_POPUP] = false;
        }

        /// <summary>
        /// Packs offer content to dictionary (only resources)
        /// </summary>
        /// <returns>Offer content in dictionary</returns>
        private  Dictionary<Resources.ResourceType, int>[] GetOfferContent()
        {
            //Destiny: Packing offer content to two dictionaries (index: 0 - to give, 1 - to take)
            var offerContent = new Dictionary<Resources.ResourceType, int>[2];
            for (var i = 0; i < 2; i++)
            {
                offerContent[i] = new Dictionary<Resources.ResourceType, int>
                {
                    { Resources.ResourceType.Clay, clayValue[i] },
                    { Resources.ResourceType.Iron, ironValue[i] },
                    { Resources.ResourceType.Wheat, wheatValue[i] },
                    { Resources.ResourceType.Wood, woodValue[i] },
                    { Resources.ResourceType.Wool, woolValue[i] }
                };
            }
            
            return offerContent;
        }
    }
}
