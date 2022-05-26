using Board;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    //Destiny: Handling interactions with actions tab UI
    public class ActionsContentNavigation : MonoBehaviour
    {
        //Destiny: Buttons of action tab content
        [Header("Action tab buttons")][Space(5)]
        [Tooltip("Turn Skip Button")]
        [SerializeField] private Button turnSkipButton;
        [Tooltip("Build Button")]
        [SerializeField] private Button buildButton;

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst &&
                GameManager.CurrentPlayer != GameManager.PlayersNumber - 1)
            {
                GameManager.SwitchToNextPlayer();
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst &&
                GameManager.CurrentPlayer == GameManager.PlayersNumber - 1)
            {
                GameManager.SwitchingGameMode = GameManager.SwitchingMode.InitialSwitchingSecond;
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond &&
                GameManager.CurrentPlayer != 0)
            {
                GameManager.SwitchToPreviousPlayer();
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond &&
                GameManager.CurrentPlayer == 0)
            {
                GameManager.SwitchingGameMode = GameManager.SwitchingMode.GameSwitching;
            }
            else
            {
                GameManager.SwitchToNextPlayer();
            }
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void OnBuildButton()
        {
            if (GameManager.Selected.Element as JunctionElement != null)
            {
                var element = (JunctionElement) GameManager.Selected.Element;
                var initialDistribution = 
                    GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching ? false : true;
                GameManager.Players[GameManager.CurrentPlayer].properties
                    .AddBuilding(element.id, element.type == JunctionElement.JunctionType.Village, initialDistribution);
            }
            else if (GameManager.Selected.Element as PathElement != null)
            {
                var element = (PathElement) GameManager.Selected.Element;
                var initialDistribution =
                    GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching ? false : true;
                GameManager.Players[GameManager.CurrentPlayer].properties.AddPath(element.id, initialDistribution);
            }
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            if (GameManager.Selected.Element == null)
            {
                buildButton.interactable = false;
                return;
            }

            if (GameManager.Selected.Element as JunctionElement != null)
            {
                buildButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties
                    .CheckIfPlayerCanBuildBuilding(((JunctionElement)GameManager.Selected.Element).id);
            }
            else if (GameManager.Selected.Element as PathElement != null)
            {
                buildButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties
                    .CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
            }
        }
        
        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
            buildButton.onClick.AddListener(OnBuildButton);
        }

        void Update()
        {
            BuildButtonActivity();
        }
    }
}
