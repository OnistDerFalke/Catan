using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class BuildingsLimits : MonoBehaviour
    {
        [Header("Limits")][Space(5)]
        [Tooltip("Villages Text")]
        [SerializeField] private Text villagesText;
        [Tooltip("Cities Text")]
        [SerializeField] private Text citiesText;
        [Tooltip("Paths Text")]
        [SerializeField] private Text pathsText;        
        
        void Update()
        {
            //Destiny: Temporary hardcoded, needs to be set from e.g. GameManager (both limits and current numbers)
            //Here v (villages), c (cities), p (paths), lim (limit)
            int v = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetVillagesNumber();
            int c = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetCitiesNumber();
            int p = GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber();
            int vlim = GameManager.BuildManager.MaxVillageNumber;
            int clim = GameManager.BuildManager.MaxCityNumber;
            int plim = GameManager.BuildManager.MaxPathNumber;

            villagesText.text = $"{v}/{vlim}";
            citiesText.text = $"{c}/{clim}";
            pathsText.text = $"{p}/{plim}";
        }
    }
}
