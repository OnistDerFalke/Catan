using System;
using System.Collections.Generic;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    public class PlayerThiefPayRequest
    {
        public int loanValue;
        public int playerIndex;
        public Dictionary<Resources.ResourceType, int> resourceLimits;

        public PlayerThiefPayRequest(int loan, int index, Dictionary<Resources.ResourceType, int> limit)
        {
            loanValue = loan;
            playerIndex = index;
            resourceLimits = limit;
        }
    }

    public class ThiefPayPopupController : MonoBehaviour
    {
        [Header("Header Text")][Space(5)]
        [Tooltip("Header Text")]
        [SerializeField] private Text headerText;
        
        [Header("Values Texts")][Space(5)]
        [Tooltip("Clay Value")]
        [SerializeField] private Text clayValueText;
        [Tooltip("Iron Value")]
        [SerializeField] private Text ironValueText;
        [Tooltip("Wheat Value")]
        [SerializeField] private Text wheatValueText;
        [Tooltip("Wood Value")]
        [SerializeField] private Text woodValueText;
        [Tooltip("Wool Value")]
        [SerializeField] private Text woolValueText;
        
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
        
        [Header("Add Buttons")][Space(5)]
        [Tooltip("Clay Add")]
        [SerializeField] private Button clayAdd;
        [Tooltip("Iron Add")]
        [SerializeField] private Button ironAdd;
        [Tooltip("Wheat Add")]
        [SerializeField] private Button wheatAdd;
        [Tooltip("Wood Add")]
        [SerializeField] private Button woodAdd;
        [Tooltip("Wool Add")]
        [SerializeField] private Button woolAdd;
        
        [Header("Remove Buttons")][Space(5)]
        [Tooltip("Clay Remove")]
        [SerializeField] private Button clayRemove;
        [Tooltip("Iron Remove")]
        [SerializeField] private Button ironRemove;
        [Tooltip("Wheat Remove")]
        [SerializeField] private Button wheatRemove;
        [Tooltip("Wood Remove")]
        [SerializeField] private Button woodRemove;
        [Tooltip("Wool Remove")]
        [SerializeField] private Button woolRemove;
        
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")]
        [SerializeField] private Button confirmButton;

        private int clayValue, ironValue, wheatValue, woodValue, woolValue;
        private int numberChosen;
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
            headerText.text = $"{GameManager.Players[currentRequestHandled.playerIndex].name} " +
                              $"wybiera surowce do oddania: {numberChosen}/{currentRequestHandled.loanValue}";
            
            UpdateValuesTexts();
            UpdateAvailabilityTexts();
            BlockIfLimit();
            confirmButton.enabled = numberChosen >= currentRequestHandled.loanValue;
            if (numberChosen <= 0)
            {
                ManageRemovesBlock(false);
            }
            else if (numberChosen >= currentRequestHandled.loanValue)
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
                clayAvailabilityText.text = $"Dostępnych: " +
                                            $"{GameManager.Players[currentRequestHandled.playerIndex].resources.GetResourceNumber(Resources.ResourceType.Clay) - clayValue}";
                ironAvailabilityText.text = $"Dostępnych: " +
                                            $"{GameManager.Players[currentRequestHandled.playerIndex].resources.GetResourceNumber(Resources.ResourceType.Iron) - ironValue}";
                wheatAvailabilityText.text = $"Dostępnych: " +
                                            $"{GameManager.Players[currentRequestHandled.playerIndex].resources.GetResourceNumber(Resources.ResourceType.Wheat) - wheatValue}";
                woodAvailabilityText.text = $"Dostępnych: " +
                                            $"{GameManager.Players[currentRequestHandled.playerIndex].resources.GetResourceNumber(Resources.ResourceType.Wood) - woodValue}";
                woolAvailabilityText.text = $"Dostępnych: " +
                                            $"{GameManager.Players[currentRequestHandled.playerIndex].resources.GetResourceNumber(Resources.ResourceType.Wool) - woolValue}";
            }
        }

        private void BlockIfZero()
        {
            clayRemove.gameObject.SetActive(clayValue > 0); 
            ironRemove.gameObject.SetActive(ironValue > 0);
            wheatRemove.gameObject.SetActive(wheatValue > 0);
            woodRemove.gameObject.SetActive(woodValue > 0);
            woolRemove.gameObject.SetActive(woolValue > 0);
        }

        private void BlockIfLimit()
        {
            clayAdd.gameObject.SetActive(clayValue < currentRequestHandled.resourceLimits[Resources.ResourceType.Clay]);
            ironAdd.gameObject.SetActive(ironValue < currentRequestHandled.resourceLimits[Resources.ResourceType.Iron]);
            wheatAdd.gameObject.SetActive(wheatValue < currentRequestHandled.resourceLimits[Resources.ResourceType.Wheat]);
            woodAdd.gameObject.SetActive(woodValue < currentRequestHandled.resourceLimits[Resources.ResourceType.Wood]);
            woolAdd.gameObject.SetActive(woolValue < currentRequestHandled.resourceLimits[Resources.ResourceType.Wool]);
        }

        private void OnConfirmButton()
        {
            if (numberChosen == currentRequestHandled.loanValue)
            {
                confirmButton.enabled = false;               

                resourcesToReturn.Add(Resources.ResourceType.Wood, woodValue);
                resourcesToReturn.Add(Resources.ResourceType.Clay, clayValue);
                resourcesToReturn.Add(Resources.ResourceType.Wheat, wheatValue);
                resourcesToReturn.Add(Resources.ResourceType.Wool, woolValue);
                resourcesToReturn.Add(Resources.ResourceType.Iron, ironValue);
                
                //Destiny: subtraction of selected resources
                GameManager.Players[currentRequestHandled.playerIndex].resources.SubtractResources(resourcesToReturn);

                MoveToNextPlayer();
            }
        }

        private void MoveToNextPlayer()
        {
            resourcesToReturn = new Dictionary<Resources.ResourceType, int>();
            confirmButton.enabled = false;
            ClearValues();
            
            if (playerThiefPayRequests.Count > 0)
            {
                currentRequestHandled = playerThiefPayRequests[0];
                playerThiefPayRequests.RemoveAt(0);
            }
            else
            {
                GameManager.MovingUserMode = GameManager.MovingMode.MovingThief;
                GameManager.ThiefPayPopupShown = false;
            }
        }
        
        private void PlayersLoop()
        {
            playerThiefPayRequests = new List<PlayerThiefPayRequest>();
            //Destiny: if player has more than 7 cards have to give them back
            foreach (var player in GameManager.Players)
            {
                if (player.resources.GetResourceNumber() > GameManager.MaxResourceNumberWhenTheft)
                {
                    //Destiny: Setting up important info about player limits and loan
                    resourcesToReturn = new Dictionary<Resources.ResourceType, int>();
                    var loanValue = (int)Math.Floor(player.resources.GetResourceNumber() / 2.0);
                    var resourceLimits = player.resources.GetResourcesNumber();
                    var playerIndex = player.index;
                    playerThiefPayRequests.Add(new PlayerThiefPayRequest(loanValue, playerIndex, resourceLimits));
                }
            }
            MoveToNextPlayer();
        }
    }
}
