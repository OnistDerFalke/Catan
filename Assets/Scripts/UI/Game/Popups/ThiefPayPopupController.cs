using System;
using System.Collections.Generic;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    //Destiny: Template class for thief pay requests
    public class PlayerThiefPayRequest
    {
        //Destiny: How much player need to pay to thief
        public readonly int LoanValue;
        
        //Destiny: Index of the player
        public readonly int PlayerIndex;
        
        //Destiny: Limits of resources that player can pay
        public readonly Dictionary<Resources.ResourceType, int> ResourceLimits;

        public PlayerThiefPayRequest(int loan, int index, Dictionary<Resources.ResourceType, int> limit)
        {
            LoanValue = loan;
            PlayerIndex = index;
            ResourceLimits = limit;
        }
    }

    public class ThiefPayPopupController : MonoBehaviour
    {
        //Destiny: Text of the subheader
        [Header("Subheader Text")][Space(5)]
        [Tooltip("Subheader Text")][SerializeField] private Text subheaderText;
        
        //Destiny: Values under resources
        [Header("Values Texts")][Space(5)]
        [Tooltip("Clay Value")] [SerializeField] private Text clayValueText;
        [Tooltip("Iron Value")] [SerializeField] private Text ironValueText;
        [Tooltip("Wheat Value")] [SerializeField] private Text wheatValueText;
        [Tooltip("Wood Value")] [SerializeField] private Text woodValueText;
        [Tooltip("Wool Value")] [SerializeField] private Text woolValueText;
        
        //Destiny: Availability texts
        [Header("Availability Texts")][Space(5)]
        [Tooltip("Clay Availability")] [SerializeField] private Text clayAvailabilityText;
        [Tooltip("Iron Availability")] [SerializeField] private Text ironAvailabilityText;
        [Tooltip("Wheat Availability")] [SerializeField] private Text wheatAvailabilityText;
        [Tooltip("Wood Availability")] [SerializeField] private Text woodAvailabilityText;
        [Tooltip("Wool Availability")] [SerializeField] private Text woolAvailabilityText;
        
        //Destiny: Add resource buttons
        [Header("Add Buttons")][Space(5)]
        [Tooltip("Clay Add")] [SerializeField] private Button clayAdd;
        [Tooltip("Iron Add")] [SerializeField] private Button ironAdd;
        [Tooltip("Wheat Add")] [SerializeField] private Button wheatAdd;
        [Tooltip("Wood Add")] [SerializeField] private Button woodAdd;
        [Tooltip("Wool Add")] [SerializeField] private Button woolAdd;
        
        //Destiny: Remove resource button
        [Header("Remove Buttons")][Space(5)]
        [Tooltip("Clay Remove")] [SerializeField] private Button clayRemove;
        [Tooltip("Iron Remove")] [SerializeField] private Button ironRemove;
        [Tooltip("Wheat Remove")] [SerializeField] private Button wheatRemove;
        [Tooltip("Wood Remove")] [SerializeField] private Button woodRemove;
        [Tooltip("Wool Remove")] [SerializeField] private Button woolRemove;
        
        //Destiny: Confirm button
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")] [SerializeField] private Button confirmButton;
        
        //Destiny: Static texts
        [Header("Static Texts")][Space(5)]
        [Tooltip("Availability static text")] [SerializeField] private string availabilityStaticText;
        [Tooltip("Subheader static text")] [SerializeField] private string subheaderStaticText;
        
        //Destiny: Values of resources chosen
        private int clayValue, ironValue, wheatValue, woodValue, woolValue;
        private int numberChosen;
        
        //Destiny: Fields connected with requests
        private Dictionary<Resources.ResourceType, int> resourcesToReturn;
        private List<PlayerThiefPayRequest> playerThiefPayRequests;
        private PlayerThiefPayRequest currentRequestHandled;

        void Start()
        {
            //Destiny: Usage of + - buttons
            clayAdd.onClick.AddListener(() =>
            {
                clayValue++;
                numberChosen++;
            });
            ironAdd.onClick.AddListener(() =>
            {
                ironValue++;
                numberChosen++;
            });
            wheatAdd.onClick.AddListener(() =>
            {
                wheatValue++;
                numberChosen++;
            });
            woodAdd.onClick.AddListener(() =>
            {
                woodValue++;
                numberChosen++;
            });
            woolAdd.onClick.AddListener(() =>
            {
                woolValue++;
                numberChosen++;
            });
            
            clayRemove.onClick.AddListener(() =>
            {
                clayValue--;
                numberChosen--;
            });
            ironRemove.onClick.AddListener(() =>
            {
                ironValue--;
                numberChosen--;
            });
            wheatRemove.onClick.AddListener(() =>
            {
                wheatValue--;
                numberChosen--;
            });
            woodRemove.onClick.AddListener(() =>
            {
                woodValue--;
                numberChosen--;
            });
            woolRemove.onClick.AddListener(() =>
            {
                woolValue--;
                numberChosen--;
            });
            
            //Destiny: Usage of confirm button
            confirmButton.onClick.AddListener(OnConfirmButton);
        }

        void OnEnable()
        {
            PlayersLoop();
            UpdateValuesTexts();
            UpdateAvailabilityTexts();
        }

        void Update()
        {
            subheaderText.text = $"{GameManager.Players[currentRequestHandled.PlayerIndex].name} " +
                              subheaderStaticText +  $" {numberChosen}/{currentRequestHandled.LoanValue}";
            
            UpdateValuesTexts();
            UpdateAvailabilityTexts();
            BlockIfLimit();
            confirmButton.enabled = numberChosen >= currentRequestHandled.LoanValue;
            if (numberChosen <= 0)
            {
                ManageRemovesBlock(false);
            }
            else if (numberChosen >= currentRequestHandled.LoanValue)
            {
                ManageRemovesBlock(true);
                ManageAddsBlock(false);
            }
            else
            {
                ManageRemovesBlock(true);
            }
            BlockIfZero();
        }

        /// <summary>
        /// Clears all chosen values
        /// </summary>
        private void ClearValues()
        {
            clayValue = 0;
            ironValue = 0;
            wheatValue = 0;
            woodValue = 0;
            woolValue = 0;
            numberChosen = 0;
        }

        /// <summary>
        /// Blocks/unlocks buttons of adding
        /// </summary>
        /// <param name="unlocked">Do unlock</param>
        private void ManageAddsBlock(bool unlocked)
        {
            clayAdd.gameObject.SetActive(unlocked);
            ironAdd.gameObject.SetActive(unlocked);
            wheatAdd.gameObject.SetActive(unlocked);
            woodAdd.gameObject.SetActive(unlocked);
            woolAdd.gameObject.SetActive(unlocked);
        }
        
        /// <summary>
        /// Blocks/unlocks buttons of removing
        /// </summary>
        /// <param name="unlocked">Do unlock</param>
        private void ManageRemovesBlock(bool unlocked)
        {
            clayRemove.gameObject.SetActive(unlocked);
            ironRemove.gameObject.SetActive(unlocked);
            wheatRemove.gameObject.SetActive(unlocked);
            woodRemove.gameObject.SetActive(unlocked);
            woolRemove.gameObject.SetActive(unlocked);
        }

        /// <summary>
        /// Updates values in popup
        /// </summary>
        private void UpdateValuesTexts()
        {
            clayValueText.text = clayValue.ToString();
            ironValueText.text = ironValue.ToString();
            wheatValueText.text = wheatValue.ToString();
            woodValueText.text = woodValue.ToString();
            woolValueText.text = woolValue.ToString();
        }
        
        private void UpdateAvailabilityTexts()
        {
            if (currentRequestHandled != null)
            {
                clayAvailabilityText.text = availabilityStaticText +
                                            $"{GameManager.Players[currentRequestHandled.PlayerIndex].resources.GetResourceNumber(Resources.ResourceType.Clay) - clayValue}";
                ironAvailabilityText.text = availabilityStaticText +
                                            $"{GameManager.Players[currentRequestHandled.PlayerIndex].resources.GetResourceNumber(Resources.ResourceType.Iron) - ironValue}";
                wheatAvailabilityText.text = availabilityStaticText +
                                             $"{GameManager.Players[currentRequestHandled.PlayerIndex].resources.GetResourceNumber(Resources.ResourceType.Wheat) - wheatValue}";
                woodAvailabilityText.text = availabilityStaticText +
                                            $"{GameManager.Players[currentRequestHandled.PlayerIndex].resources.GetResourceNumber(Resources.ResourceType.Wood) - woodValue}";
                woolAvailabilityText.text = availabilityStaticText +
                                            $"{GameManager.Players[currentRequestHandled.PlayerIndex].resources.GetResourceNumber(Resources.ResourceType.Wool) - woolValue}";
            }
        }

        /// <summary>
        /// Blocks remove buttons if value reached 0
        /// </summary>
        private void BlockIfZero()
        {
            clayRemove.gameObject.SetActive(clayValue > 0); 
            ironRemove.gameObject.SetActive(ironValue > 0);
            wheatRemove.gameObject.SetActive(wheatValue > 0);
            woodRemove.gameObject.SetActive(woodValue > 0);
            woolRemove.gameObject.SetActive(woolValue > 0);
        }

        /// <summary>
        /// Blocks adding buttons if value reached max value
        /// </summary>
        private void BlockIfLimit()
        {
            clayAdd.gameObject.SetActive(clayValue < currentRequestHandled.ResourceLimits[Resources.ResourceType.Clay]);
            ironAdd.gameObject.SetActive(ironValue < currentRequestHandled.ResourceLimits[Resources.ResourceType.Iron]);
            wheatAdd.gameObject.SetActive(wheatValue < currentRequestHandled.ResourceLimits[Resources.ResourceType.Wheat]);
            woodAdd.gameObject.SetActive(woodValue < currentRequestHandled.ResourceLimits[Resources.ResourceType.Wood]);
            woolAdd.gameObject.SetActive(woolValue < currentRequestHandled.ResourceLimits[Resources.ResourceType.Wool]);
        }

        /// <summary>
        /// Event that happens on clicking confirm button
        /// </summary>
        private void OnConfirmButton()
        {
            if (numberChosen == currentRequestHandled.LoanValue)
            {
                confirmButton.enabled = false;               

                //Destiny: Setting resources to return
                resourcesToReturn.Add(Resources.ResourceType.Wood, woodValue);
                resourcesToReturn.Add(Resources.ResourceType.Clay, clayValue);
                resourcesToReturn.Add(Resources.ResourceType.Wheat, wheatValue);
                resourcesToReturn.Add(Resources.ResourceType.Wool, woolValue);
                resourcesToReturn.Add(Resources.ResourceType.Iron, ironValue);
                
                //Destiny: Subtraction of selected resources
                GameManager.Players[currentRequestHandled.PlayerIndex].resources.SubtractResources(resourcesToReturn);

                //Destiny: Switching popup to next player
                MoveToNextPlayer();
            }
        }

        /// <summary>
        /// Popup new values for next player on request list
        /// </summary>
        private void MoveToNextPlayer()
        {
            resourcesToReturn = new Dictionary<Resources.ResourceType, int>();
            confirmButton.enabled = false;
            ClearValues();
            
            //Destiny: Take requests if there are any - otherwise hide popup
            if (playerThiefPayRequests.Count > 0)
            {
                //Destiny: Takes next request from list
                currentRequestHandled = playerThiefPayRequests[0];
                playerThiefPayRequests.RemoveAt(0);
            }
            else
            {
                //Destiny: Hide popup and force moving thief mode
                GameManager.MovingUserMode = GameManager.MovingMode.MovingThief;
                GameManager.PopupsShown[GameManager.THIEF_PAY_POPUP] = false;
            }
        }
        
        /// <summary>
        /// Creates new thief pay requests and adds it to the list
        /// </summary>
        private void PlayersLoop()
        {
            playerThiefPayRequests = new List<PlayerThiefPayRequest>();
            
            //Destiny: if player has more than 7 cards - he have to give them back
            foreach (var player in GameManager.Players)
            {
                if (player.resources.GetResourceNumber() > GameManager.MaxResourceNumberWhenTheft)
                {
                    //Destiny: Setting up important info about player limits and loan
                    resourcesToReturn = new Dictionary<Resources.ResourceType, int>();
                    var loanValue = (int)Math.Floor(player.resources.GetResourceNumber() / 2.0);
                    var resourceLimits = player.resources.GetResourcesNumber();
                    var playerIndex = player.index;
                    
                    //Destiny: Add request to requests list
                    playerThiefPayRequests.Add(new PlayerThiefPayRequest(loanValue, playerIndex, resourceLimits));
                }
            }
            MoveToNextPlayer();
        }
    }
}
