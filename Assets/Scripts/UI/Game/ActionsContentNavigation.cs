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
        [Tooltip("Throw Dice Button")]
        [SerializeField] private Button throwDiceButton;
        [Tooltip("Buy Card Button")]
        [SerializeField] private Button buyCardButton;
        
        //Destiny: Controller of the 3D UI Dice
        [Header("Real Dice Component")][Space(5)]
        [Tooltip("Dice Controller")]
        [SerializeField] private DiceController diceController;

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

            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching)
            {
                throwDiceButton.interactable = true;
                GameManager.CurrentDiceThrownNumber = 0;
                diceController.HideDicesOutputs();
            }
            GameManager.Selected.Element = null;
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void OnBuildButton()
        {
            if (GameManager.Selected.Element as JunctionElement != null)
            {
                var element = (JunctionElement) GameManager.Selected.Element;
                GameManager.Players[GameManager.CurrentPlayer].BuildBuilding(element);
            }
            else if (GameManager.Selected.Element as PathElement != null)
            {
                var element = (PathElement) GameManager.Selected.Element;
                GameManager.Players[GameManager.CurrentPlayer].BuildPath(element);
            }
        }

        /// <summary>
        /// Throws the dice
        /// </summary>
        private void OnThrowDiceButton()
        {
            diceController.AnimateDiceOnThrow();
        }

        /// <summary>
        /// Buys the card
        /// </summary>
        private void OnBuyCardButton()
        {
            GameManager.Players[GameManager.CurrentPlayer].BuyCard();
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            if ((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber == 0) || 
                GameManager.Selected.Element == null)
            {
                buildButton.interactable = false;
                return;
            }

            if (((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber != 0) ||
                GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching) && 
                GameManager.Selected.Element as JunctionElement != null)
            {
                buildButton.interactable = GameManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)GameManager.Selected.Element).id);
            }
            else if (((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber != 0) ||
                GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching) && 
                GameManager.Selected.Element as PathElement != null)
            {
                buildButton.interactable = GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
            }
            else
            {
                buildButton.interactable = true;
            }
        }

        /// <summary>
        /// Blocks turn skip button if build conditions are not satisfied
        /// </summary>
        private void TurnSkipButtonActivity()
        {
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst)
            {
                turnSkipButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties.buildings.Count == 1 &&
                    GameManager.Players[GameManager.CurrentPlayer].properties.paths.Count == 1;
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                turnSkipButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties.buildings.Count == 2 &&
                    GameManager.Players[GameManager.CurrentPlayer].properties.paths.Count == 2;
            }
            else if (GameManager.CurrentDiceThrownNumber == 0)
            {
                turnSkipButton.interactable = false;
            } 
            else
            {
                turnSkipButton.interactable = true;
            }
        }

        private void ThrowDiceButtonActivity()
        {
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber == 0)
                throwDiceButton.interactable = true;
            else
                throwDiceButton.interactable = false;
        }

        /// <summary>
        /// Blocks buy card button if buying conditions are not satisfied
        /// </summary>
        private void BuyCardButtonActivity()
        {
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching)
                buyCardButton.interactable = GameManager.Players[GameManager.CurrentPlayer].CanBuyCard();
            else
                buyCardButton.interactable = false;
        }

        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
            buildButton.onClick.AddListener(OnBuildButton);
            throwDiceButton.onClick.AddListener(OnThrowDiceButton);
            buyCardButton.onClick.AddListener(OnBuyCardButton);
        }

        void Update()
        {
            BuildButtonActivity();
            TurnSkipButtonActivity();
            ThrowDiceButtonActivity();
            BuyCardButtonActivity();
        }
    }
}
