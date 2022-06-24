using DataStorage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player.Resources;

namespace UI.Game.Popups
{
    public class InventionPopupController : MonoBehaviour
    {
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

        private float clayValue, ironValue, wheatValue, woodValue, woolValue;
        private float numberChosen;

        void Start()
        {
            confirmButton.enabled = false;
            ClearValues();
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

        void Update()
        {
            UpdateValuesTexts();
            confirmButton.enabled = numberChosen >= 2;
            switch (numberChosen)
            {
                case <= 0:
                    ManageRemovesBlock(false);
                    ManageAddsBlock(true);
                    break;
                case >= 2:
                    ManageRemovesBlock(true);
                    ManageAddsBlock(false);
                    break;
                default:
                    ManageRemovesBlock(true);
                    ManageAddsBlock(true);
                    break;
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
            clayAdd.enabled = unlocked;
            ironAdd.enabled = unlocked;
            wheatAdd.enabled = unlocked;
            woodAdd.enabled = unlocked;
            woolAdd.enabled = unlocked;
        }
        
        /// <summary>
        /// Blocks/unlocks buttons of removing
        /// </summary>
        /// <param name="unlocked">Do unlock</param>
        private void ManageRemovesBlock(bool unlocked)
        {
            clayRemove.enabled = unlocked;
            ironRemove.enabled = unlocked;
            wheatRemove.enabled = unlocked;
            woodRemove.enabled = unlocked;
            woolRemove.enabled = unlocked;
        }

        private void UpdateValuesTexts()
        {
            clayValueText.text = clayValue.ToString();
            ironValueText.text = ironValue.ToString();
            wheatValueText.text = wheatValue.ToString();
            woodValueText.text = woodValue.ToString();
            woolValueText.text = woolValue.ToString();
        }

        private void BlockIfZero()
        {
            clayRemove.enabled = clayValue > 0; 
            ironRemove.enabled = ironValue > 0;
            wheatRemove.enabled = wheatValue > 0;
            woodRemove.enabled = woodValue > 0;
            woolRemove.enabled = woolValue > 0;
        }

        private void OnConfirmButton()
        {
            if (numberChosen == 2)
            {
                confirmButton.enabled = false;
                GameManager.InventionPopupShown = false;

                List<ResourceType> chosenResources = new();
                for (int i = 0; i < clayValue; i++)
                    chosenResources.Add(ResourceType.Clay);
                for (int i = 0; i < woodValue; i++)
                    chosenResources.Add(ResourceType.Wood);
                for (int i = 0; i < woolValue; i++)
                    chosenResources.Add(ResourceType.Wool);
                for (int i = 0; i < wheatValue; i++)
                    chosenResources.Add(ResourceType.Wheat);
                for (int i = 0; i < ironValue; i++)
                    chosenResources.Add(ResourceType.Iron);

                GameManager.Players[GameManager.CurrentPlayer].properties.cards.InventionCardEffect(chosenResources);
            }
        }
    }
}
