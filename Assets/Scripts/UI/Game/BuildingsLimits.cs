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
            int v = GameManager.Players[GameManager.CurrentPlayer].properties.GetVillagesNumber();
            int c = GameManager.Players[GameManager.CurrentPlayer].properties.GetCitiesNumber();
            int p = GameManager.Players[GameManager.CurrentPlayer].properties.GetPathsNumber();
            int vlim = GameManager.MaxVillageNumber;
            int clim = GameManager.MaxCityNumber;
            int plim = GameManager.MaxPathNumber;

            villagesText.text = $"{v.ToString()}/{vlim.ToString()}";
            citiesText.text = $"{c.ToString()}/{clim.ToString()}";
            pathsText.text = $"{p.ToString()}/{plim.ToString()}";
        }
    }
}
