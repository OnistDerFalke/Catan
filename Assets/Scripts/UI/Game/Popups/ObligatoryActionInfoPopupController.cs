using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using static Board.States.GameState;

namespace UI.Game.Popups
{
    public class ObligatoryActionInfoPopupController : MonoBehaviour
    {
        [Header("Info Text")][Space(5)]
        [Tooltip("Info Text")][SerializeField] 
        private Text infoText;
        
        [Header("Background")][Space(5)]
        [Tooltip("Background")][SerializeField] 
        private GameObject background;
        
        [Header("Texts for events")][Space(5)]
        [Tooltip("Move thief event text")][SerializeField]
        private string moveThiefText;
        [Tooltip("One path for free text")][SerializeField]
        private string onePathForFreeText;
        [Tooltip("Two paths for free text")][SerializeField] 
        private string twoPathsForFreeText;
        [Tooltip("Throw dice text")][SerializeField]
        private string throwDiceText;
        [Tooltip("Building phase text")][SerializeField] 
        private string buildingPhaseText;
        [Tooltip("Trading phase text")][SerializeField]
        private string tradingPhaseText;
        [Tooltip("Build one village text")][SerializeField]
        private string buildVillageText;
        [Tooltip("Build one path text")][SerializeField]
        private string buildPathText;
        [Tooltip("End turn text")][SerializeField] 
        private string endTurnText;
        [Tooltip("End game text")][SerializeField] 
        private string endGameText;

        void Update()
        {
            CheckWhatToShow();
        }

        private void CheckWhatToShow()
        {
            background.SetActive(true);

            //Destiny: End game
            if (GameManager.EndGame)
            {
                infoText.text = endGameText;
                return;
            }

            if (GameManager.State.MovingUserMode != MovingMode.Normal)
            {
                infoText.text = GameManager.State.MovingUserMode switch
                {
                    MovingMode.MovingThief => moveThiefText,
                    MovingMode.OnePathForFree => onePathForFreeText,
                    MovingMode.TwoPathsForFree => twoPathsForFreeText,
                    MovingMode.ThrowDice => throwDiceText,
                    MovingMode.BuildPath => buildPathText,
                    MovingMode.BuildVillage => buildVillageText,
                    MovingMode.EndTurn => endTurnText,
                    _ => ""
                };

                return;
            }

            if (GameManager.State.BasicMovingUserMode != BasicMovingMode.Normal)
            {
                infoText.text = GameManager.State.BasicMovingUserMode switch
                {
                    BasicMovingMode.BuildPhase => buildingPhaseText,
                    BasicMovingMode.TradePhase => tradingPhaseText,
                    _ => ""
                };

                return;
            }

            background.SetActive(false);
        }
    }
}
