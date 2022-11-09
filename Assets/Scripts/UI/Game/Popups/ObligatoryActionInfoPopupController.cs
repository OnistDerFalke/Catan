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
        [Tooltip("Additional Info Text")][SerializeField] 
        private Text additionalInfoText;
        
        [Header("Background")][Space(5)]
        [Tooltip("Background")][SerializeField] 
        private GameObject background;
        
        [Header("Texts for events")][Space(5)]
        [Tooltip("Move thief event text")][SerializeField]
        private string[] moveThiefText;
        [Tooltip("One path for free text")][SerializeField]
        private string[] onePathForFreeText;
        [Tooltip("Two paths for free text")][SerializeField] 
        private string[] twoPathsForFreeText;
        [Tooltip("Building phase text")][SerializeField] 
        private string[] buildingPhaseText;
        [Tooltip("Trading phase text")][SerializeField]
        private string[] tradingPhaseText;
        [Tooltip("Build one village text")][SerializeField]
        private string[] buildVillageText;
        [Tooltip("Build one path text")][SerializeField]
        private string[] buildPathText;
        [Tooltip("End turn text")][SerializeField] 
        private string[] endTurnText;
        [Tooltip("End game text")][SerializeField] 
        private string[] endGameText;

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
                infoText.text = endGameText[0];
                additionalInfoText.text = endGameText[1];
                return;
            }

            if (GameManager.State.MovingUserMode != MovingMode.Normal)
            {
                infoText.text = GameManager.State.MovingUserMode switch
                {
                    MovingMode.MovingThief => moveThiefText[0],
                    MovingMode.OnePathForFree => onePathForFreeText[0],
                    MovingMode.TwoPathsForFree => twoPathsForFreeText[0],
                    MovingMode.BuildPath => buildPathText[0],
                    MovingMode.BuildVillage => buildVillageText[0],
                    MovingMode.EndTurn => endTurnText[0],
                    _ => ""
                };

                additionalInfoText.text = GameManager.State.MovingUserMode switch
                {
                    MovingMode.MovingThief => moveThiefText[1],
                    MovingMode.OnePathForFree => onePathForFreeText[1],
                    MovingMode.TwoPathsForFree => twoPathsForFreeText[1],
                    MovingMode.BuildPath => buildPathText[1],
                    MovingMode.BuildVillage => buildVillageText[1],
                    MovingMode.EndTurn => endTurnText[1],
                    _ => ""
                };

                if (infoText.text == "")
                    background.SetActive(false);

                return;
            }

            if (GameManager.State.BasicMovingUserMode != BasicMovingMode.Normal)
            {
                infoText.text = GameManager.State.BasicMovingUserMode switch
                {
                    BasicMovingMode.BuildPhase => buildingPhaseText[0],
                    BasicMovingMode.TradePhase => tradingPhaseText[0],
                    _ => ""
                };
                
                additionalInfoText.text = GameManager.State.BasicMovingUserMode switch
                {
                    BasicMovingMode.BuildPhase => buildingPhaseText[1],
                    BasicMovingMode.TradePhase => tradingPhaseText[1],
                    _ => ""
                };

                return;
            }

            background.SetActive(false);
        }
    }
}
