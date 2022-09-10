using Assets.Scripts.DataStorage.Managers;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

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
        
        void Update()
        {
            int villages = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetVillagesNumber();
            int cities = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetCitiesNumber();
            int paths = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber();
            int villagesLimit = BuildManager.MaxVillageNumber;
            int citiesLimit = BuildManager.MaxCityNumber;
            int pathsLimit = BuildManager.MaxPathNumber;

            villagesText.text = $"{villages}/{villagesLimit}";
            citiesText.text = $"{cities}/{citiesLimit}";
            pathsText.text = $"{paths}/{pathsLimit}";
        }
    }
}
