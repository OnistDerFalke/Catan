using Assets.Scripts.DataStorage.Managers;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Board.States.JunctionState;

namespace UI.Game
{
    public class BuildingsLimits : MonoBehaviour
    {
        [Header("Limits")][Space(5)]
        [Tooltip("Villages Text")][SerializeField]
        private Text villagesText;
        [Tooltip("Cities Text")][SerializeField]
        private Text citiesText;
        [Tooltip("Paths Text")][SerializeField] 
        private Text pathsText;   
        [Tooltip("Cards Text")][SerializeField] 
        private Text cardsText;
        
        [Header("Labels")][Space(5)]
        [Tooltip("Villages Label")][SerializeField]
        private Text villagesLabel;
        [Tooltip("Cities Text")][SerializeField]
        private Text citiesLabel;
        [Tooltip("Paths Text")][SerializeField] 
        private Text pathsLabel;   
        [Tooltip("Cards Text")][SerializeField] 
        private Text cardsLabel;
        
        [Header("Values")][Space(5)]
        [Tooltip("Villages Value")][SerializeField]
        private Text villagesValue;
        [Tooltip("Cities Value")][SerializeField]
        private Text citiesValue;
        [Tooltip("Paths Value")][SerializeField] 
        private Text pathsValue;   
        [Tooltip("Cards Value")][SerializeField] 
        private Text cardsValue;
        
        [Header("Resources Icons")][Space(5)]
        [Tooltip("Villages Resource Icons")][SerializeField]
        private Image[] villagesResources;
        [Tooltip("Cities Resource Icons")][SerializeField]
        private Image[] citiesResources;
        [Tooltip("Paths Resource Icons")][SerializeField] 
        private Image[] pathsResources;   
        [Tooltip("Cards Resource Icons")][SerializeField] 
        private Image[] cardsResources;

        [Tooltip("Numbers Near Icons Cities")] [SerializeField]
        private Text[] numbersNearResourcesCities;
        
        [Header("Labels Colors")][Space(5)]
        [Tooltip("Enabled Color")][SerializeField]
        private Color enabledColor;
        [Tooltip("Enabled Color")][SerializeField]
        private Color disabledColor;

        private enum LabelType
        {
            VILLAGES, CITES, PATHS, CARDS
        }
        
        void Start()
        {
            SetLabelState(LabelType.VILLAGES, true);
            SetLabelState(LabelType.CITES, true);
            SetLabelState(LabelType.PATHS, true);
            SetLabelState(LabelType.CARDS, true);
        }

        void Update()
        {
            SetLabelState(LabelType.VILLAGES, GameManager.BuildManager.CheckIfPlayerCanBuildAnyBuilding(JunctionType.Village));
            SetLabelState(LabelType.CITES, GameManager.BuildManager.CheckIfPlayerCanBuildAnyBuilding(JunctionType.City));
            SetLabelState(LabelType.PATHS, GameManager.BuildManager.CheckIfPlayerCanBuildAnyPath());
            SetLabelState(LabelType.CARDS, GameManager.CardsManager.CheckIfCurrentPlayerCanUseAnyCard());

            var villages = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetVillagesNumber();
            var cities = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetCitiesNumber();
            var paths = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber();
            var cards = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.GetCardsNumber();
            var villagesLimit = BuildManager.MaxVillageNumber;
            var citiesLimit = BuildManager.MaxCityNumber;
            var pathsLimit = BuildManager.MaxPathNumber;
            
            villagesText.text = $"{villages}/{villagesLimit}";
            citiesText.text = $"{cities}/{citiesLimit}";
            pathsText.text = $"{paths}/{pathsLimit}";
            cardsText.text = $"{cards}";
        }

        /// <summary>
        /// Changes color of the label in case of it is enabled or not
        /// </summary>
        /// <param name="label">Label to change</param>
        /// <param name="labelEnabled">Do enable or disable the label</param>
        void SetLabelState(LabelType label, bool labelEnabled)
        {
            switch(label)
            {
                case LabelType.VILLAGES:
                    villagesLabel.color = labelEnabled ? enabledColor : disabledColor;
                    villagesValue.color = labelEnabled ? enabledColor : disabledColor;
                    foreach(var e in villagesResources) e.color = labelEnabled ? enabledColor : disabledColor;
                    break;
                case LabelType.CITES:
                    citiesLabel.color = labelEnabled ? enabledColor : disabledColor;
                    citiesValue.color = labelEnabled ? enabledColor : disabledColor;
                    foreach(var e in citiesResources) e.color = labelEnabled ? enabledColor : disabledColor;
                    foreach(var e in numbersNearResourcesCities) 
                        e.color = labelEnabled ? enabledColor : disabledColor;
                    break;
                case LabelType.PATHS:
                    pathsLabel.color = labelEnabled ? enabledColor : disabledColor;
                    pathsValue.color = labelEnabled ? enabledColor : disabledColor;
                    foreach(var e in pathsResources) e.color = labelEnabled ? enabledColor : disabledColor;
                    break;
                case LabelType.CARDS:
                    cardsLabel.color = labelEnabled ? enabledColor : disabledColor;
                    cardsValue.color = labelEnabled ? enabledColor : disabledColor;
                    foreach(var e in cardsResources) e.color = labelEnabled ? enabledColor : disabledColor;
                    break;
            }
        }
    }
}
