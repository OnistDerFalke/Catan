using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using static Board.States.GameState;

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
        [Tooltip("Build one village text")]
        [SerializeField] private string buildVillageText;
        [Tooltip("Build one path text")]
        [SerializeField] private string buildPathText;
        [Tooltip("End turn text")]
        [SerializeField] private string endTurnText;
        [Tooltip("End game text")]
        [SerializeField] private string endGameText;

        void Update()
        {
            CheckWhatToShow();
        }

        private void CheckWhatToShow()
        {
            //Destiny: End game
            if (GameManager.EndGame)
            {
                infoText.text = endGameText;
                return;
            }

            //Destiny: Initial distribution during advanced game
            if (GameManager.State.Mode == CatanMode.Advanced && 
                (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond))
            {
                switch (GameManager.State.MovingUserMode)
                {
                    case MovingMode.BuildPath:
                        infoText.text = buildPathText;
                        break;
                    case MovingMode.BuildVillage:
                        infoText.text = buildVillageText;
                        break;
                    case MovingMode.EndTurn:
                        infoText.text = endTurnText;
                        break;
                }
                
                return;
            }

            //Destiny: Basic game or advanced game after initial distribution
            Debug.Log($"{GameManager.State.MovingUserMode}  {GameManager.State.BasicMovingUserMode}");
            switch (GameManager.State.MovingUserMode)
            {
                case MovingMode.MovingThief:
                    background.SetActive(true);
                    infoText.text = moveThiefText;
                    break;
                case MovingMode.OnePathForFree:
                    background.SetActive(true);
                    infoText.text = onePathForFreeText;
                    break;
                case MovingMode.TwoPathsForFree:
                    background.SetActive(true);
                    infoText.text = twoPathsForFreeText;
                    break;
                case MovingMode.ThrowDice:
                    background.SetActive(true);
                    infoText.text = throwDiceText;
                    break;
                case MovingMode.Normal:
                    switch (GameManager.State.BasicMovingUserMode)
                    {
                        case BasicMovingMode.BuildPhase:
                            background.SetActive(true);
                            infoText.text = buildingPhaseText;
                            break;
                        case BasicMovingMode.TradePhase:
                            background.SetActive(true);
                            infoText.text = tradingPhaseText;
                            break;
                        case BasicMovingMode.Normal:
                            background.SetActive(false);
                            break;
                    }
                    break;
            }
        }
    }
}
