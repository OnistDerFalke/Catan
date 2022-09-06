using System.Collections.Generic;
using System.Linq;
using Board;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    public class SeaTradePopupController : MonoBehaviour
    {
        //Destiny: Resources values chosen in popup (index: 0 - to give, 1 - to take)
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

        //Destiny: Buttons for adding resources to offer (index: 0 - to give, 1 - to take)
        [Header("Add Buttons")][Space(5)]
        [Tooltip("Clay Add")][SerializeField]
        private Button[] clayAdd;
        [Tooltip("Iron Add")][SerializeField] 
        private Button[] ironAdd;
        [Tooltip("Wheat Add")][SerializeField] 
        private Button[] wheatAdd;
        [Tooltip("Wood Add")][SerializeField]
        private Button[] woodAdd;
        [Tooltip("Wool Add")][SerializeField]
        private Button[] woolAdd;

        //Destiny: Buttons for removing resources from offer (index: 0 - to give, 1 - to take)
        [Header("Remove Buttons")][Space(5)] 
        [Tooltip("Clay Remove")][SerializeField]
        private Button[] clayRemove;
        [Tooltip("Iron Remove")][SerializeField] 
        private Button[] ironRemove;
        [Tooltip("Wheat Remove")][SerializeField]
        private Button[] wheatRemove;
        [Tooltip("Wood Remove")][SerializeField]
        private Button[] woodRemove;
        [Tooltip("Wool Remove")][SerializeField] 
        private Button[] woolRemove;
        
        //Destiny: Icons of special ports
        [Header("Icons Of Special Ports")][Space(5)] 
        [Tooltip("Clay Special Icon")][SerializeField]
        private Image claySpecialIcon;
        [Tooltip("Iron Special Icon")][SerializeField]
        private Image ironSpecialIcon;
        [Tooltip("Wheat Special Icon")][SerializeField] 
        private Image wheatSpecialIcon;
        [Tooltip("Wood Special Icon")][SerializeField]
        private Image woodSpecialIcon;
        [Tooltip("Wool Special Icon")][SerializeField]
        private Image woolSpecialIcon;

        //Destiny: Buttons for exchanging or aborting transaction
        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Exchange Button")][SerializeField] 
        private Button exchangeButton;
        [Tooltip("Abort Button")][SerializeField]
        private Button abortButton;

        //Destiny: Texts and other values
        [Header("Other Values Texts")][Space(5)]
        [Tooltip("Standard Exchange Proportion")][SerializeField] 
        private Text standardExchangeProportion;
        [Tooltip("No Port Proportion")][SerializeField]
        private string noPortProportion;
        [Tooltip("Has Port Proportion")][SerializeField]
        private string hasPortProportion;
        [Tooltip("Overflow Resources")][SerializeField] 
        private Text overflowResources;
        [Tooltip("Due Resources")][SerializeField]
        private Text dueResources;
        
        //Destiny: Specific Colors
        [Header("Specific Colors")][Space(5)]
        [Tooltip("Green")][SerializeField] 
        private Color greenColor;
        [Tooltip("Gray")][SerializeField]
        private Color grayColor;
        
        //Destiny: Values of offer resources (index: 0 - to give, 1 - to take)
        private int[] clayValue, ironValue, wheatValue, woodValue, woolValue;
        
        void Start()
        {
            //Destiny: Adding click listeners to flow control buttons
            exchangeButton.onClick.AddListener(OnExchangeButton);
            abortButton.onClick.AddListener(OnAbortButton);
            
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
            //Destiny: Clear values on start
            ClearValues();
            
            //Destiny: Update all elements in popup
            SetStandardExchangeValue();
            SetSpecialPortsIcons();
        }

        void Update()
        {
            //Destiny: Block exchange button if cannot be clicked
            exchangeButton.interactable = CheckIfCanPass();

            //Destiny: Updates popup values
            UpdateValuesTexts();
            UpdateSpecialValues();
            
            //Destiny: Blocking + and - buttons if more or less of any resource cannot be chosen
            BlockIfLimit();
            BlockIfZero();
        }

        /// <summary>
        /// Event on clicking exchange button
        /// </summary>
        private void OnExchangeButton()
        {
            //Destiny: Exchange of resources between players
            var resources = PackResourcesDictionary();
            GameManager.TradeManager.ExchangeResourcesOnePlayer(GameManager.State.CurrentPlayerId, resources[0], resources[1]);

            //Destiny: Hiding the popup after clicking the button
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.SEA_TRADE_POPUP] = false;
        }

        /// <summary>
        /// Event on clicking abort button
        /// </summary>
        private void OnAbortButton()
        {
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.SEA_TRADE_POPUP] = false;
        }

        /// <summary>
        /// Sets standard exchange proportion in popup
        /// </summary>
        private void SetStandardExchangeValue()
        {
            standardExchangeProportion.text = GameManager.State.Players[GameManager.State.CurrentPlayerId].ports
                .GetPortKeyPair(JunctionElement.PortType.Normal) ? hasPortProportion : noPortProportion;
        }

        /// <summary>
        /// Sets colors of premium icons (green if premium port active, gray if it is not)
        /// </summary>
        private void SetSpecialPortsIcons()
        {
            var specialPortsInfo = GameManager.State.Players[GameManager.State.CurrentPlayerId].ports.GetSpecialPortsInfo();

            woolSpecialIcon.color = specialPortsInfo[JunctionElement.PortType.Wool] ? greenColor : grayColor;
            woodSpecialIcon.color = specialPortsInfo[JunctionElement.PortType.Wood] ? greenColor : grayColor;
            ironSpecialIcon.color = specialPortsInfo[JunctionElement.PortType.Iron] ? greenColor : grayColor;
            claySpecialIcon.color = specialPortsInfo[JunctionElement.PortType.Clay] ? greenColor : grayColor;
            wheatSpecialIcon.color = specialPortsInfo[JunctionElement.PortType.Wheat] ? greenColor : grayColor;
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
            var resources = GameManager.State.Players[GameManager.State.CurrentPlayerId].resources.GetResourcesNumber();

            //Destiny: Player cannot offer more resources than he has
            clayAdd[0].gameObject.SetActive(clayValue[0] < resources[Resources.ResourceType.Clay]);
            ironAdd[0].gameObject.SetActive(ironValue[0] < resources[Resources.ResourceType.Iron]);
            wheatAdd[0].gameObject.SetActive(wheatValue[0] < resources[Resources.ResourceType.Wheat]);
            woodAdd[0].gameObject.SetActive(woodValue[0] < resources[Resources.ResourceType.Wood]);
            woolAdd[0].gameObject.SetActive(woolValue[0] < resources[Resources.ResourceType.Wool]);
            
            //Destiny: Player can ask only max resources that can be asked
            clayAdd[1].gameObject.SetActive(clayValue[1] < GameManager.ResourceManager.MaxResourcesNumber);
            ironAdd[1].gameObject.SetActive(ironValue[1] < GameManager.ResourceManager.MaxResourcesNumber);
            wheatAdd[1].gameObject.SetActive(wheatValue[1] < GameManager.ResourceManager.MaxResourcesNumber);
            woodAdd[1].gameObject.SetActive(woodValue[1] < GameManager.ResourceManager.MaxResourcesNumber);
            woolAdd[1].gameObject.SetActive(woolValue[1] < GameManager.ResourceManager.MaxResourcesNumber);
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
        /// Updates information values of overflow and due resources
        /// </summary>
        private void UpdateSpecialValues()
        {
            var countValues = GameManager.TradeManager.CountTradeResources(PackResourcesDictionary()[0]);
            overflowResources.text = countValues[GameManager.TradeManager.ADDITIONAL_RESOURCES].ToString();
            dueResources.text = countValues[GameManager.TradeManager.RESOURCES_TO_BOUGHT_STRING].ToString();
        }

        /// <summary>
        /// Packs chosen resources values to dictionaries
        /// </summary>
        /// <returns>Dictionary array of resources: 0 - to give, 1 - to get</returns>
        private Dictionary<Resources.ResourceType, int>[] PackResourcesDictionary()
        {
            var resources = new Dictionary<Resources.ResourceType, int>[2];
            for (var i = 0; i < 2; i++)
            {
                resources[i] = new Dictionary<Resources.ResourceType, int>
                {
                    { Resources.ResourceType.Wood, woodValue[i] },
                    { Resources.ResourceType.Clay, clayValue[i] },
                    { Resources.ResourceType.Wheat, wheatValue[i] },
                    { Resources.ResourceType.Wool, woolValue[i] },
                    { Resources.ResourceType.Iron, ironValue[i] }
                };
            }

            return resources;
        }

        /// <summary>
        /// Checks if transaction can be accepted
        /// </summary>
        /// <returns>If transaction can be accepted</returns>
        private bool CheckIfCanPass()
        {
            var countValues = GameManager.TradeManager.CountTradeResources(PackResourcesDictionary()[0]);

            //Destiny: Overflow value needs to be 0
            if (countValues[GameManager.TradeManager.ADDITIONAL_RESOURCES] != 0)
            {
                return false;
            }
            
            //Destiny: Chosen elements to get need to be equal to value that can be taken
            if (PackResourcesDictionary()[1].Sum(element => element.Value) !=
                countValues[GameManager.TradeManager.RESOURCES_TO_BOUGHT_STRING])
            {
                return false;
            }
            
            //Destiny: Transaction cannot be empty (nothing for nothing)
            if (PackResourcesDictionary()[0].Sum(element => element.Value) == 0 
                && PackResourcesDictionary()[1].Sum(element => element.Value) == 0)
            {
                return false;
            }
            
            return true;
        }
    }
}
