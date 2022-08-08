using System.Collections;
using Board;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using static Board.States.GameState;

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
        
        //Destiny: Cards scrollbar needed to reset
        [Header("Cards Scrollbar Rect")][Space(5)]
        [Tooltip("Cards Scrollrect")] [SerializeField] private ScrollRect cardsScrollrect;

        //Destiny: Turn skip button text variants
        [Header("Turn skip button text variants")][Space(5)]
        [Tooltip("Turn skip text")] [SerializeField] private string turnSkipText;
        [Tooltip("End game text")] [SerializeField] private string endGameText;

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

            GameManager.State.MovingUserMode = 
                GameManager.State.CurrentDiceThrownNumber != 0 ? MovingMode.Normal : MovingMode.ThrowDice;
            GameManager.Selected.Element = null;
            moveThiefButton.interactable = false;

            //Destiny: Popup with choosing player shows
            if (GameManager.AdjacentPlayerIdToFieldWithResource(BoardManager.FindThief()).Count != 0)
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.THIEF_PLAYER_CHOICE_POPUP] = true;
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
            GameManager.SetProperPhase(BasicMovingMode.BuildPhase);
            ShowAdvancedMerchantMenu(false);
        }

        /// <summary>
        /// Event on land trade button
        /// </summary>
        private void OnLandTradeButton()
        {
            //Destiny: Showing trade offer creator window
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.LAND_TRADE_POPUP] = true;
            
            //TODO: Needs to be implemented (logic)
        }
        
        /// <summary>
        /// Event on sea trade button
        /// </summary>
        private void OnSeaTradeButton()
        {
            //Destiny: Showing sea trade popup
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.SEA_TRADE_POPUP] = true;
        }

        /// <summary>
        /// Buys the card
        /// </summary>
        private void OnBuyCardButton()
        {
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.BOUGHT_CARD_POPUP] = true;
            GameManager.PopupManager.LastBoughtCard = GameManager.State.Players[GameManager.State.CurrentPlayerId].BuyCard();
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void OnBuildButton()
        {
            if (GameManager.Selected.Element as JunctionElement != null)
            {
                var element = (JunctionElement)GameManager.Selected.Element;
                GameManager.State.Players[GameManager.State.CurrentPlayerId].BuildBuilding(element);
            }
            else if (GameManager.Selected.Element as PathElement != null)
            {
                var element = (PathElement)GameManager.Selected.Element;
                GameManager.State.Players[GameManager.State.CurrentPlayerId].BuildPath(element);
            }
        }

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            //Destiny: Resets cards scrollbar
            StartCoroutine(ResetCardsScrollbar());
            
            //Destiny: Hide advanced merchant menu on switching to next player
            ShowAdvancedMerchantMenu(false);
            
            //Destiny: If player has at least 10 points at the end of his turn than end the game
            if (GameManager.EndGameCondition())
            {
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.END_GAME_POPUP] = true;
            }

            GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.UnblockCards();
            GameManager.State.Players[GameManager.State.CurrentPlayerId].canUseCard = true;

            GameManager.SwitchPlayer();

            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst ||
                GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond)
            {
                GameManager.State.MovingUserMode = MovingMode.BuildVillage;
            }
            else
            {
                GameManager.State.MovingUserMode = MovingMode.ThrowDice;
            }
            GameManager.SetProperPhase(BasicMovingMode.TradePhase);

            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching)
            {
                throwDiceButton.interactable = true;
                GameManager.State.CurrentDiceThrownNumber = 0;
                diceController.HideDicesOutputs();
            }
            GameManager.Selected.Element = null;
        }

        /// <summary>
        /// Blocks throw dice button if player hasn't throw the dice
        /// </summary>
        private void ThrowDiceButtonActivity()
        {
            if (GameManager.State.MovingUserMode != MovingMode.ThrowDice || GameManager.PopupManager.CheckIfWindowShown())
                throwDiceButton.interactable = false;
            else
                throwDiceButton.interactable = true;
        }

        /// <summary>
        /// Blocks thief move button if moving conditions were not satisfied
        /// </summary>
        private void ThiefMoveButtonActivity()
        {
            if (GameManager.State.MovingUserMode == MovingMode.MovingThief && 
                GameManager.Selected.Element as FieldElement != null &&
                !GameManager.PopupManager.CheckIfWindowShown())
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
            if (GameManager.State.MovingUserMode != MovingMode.Normal || 
                GameManager.State.BasicMovingUserMode == BasicMovingMode.BuildPhase ||
                GameManager.PopupManager.CheckIfWindowShown())
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
            //Destiny: End trade button does not exist in advanced mode so interactions cannot be checked
            if (GameManager.State.Mode == CatanMode.Advanced) 
                return;
            
            if (GameManager.State.MovingUserMode != MovingMode.Normal || 
                GameManager.State.BasicMovingUserMode == BasicMovingMode.BuildPhase ||
                GameManager.PopupManager.CheckIfWindowShown())
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
            if (!GameManager.PopupManager.CheckIfWindowShown() &&
                ((GameManager.State.BasicMovingUserMode == BasicMovingMode.Normal &&
                GameManager.State.CurrentDiceThrownNumber != 0) ||
                (GameManager.State.Mode == CatanMode.Basic && 
                GameManager.State.BasicMovingUserMode == BasicMovingMode.BuildPhase)) &&
                GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                GameManager.CardsManager.Deck.Count > 0)
            {
                buyCardButton.interactable = GameManager.State.Players[GameManager.State.CurrentPlayerId].CanBuyCard();
            }
            else
            {
                buyCardButton.interactable = false;
            }
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            if ((GameManager.Selected.Element as PathElement != null) && 
                ((PathElement)GameManager.Selected.Element).Available(GameManager.Selected.Element))
            {
                buildButton.interactable = true;
            }
            else if ((GameManager.Selected.Element as JunctionElement != null) && 
                ((JunctionElement)GameManager.Selected.Element).Available(GameManager.Selected.Element))
            {
                buildButton.interactable = true;
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
            if (GameManager.PopupManager.CheckIfWindowShown() || 
                (GameManager.State.MovingUserMode != MovingMode.Normal && 
                GameManager.State.MovingUserMode != MovingMode.EndTurn) ||
                GameManager.State.BasicMovingUserMode == BasicMovingMode.TradePhase)
            {
                turnSkipButton.interactable = false;
                return;
            }

            // if player can build building for free
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst)
            {
                turnSkipButton.interactable = 
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 1 &&
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 1;
            }
            else if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond)
            {
                turnSkipButton.interactable = 
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 2 &&
                    GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 2;
            }
            // if the dice wasn't rolled
            else if (GameManager.State.CurrentDiceThrownNumber == 0)
            {
                turnSkipButton.interactable = false;
            } 
            else
            {
                turnSkipButton.interactable = true;
            }

            // if it's time to move the thief
            if (GameManager.State.MovingUserMode == MovingMode.MovingThief) 
                turnSkipButton.interactable = false;
        }

        /// <summary>
        /// Update text of turn skip button based on the fact if the game ended
        /// </summary>
        private void TurnSkipButtonText()
        {
            turnSkipButton.GetComponentInChildren<Text>().text = GameManager.EndGameCondition() ? endGameText : turnSkipText;
        }

        /// <summary>
        /// Hides redundant buttons and modifies the UI depending on the game mode 
        /// </summary>
        private void ManageButtonGrid()
        {
            if (GameManager.State.Mode != CatanMode.Advanced) 
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

        /// <summary>
        /// Resets scrollbar rect to default value
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResetCardsScrollbar()
        {
            yield return null;
            cardsScrollrect.verticalNormalizedPosition = 1f;
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
            TurnSkipButtonText();
        }
    }
}
