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
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Offer Button")]
        [SerializeField] private Button offerButton;
        [Tooltip("Abort Button")]
        [SerializeField] private Button abortButton;
        
        [Tooltip("Players Buttons")]
        [SerializeField] private Button[] playersButtons = new Button[4];
        
        [Tooltip("Players Names")]
        [SerializeField] private Text[] playersNames = new Text[4];
        
        [Tooltip("Players Colors")]
        [SerializeField] private Image[] playersColors = new Image[4];

        //Destiny: Resources choice popup elements (index: 0 - to give, 1 - to take)
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

        [Header("Add Buttons")][Space(5)]
        [Tooltip("Clay Add")]
        [SerializeField] private Button[] clayAdd;
        [Tooltip("Iron Add")]
        [SerializeField] private Button[] ironAdd;
        [Tooltip("Wheat Add")]
        [SerializeField] private Button[] wheatAdd;
        [Tooltip("Wood Add")]
        [SerializeField] private Button[] woodAdd;
        [Tooltip("Wool Add")]
        [SerializeField] private Button[] woolAdd;
        
        [Header("Remove Buttons")][Space(5)]
        [Tooltip("Clay Remove")]
        [SerializeField] private Button[] clayRemove;
        [Tooltip("Iron Remove")]
        [SerializeField] private Button[] ironRemove;
        [Tooltip("Wheat Remove")]
        [SerializeField] private Button[] wheatRemove;
        [Tooltip("Wood Remove")]
        [SerializeField] private Button[] woodRemove;
        [Tooltip("Wool Remove")]
        [SerializeField] private Button[] woolRemove;
        
        [Header("Availability Texts")][Space(5)]
        [Tooltip("Clay Availability")]
        [SerializeField] private Text clayAvailabilityText;
        [Tooltip("Iron Availability")]
        [SerializeField] private Text ironAvailabilityText;
        [Tooltip("Wheat Availability")]
        [SerializeField] private Text wheatAvailabilityText;
        [Tooltip("Wood Availability")]
        [SerializeField] private Text woodAvailabilityText;
        [Tooltip("Wool Availability")]
        [SerializeField] private Text woolAvailabilityText;
        
        private int chosenPlayer;
        
        private int[] clayValue, ironValue, wheatValue, woodValue, woolValue;

        private float[] verticalPlacement;

        void Start()
        {
            //Destiny: Chosen player is unset on start
            chosenPlayer = -1;
            
            offerButton.onClick.AddListener(OnOfferButton);
            abortButton.onClick.AddListener(OnAbortButton);

            for (var i = 0; i < playersButtons.Length; i++)
            {
                var index = i;
                playersButtons[i].onClick.AddListener(() => { chosenPlayer = index;});
            }

            for (var i = 0; i < 2; i++)
            {
                //Destiny: Usage of + - buttons
                var type = i;
                clayAdd[i].onClick.AddListener(() =>
                {
                    clayValue[type]++;
                });
                ironAdd[i].onClick.AddListener(() =>
                {
                    ironValue[type]++;
                });
                wheatAdd[i].onClick.AddListener(() =>
                {
                    wheatValue[type]++;
                });
                woodAdd[i].onClick.AddListener(() =>
                {
                    woodValue[type]++;
                });
                woolAdd[i].onClick.AddListener(() =>
                {
                    woolValue[type]++;
                });
            
                clayRemove[i].onClick.AddListener(() =>
                {
                    clayValue[type]--;
                });
                ironRemove[i].onClick.AddListener(() =>
                {
                    ironValue[type]--;
                });
                wheatRemove[i].onClick.AddListener(() =>
                {
                    wheatValue[type]--;
                });
                woodRemove[i].onClick.AddListener(() =>
                {
                    woodValue[type]--;
                });
                woolRemove[i].onClick.AddListener(() =>
                {
                    woolValue[type]--;
                });
            }
        }

        void OnEnable()
        { 
            //Destiny: Get start button positions and set placements
            if (verticalPlacement == null)
            {
                verticalPlacement = new float[playersButtons.Length];
                for (var i = 0; i < playersButtons.Length; i++)
                    verticalPlacement[i] = playersButtons[i].gameObject.transform.localPosition.y;
            }

            offerButton.interactable = false;
            ClearValues();
            chosenPlayer = -1;
            SetPlayersInfo();
            UpdateAvailabilityTexts();
        }

        void Update()
        {
            //Destiny: Zooms button if has been chosen
            for (var i = 0; i < playersButtons.Length; i++)
                playersButtons[i].gameObject.transform.localScale = i == chosenPlayer ?
                    new Vector3(1.2f, 1.2f, 1.2f) : Vector3.one;
            
            UpdateValuesTexts();
            BlockIfLimit();
            BlockIfZero();
            
            //Destiny: If transaction is legal and player chosen, offer can be sent
            offerButton.interactable = CheckIfNotDonation() && chosenPlayer >= 0;
        }
        
        /// <summary>
        /// Clears all chosen values
        /// </summary>
        private void ClearValues()
        {
            clayValue = new int[2];
            ironValue = new int[2];
            wheatValue = new int[2];
            woodValue = new int[2];
            woolValue = new int[2];
        }
        
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

        private void BlockIfLimit()
        {
            clayAdd[0].gameObject.SetActive(clayValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Player.Resources.ResourceType.Clay));
            ironAdd[0].gameObject.SetActive(ironValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Player.Resources.ResourceType.Iron));
            wheatAdd[0].gameObject.SetActive(wheatValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Player.Resources.ResourceType.Wheat));
            woodAdd[0].gameObject.SetActive(woodValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Player.Resources.ResourceType.Wood));
            woolAdd[0].gameObject.SetActive(woolValue[0] < GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Player.Resources.ResourceType.Wool));
            
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
            {
                button.gameObject.SetActive(false);
            }

            var placeInVerticalGrid = 0;
            foreach (var player in GameManager.Players)
            {
                //Destiny: Player cannot choose himself
                if (player.index == GameManager.Players[GameManager.CurrentPlayer].index)
                    continue;
                
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
        
        private void UpdateAvailabilityTexts()
        {
            clayAvailabilityText.text = $"Dostępnych: " + $"{GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Clay)}";
            ironAvailabilityText.text = $"Dostępnych: " + $"{GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Iron)}";
            wheatAvailabilityText.text = $"Dostępnych: " + $"{GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wheat)}";
            woodAvailabilityText.text = $"Dostępnych: " + $"{GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wood)}";
            woolAvailabilityText.text = $"Dostępnych: " + $"{GameManager.Players[GameManager.CurrentPlayer].resources.GetResourceNumber(Resources.ResourceType.Wool)}";
        }

        /// <summary>
        /// Closing the popup and sending an offer
        /// </summary>
        private void OnOfferButton()
        {
            offerButton.interactable = false;
            GameManager.LandTradeOfferContent = GetOfferContent();
            GameManager.LandTradeOfferTarget = chosenPlayer;
            GameManager.PopupsShown[GameManager.LAND_TRADE_ACCEPT_POPUP] = true;
            GameManager.PopupsShown[GameManager.LAND_TRADE_POPUP] = false;

            //TODO: Offered resources will be passed somewhere and offered to another player
            //TODO: All chosen resources are in the arrays, probably it will be packed to dict or sth
        }

        /// <summary>
        /// Packs offer content to dictionary (only resources)
        /// </summary>
        /// <returns>Offer content in dictionary</returns>
        private  Dictionary<Resources.ResourceType, int>[] GetOfferContent()
        {
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
        
        /// <summary>
        /// Closing the popup on abort event
        /// </summary>
        private void OnAbortButton()
        {
            GameManager.PopupsShown[GameManager.LAND_TRADE_POPUP] = false;
        }
    }
}
