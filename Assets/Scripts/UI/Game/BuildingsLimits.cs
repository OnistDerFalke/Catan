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
        
        [Header("Labels Colors")][Space(5)]
        [Tooltip("Enabled Color")][SerializeField]
        private Color enabledColor;
        [Tooltip("Enabled Color")][SerializeField]
        private Color disabledColor;

        void Start()
        {
            SetLabelState(villagesLabel, true);
            SetLabelState(citiesLabel, true);
            SetLabelState(pathsLabel, true);
            SetLabelState(cardsLabel, true);
        }

        void Update()
        {
            SetLabelState(villagesLabel, GameManager.BuildManager.CheckIfPlayerCanBuildAnyBuilding(JunctionType.Village));
            SetLabelState(citiesLabel, GameManager.BuildManager.CheckIfPlayerCanBuildAnyBuilding(JunctionType.City));
            SetLabelState(pathsLabel, GameManager.BuildManager.CheckIfPlayerCanBuildAnyPath());
            SetLabelState(cardsLabel, GameManager.State.Players[GameManager.State.CurrentPlayerId].CanBuyCard());

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
        void SetLabelState(Text label, bool labelEnabled)
        {
            label.color = labelEnabled ? enabledColor : disabledColor;
        }
    }
}
