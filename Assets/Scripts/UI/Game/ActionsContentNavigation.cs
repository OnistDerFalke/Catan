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
        [Tooltip("Land Trade Button")]
        [SerializeField] private Button landTradeButton;
        [Tooltip("Sea Trade Button")]
        [SerializeField] private Button seaTradeButton;
        [Tooltip("Move Thief Button")]
        [SerializeField] private Button moveThiefButton;
        
        //Destiny: Controller of the 3D UI Dice
        [Header("Real Dice Component")][Space(5)]
        [Tooltip("Dice Controller")]
        [SerializeField] private DiceController diceController;

        /// <summary>
        /// Throws the dice
        /// </summary>
        private void OnThrowDiceButton()
        {
            diceController.AnimateDiceOnThrow();
        }

        /// <summary>
        /// Moves the thief
        /// </summary>
        private void OnMoveThiefButton()
        {
            BoardManager.UpdateThief();

            GameManager.MovingUserMode = GameManager.CurrentDiceThrownNumber != 0 ? 
                GameManager.MovingMode.Normal : GameManager.MovingMode.ThrowDice;
            GameManager.Selected.Element = null;
            moveThiefButton.interactable = false;

            //Destiny: Popup with choosing player shows
            if (GameManager.AdjacentPlayerIdToField(BoardManager.FindThief()).Count != 0)
                GameManager.ThiefPlayerChoicePopupShown = true;
        }

        /// <summary>
        /// Event when trade button clicked
        /// </summary>
        private void OnTradeButton()
        {
           ShowAdvancedMerchantMenu(true);
        }

        /// <summary>
        /// Event on end trading part in basic mode
        /// </summary>
        private void OnEndTradeButton()
        {
            GameManager.SetProperPhase(GameManager.BasicMovingMode.BuildPhase);
            ShowAdvancedMerchantMenu(false);
        }

        /// <summary>
        /// Event on land trade button
        /// </summary>
        private void OnLandTradeButton()
        {
            //TODO: Needs to be implemented
        }
        
        /// <summary>
        /// Event on sea trade button
        /// </summary>
        private void OnSeaTradeButton()
        {
            //TODO: Needs to be implemented
        }

        /// <summary>
        /// Buys the card
        /// </summary>
        private void OnBuyCardButton()
        {
            GameManager.BoughtCardPopupShown = true;
            GameManager.LastBoughtCard = GameManager.Players[GameManager.CurrentPlayer].BuyCard();
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void OnBuildButton()
        {
            if (GameManager.Selected.Element as JunctionElement != null)
            {
                var element = (JunctionElement)GameManager.Selected.Element;
                GameManager.Players[GameManager.CurrentPlayer].BuildBuilding(element);
            }
            else if (GameManager.Selected.Element as PathElement != null)
            {
                var element = (PathElement)GameManager.Selected.Element;
                GameManager.Players[GameManager.CurrentPlayer].BuildPath(element);
            }
        }

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            GameManager.Players[GameManager.CurrentPlayer].properties.cards.UnblockCards();
            GameManager.Players[GameManager.CurrentPlayer].canUseCard = true;

            GameManager.SwitchPlayer();

            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst ||
                GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond)
                GameManager.MovingUserMode = GameManager.MovingMode.BuildVillage;
            else
                GameManager.MovingUserMode = GameManager.MovingMode.ThrowDice;
            GameManager.SetProperPhase(GameManager.BasicMovingMode.TradePhase);

            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching)
            {
                throwDiceButton.interactable = true;
                GameManager.CurrentDiceThrownNumber = 0;
                diceController.HideDicesOutputs();
            }
            GameManager.Selected.Element = null;
        }





        /// <summary>
        /// Blocks throw dice button if player hasn't throw the dice
        /// </summary>
        private void ThrowDiceButtonActivity()
        {
            if (GameManager.MovingUserMode != GameManager.MovingMode.ThrowDice || GameManager.CheckIfWindowShown())
                throwDiceButton.interactable = false;
            else
                throwDiceButton.interactable = true;
        }

        /// <summary>
        /// Blocks thief move button if moving conditions were not satisfied
        /// </summary>
        private void ThiefMoveButtonActivity()
        {
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief && 
                GameManager.Selected.Element as FieldElement != null &&
                !GameManager.CheckIfWindowShown())
            {
                moveThiefButton.interactable = !((FieldElement)GameManager.Selected.Element).IfThief();
            }
            else
            {
                moveThiefButton.interactable = false;
            }
        }

        /// <summary>
        /// Blocks trade button if conditions were not satisfied
        /// </summary>
        private void TradeButtonActivity()
        {
            if (GameManager.MovingUserMode != GameManager.MovingMode.Normal || 
                GameManager.BasicMovingUserMode == GameManager.BasicMovingMode.BuildPhase ||
                GameManager.CheckIfWindowShown())
            {
                tradeButton.interactable = false;
            }
            else
            {
                tradeButton.interactable = true;
            }
        }

        /// <summary>
        /// Blocks end trade button if conditions were not satisfied
        /// </summary>
        private void EndTradeButtonActivity()
        {
            if (GameManager.MovingUserMode != GameManager.MovingMode.Normal ||
                GameManager.BasicMovingUserMode == GameManager.BasicMovingMode.BuildPhase ||
                GameManager.CheckIfWindowShown())
            {
                endTradeButton.interactable = false;
            }
            else
            {
                endTradeButton.interactable = true;
            }
        }

        /// <summary>
        /// Blocks buy card button if buying conditions are not satisfied
        /// </summary>
        private void BuyCardButtonActivity()
        {
            if (!GameManager.CheckIfWindowShown() & ((GameManager.BasicMovingUserMode == GameManager.BasicMovingMode.Normal ||
                (GameManager.Mode == GameManager.CatanMode.Basic &&
                GameManager.BasicMovingUserMode == GameManager.BasicMovingMode.BuildPhase)) && 
                GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching))
                buyCardButton.interactable = GameManager.Players[GameManager.CurrentPlayer].CanBuyCard();
            else
                buyCardButton.interactable = false;
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            if ((GameManager.Selected.Element as PathElement != null) && (((PathElement)GameManager.Selected.Element).Available()))
                buildButton.interactable = true;
            else if ((GameManager.Selected.Element as JunctionElement != null) && (((JunctionElement)GameManager.Selected.Element).Available()))
                buildButton.interactable = true;
            else
                buildButton.interactable = false;
        }

        /// <summary>
        /// Blocks turn skip button if build conditions are not satisfied
        /// </summary>
        private void TurnSkipButtonActivity()
        {
            if (GameManager.CheckIfWindowShown() || 
                (GameManager.MovingUserMode != GameManager.MovingMode.Normal && GameManager.MovingUserMode != GameManager.MovingMode.EndTurn) ||
                GameManager.BasicMovingUserMode == GameManager.BasicMovingMode.TradePhase)
            {
                turnSkipButton.interactable = false;
                return;
            }

            // if player can build building for free
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingFirst)
            {
                turnSkipButton.interactable = 
                    GameManager.Players[GameManager.CurrentPlayer].properties.GetBuildingsNumber() == 1 &&
                    GameManager.Players[GameManager.CurrentPlayer].properties.GetPathsNumber() == 1;
            }
            else if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.InitialSwitchingSecond)
            {
                turnSkipButton.interactable =
                    GameManager.Players[GameManager.CurrentPlayer].properties.GetBuildingsNumber() == 2 &&
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

            // if it's time to move the thief
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief) 
                turnSkipButton.interactable = false;
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
            landTradeButton.transform.localPosition -= new Vector3(0, gridOffset, 0);
            seaTradeButton.transform.localPosition -= new Vector3(0, gridOffset, 0);
            moveThiefButton.transform.localPosition -= new Vector3(0, gridOffset, 0);
        }

        /// <summary>
        /// Shows advanced choice for merchant type
        /// </summary>
        /// <param name="doShow">If true advanced choice is active</param>
        private void ShowAdvancedMerchantMenu(bool doShow)
        {
            tradeButton.gameObject.SetActive(!doShow);
            landTradeButton.gameObject.SetActive(doShow);
            seaTradeButton.gameObject.SetActive(doShow);
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
            landTradeButton.onClick.AddListener(OnLandTradeButton);
            seaTradeButton.onClick.AddListener(OnSeaTradeButton);
            ManageButtonGrid();
        }

        void Update()
        {
            ThrowDiceButtonActivity();
            ThiefMoveButtonActivity();
            TradeButtonActivity();
            EndTradeButtonActivity();
            BuyCardButtonActivity();
            BuildButtonActivity();
            TurnSkipButtonActivity();
        }
    }
}
