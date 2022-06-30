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
        
        [Header("Texts for events")][Space(5)]
        [Tooltip("Move thief event")]
        [SerializeField] private string moveThiefText;
        [Tooltip("One path for free")]
        [SerializeField] private string onePathForFreeText;
        [Tooltip("Two paths for free")]
        [SerializeField] private string twoPathsForFreeText;
       
        void OnEnable()
        {
            CheckWhatToShow();
        }

        private void CheckWhatToShow()
        {
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
            }
        }
    }
}
