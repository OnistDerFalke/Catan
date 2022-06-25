using Board;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    //Destiny: Handling interactions with actions tab UI
    public class ActionsContentNavigation : MonoBehaviour
    {
        //Destiny: Settings of the action buttons grid
        [Header("Button Grid")][Space(5)]
        [Tooltip("Grid Offset")]
        [SerializeField] private float gridOffset;
        
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
        [Tooltip("Trade Button")]
        [SerializeField] private Button tradeButton;
        [Tooltip("End Trade Button")]
        [SerializeField] private Button endTradeButton;
        [Tooltip("Move Thief Button")]
        [SerializeField] private Button moveThiefButton;
        
        //Destiny: Controller of the 3D UI Dice
        [Header("Real Dice Component")][Space(5)]
        [Tooltip("Dice Controller")]
        [SerializeField] private DiceController diceController;

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            GameManager.Players[GameManager.CurrentPlayer].properties.cards.UnblockCards();

            //Destiny: If it's first turn of elements initial distribution in advanced mode and not last player
            //then switch to next player
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst &&
                GameManager.CurrentPlayer != GameManager.PlayersNumber - 1)
            {
                GameManager.SwitchToNextPlayer();
            }
            //Destiny: If it's first turn of elements initial distribution in advanced mode and last player
            //then switch to different mode
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst &&
                GameManager.CurrentPlayer == GameManager.PlayersNumber - 1)
            {
                GameManager.SwitchingGameMode = GameManager.SwitchingMode.InitialSwitchingSecond;
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and not first player
            //then switch to previous player
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond &&
                GameManager.CurrentPlayer != 0)
            {
                GameManager.SwitchToPreviousPlayer();
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and first player
            //then switch to different mode
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
        /// Event when trade button clicked
        /// </summary>
        private void OnTradeButton()
        {
            //TODO: Needs to be implemented
        }

        /// <summary>
        /// Event on end trading part in basic mode
        /// </summary>
        private void OnEndTradeButton()
        {
            //TODO: Needs to be implemented
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
        /// Moves the thief
        /// </summary>
        private void OnMoveThiefButton()
        {
            GameManager.MovingUserMode = GameManager.MovingMode.Normal;
            GameManager.Selected.Element = null;
            moveThiefButton.enabled = false;
            
            //TODO: Moving thief on other place
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            // if the dice wasn't rolled during normal game and player didn't use the card
            // or if none element is selected
            if ((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && 
                GameManager.CurrentDiceThrownNumber == 0 && 
                GameManager.MovingUserMode == GameManager.MovingMode.Normal) || 
                GameManager.Selected.Element == null)
            {
                buildButton.interactable = false;
            }
            // if the dice wasn't rolled during normal game but player used the card
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching &&
                GameManager.CurrentDiceThrownNumber == 0 &&
                (GameManager.MovingUserMode == GameManager.MovingMode.TwoPathsForFree || 
                    GameManager.MovingUserMode == GameManager.MovingMode.OnePathForFree) &&
                GameManager.Selected.Element as PathElement != null)
            {
                buildButton.interactable = GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
            }            
            // if the dice was rolled during normal game 
            // or it's first distribution on advanced level
            // and the junction was selected
            else if (((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber != 0) ||
                GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching) && 
                GameManager.Selected.Element as JunctionElement != null)
            {
                buildButton.interactable = GameManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)GameManager.Selected.Element).id);
            }
            // if the dice was rolled during normal game 
            // or it's first distribution on advanced level
            // and the path was selected
            else if (((GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching && GameManager.CurrentDiceThrownNumber != 0) ||
                GameManager.SwitchingGameMode != GameManager.SwitchingMode.GameSwitching) && 
                GameManager.Selected.Element as PathElement != null)
            {
                buildButton.interactable = GameManager.CheckIfPlayerCanBuildPath(((PathElement)GameManager.Selected.Element).id);
            }
            else
            {
                buildButton.interactable = false;
            }
        }

        /// <summary>
        /// Blocks turn skip button if build conditions are not satisfied
        /// </summary>
        private void TurnSkipButtonActivity()
        {
            // if player can build path for free
            if (GameManager.MovingUserMode == GameManager.MovingMode.TwoPathsForFree || 
                GameManager.MovingUserMode == GameManager.MovingMode.OnePathForFree)
            {
                turnSkipButton.interactable = false;
                return;
            }

            // if player can build building for free
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst)
            {
                turnSkipButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties.GetBuildingsNumber() == 1 &&
                    GameManager.Players[GameManager.CurrentPlayer].properties.GetPathsNumber() == 1;
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                turnSkipButton.interactable = GameManager.Players[GameManager.CurrentPlayer].properties.GetBuildingsNumber() == 2 &&
                    GameManager.Players[GameManager.CurrentPlayer].properties.GetPathsNumber() == 2;
            }
            // if the dice wasn't rolled
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

        /// <summary>
        /// Blocks thief move button if moving conditions were not satisfied
        /// </summary>
        private void ThiefMoveButtonActivity()
        {
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief
                && GameManager.Selected.Element as FieldElement != null) {
                moveThiefButton.enabled = !((FieldElement)GameManager.Selected.Element).IfThief();
            }
            else moveThiefButton.enabled = false;
        }

        /// <summary>
        /// Hides redundant buttons and modifies the UI depending on the game mode 
        /// </summary>
        private void ManageButtonGrid()
        {
            if (GameManager.Mode != GameManager.CatanMode.Advanced) 
                return;
            
            //Destiny: End trade button disappears and all above needs to be moved down
            Destroy(endTradeButton.gameObject);
            tradeButton.transform.localPosition -= new Vector3(0, gridOffset, 0);
            moveThiefButton.transform.localPosition -= new Vector3(0, gridOffset, 0);
        }

        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
            buildButton.onClick.AddListener(OnBuildButton);
            throwDiceButton.onClick.AddListener(OnThrowDiceButton);
            buyCardButton.onClick.AddListener(OnBuyCardButton);
            tradeButton.onClick.AddListener(OnTradeButton);
            endTradeButton.onClick.AddListener(OnEndTradeButton);
            moveThiefButton.onClick.AddListener(OnMoveThiefButton);
            
            ManageButtonGrid();
        }

        void Update()
        {
            BuildButtonActivity();
            TurnSkipButtonActivity();
            ThrowDiceButtonActivity();
            BuyCardButtonActivity();
            ThiefMoveButtonActivity();
        }
    }
}
