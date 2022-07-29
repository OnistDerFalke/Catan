using UnityEngine;
using UnityEngine.UI;
using static DataStorage.GameManager;

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
            int v = Players[CurrentPlayer].properties.GetVillagesNumber();
            int c = Players[CurrentPlayer].properties.GetCitiesNumber();
            int p = Players[CurrentPlayer].properties.GetPathsNumber();
            int vlim = BuildManager.MaxVillageNumber;
            int clim = BuildManager.MaxCityNumber;
            int plim = BuildManager.MaxPathNumber;

            villagesText.text = $"{v}/{vlim}";
            citiesText.text = $"{c}/{clim}";
            pathsText.text = $"{p}/{plim}";
        }
    }
}
