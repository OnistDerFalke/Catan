using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class ObligatoryActionInfoPopupController : MonoBehaviour
    {
        [Header("Info Text")][Space(5)]
        [Tooltip("Info Text")]
        [SerializeField] private Text infoText;
        
        [Header("Background")][Space(5)]
        [Tooltip("Background")]
        [SerializeField] private GameObject background;
        
        [Header("Texts for events")][Space(5)]
        [Tooltip("Move thief event text")]
        [SerializeField] private string moveThiefText;
        [Tooltip("One path for free text")]
        [SerializeField] private string onePathForFreeText;
        [Tooltip("Two paths for free text")]
        [SerializeField] private string twoPathsForFreeText;
        [Tooltip("Throw dice text")]
        [SerializeField] private string throwDiceText;
        [Tooltip("Building phase text")]
        [SerializeField] private string buildingPhaseText;
        [Tooltip("Trading phase text")]
        [SerializeField] private string tradingPhaseText;

        void Update()
        {
            CheckWhatToShow();
        }

        private void CheckWhatToShow()
        {
            background.SetActive(true);
            switch (GameManager.MovingUserMode)
            {
                case GameManager.MovingMode.MovingThief:
                {
                    infoText.text = moveThiefText;
                    break;
                }
                case GameManager.MovingMode.OnePathForFree:
                {
                    infoText.text = onePathForFreeText;
                    break;
                }
                case GameManager.MovingMode.TwoPathsForFree:
                {
                    infoText.text = twoPathsForFreeText;
                    break;
                    }
                case GameManager.MovingMode.ThrowDice:
                {
                    infoText.text = throwDiceText;
                    break;
                }
                case GameManager.MovingMode.Normal:
                {
                    switch (GameManager.BasicMovingUserMode)
                    {
                        case GameManager.BasicMovingMode.BuildPhase:
                        {
                            infoText.text = buildingPhaseText;
                            break;
                        }
                        case GameManager.BasicMovingMode.TradePhase:
                        {
                            infoText.text = tradingPhaseText;
                            break;
                        }
                        case GameManager.BasicMovingMode.Normal:
                        {
                            background.SetActive(false);
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }
}
