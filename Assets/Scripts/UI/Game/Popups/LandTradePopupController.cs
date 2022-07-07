using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

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
        
        [Header("Availability Texts")][Space(5)]
        [Tooltip("Clay Availability")]
        [SerializeField] private Text[] clayAvailabilityText;
        [Tooltip("Iron Availability")]
        [SerializeField] private Text[] ironAvailabilityText;
        [Tooltip("Wheat Availability")]
        [SerializeField] private Text[] wheatAvailabilityText;
        [Tooltip("Wood Availability")]
        [SerializeField] private Text[] woodAvailabilityText;
        [Tooltip("Wool Availability")]
        [SerializeField] private Text[] woolAvailabilityText;
        
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
        
        private int chosenPlayer;
        
        private int[] clayValue, ironValue, wheatValue, woodValue, woolValue;

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
            offerButton.interactable = false;
            ClearValues();
            SetPlayersInfo();
        }

        void Update()
        {
            //Destiny: Zooms button if has been chosen
            for (var i = 0; i < playersButtons.Length; i++)
                playersButtons[i].gameObject.transform.localScale = i == chosenPlayer ?
                    new Vector3(1.2f, 1.2f, 1.2f) : Vector3.one;
            
            UpdateValuesTexts();
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
            
            foreach (var player in GameManager.Players)
            {
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
        /// Closing the popup and sending an offer
        /// </summary>
        private void OnOfferButton()
        {
            
        }
        
        /// <summary>
        /// Closing the popup on abort event
        /// </summary>
        private void OnAbortButton()
        {
            GameManager.LandTradePopupShown = false;
        }
    }
}
