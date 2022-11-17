using System.Collections;
using System.Linq;
using Assets.Scripts.Board.States;
using Assets.Scripts.DataStorage.Managers;
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
        //Destiny: Buttons of action tab content
        [Header("Action tab buttons")][Space(5)]
        [Tooltip("Turn Skip Button")][SerializeField]
        private Button turnSkipButton;
        [Tooltip("Buy Card Button")][SerializeField]
        private Button buyCardButton;
        [Tooltip("Cancel Button")][SerializeField]
        private Button cancelButton;
        [Tooltip("Trade Button")][SerializeField] 
        private Button tradeButton;
        [Tooltip("Land Trade Button")][SerializeField] 
        private Button landTradeButton;
        [Tooltip("Sea Trade Button")][SerializeField] 
        private Button seaTradeButton;

        //Destiny: Icons on buttons of action tab content
        [Header("Action tab buttons icons")][Space(5)]
        [Tooltip("Turn Skip Button Icon")][SerializeField]
        private Image turnSkipIcon;
        [Tooltip("Buy Card Button Icon")][SerializeField]
        private Image buyCardIcon;
        [Tooltip("Cancel Button Icon")][SerializeField]
        private Image cancelIcon;
        [Tooltip("Trade Button Icon")][SerializeField] 
        private Image tradeIcon;
        
        //Destiny: Colors of icons on buttons of action tab content
        [Header("Action tab buttons icons' colors")][Space(5)]
        [Tooltip("Turn Skip Button Icon Color")][SerializeField]
        private Color turnSkipIconColor;
        [Tooltip("Buy Card Button Icon Color")][SerializeField]
        private Color buyCardIconColor;
        [Tooltip("Cancel Button Icon Color")][SerializeField]
        private Color cancelIconColor;
        [Tooltip("Trade Button Icon Color")][SerializeField] 
        private Color tradeIconColor;
        

        //Destiny: Controller of the 3D UI Dice
        [Header("Real Dice Component")][Space(5)]
        [Tooltip("Dice Controller")][SerializeField]
        private DiceController diceController;
        
        //Destiny: Cards scrollbar needed to reset
        [Header("Cards Scrollbar Rect")][Space(5)]
        [Tooltip("Cards Scrollrect")][SerializeField]
        private ScrollRect cardsScrollrect;

        //Destiny: Turn skip button text variants
        [Header("Turn skip button text variants")][Space(5)]
        [Tooltip("Turn skip text")][SerializeField]
        private string turnSkipText;
        [Tooltip("End game text")][SerializeField] 
        private string endGameText;

        /// <summary>
        /// Throws the dice
        /// </summary>
        private void OnThrowDiceButton()
        {
            diceController.AnimateDiceOnThrow();
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
            GameManager.State.BasicMovingUserMode = 
                GameManager.State.Mode == CatanMode.Basic ? BasicMovingMode.BuildPhase : BasicMovingMode.Normal;

            ShowAdvancedMerchantMenu(false);
        }

        /// <summary>
        /// Event on land trade button
        /// </summary>
        private void OnLandTradeButton()
        {
            //Destiny: Showing trade offer creator window
            GameManager.PopupManager.PopupsShown[PopupManager.LAND_TRADE_POPUP] = true;
        }
        
        /// <summary>
        /// Event on sea trade button
        /// </summary>
        private void OnSeaTradeButton()
        {
            //Destiny: Showing sea trade popup
            GameManager.PopupManager.PopupsShown[PopupManager.SEA_TRADE_POPUP] = true;
        }

        /// <summary>
        /// Buys the card
        /// </summary>
        private void OnBuyCardButton()
        {
            OnEndTradeButton();
            GameManager.PopupManager.PopupsShown[PopupManager.BOUGHT_CARD_POPUP] = true;
            GameManager.PopupManager.LastBoughtCard = GameManager.State.Players[GameManager.State.CurrentPlayerId].BuyCard();
        }
        
         /// <summary>
        /// Defines the cancel button feature
        /// </summary>
        private void OnCancelButton()
        {
            //TODO: Implementation of the cancel feature
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
                GameManager.PopupManager.PopupsShown[PopupManager.END_GAME_POPUP] = true;
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

            GameManager.State.BasicMovingUserMode =
                GameManager.State.Mode == CatanMode.Basic ? BasicMovingMode.TradePhase : BasicMovingMode.Normal;

            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching)
            {
                GameManager.State.CurrentDiceThrownNumber = 0;
                diceController.HideDicesOutputs();
            }
            GameManager.Selected.Element = null;

            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching)
                OnThrowDiceButton();
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void InvokeBuildProcedure()
        {
            OnEndTradeButton();

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
        /// Moves the thief
        /// </summary>
        private void InvokeMoveThiefProcedure()
        {
            BoardManager.UpdateThief();

            GameManager.State.MovingUserMode = 
                GameManager.State.CurrentDiceThrownNumber != 0 ? MovingMode.Normal : MovingMode.ThrowDice;
            GameManager.Selected.Element = null;

            //Destiny: Popup with choosing player shows
            if (GameManager.AdjacentPlayerIdToField(BoardManager.FindThief()).Count != 0)
            {
                GameManager.PopupManager.PopupsShown[PopupManager.THIEF_PLAYER_CHOICE_POPUP] = true;
            }
        }

        /// <summary>
        /// Blocks trade button if conditions were not satisfied
        /// </summary>
        private void TradeButtonActivity()
        {
            var val =  !(GameManager.State.MovingUserMode != MovingMode.Normal ||
                         GameManager.State.BasicMovingUserMode == BasicMovingMode.BuildPhase ||
                         GameManager.PopupManager.CheckIfWindowShown());
            tradeButton.interactable = val;
            tradeIcon.color = val ? tradeIconColor : Color.gray;
        }

        /// <summary>
        /// Blocks buy card button if buying conditions are not satisfied
        /// </summary>
        private void BuyCardButtonActivity()
        {
            if (!GameManager.PopupManager.CheckIfWindowShown() &&
                GameManager.State.CurrentDiceThrownNumber != 0 &&
                GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                GameManager.CardsManager.Deck.Count > 0)
            {
                var val = GameManager.State.Players[GameManager.State.CurrentPlayerId].CanBuyCard();
                buyCardButton.interactable = val;
                buyCardIcon.color = val ? buyCardIconColor : Color.gray;
            }
            else
            {
                buyCardButton.interactable = false;
                buyCardIcon.color = Color.gray;
            }
        }

        /// <summary>
        /// Blocks turn skip button if build conditions are not satisfied
        /// </summary>
        private void TurnSkipButtonActivity()
        {
            if (GameManager.PopupManager.CheckIfWindowShown() || 
                (GameManager.State.MovingUserMode != MovingMode.Normal && 
                GameManager.State.MovingUserMode != MovingMode.EndTurn))
            {
                turnSkipButton.interactable = false;
                turnSkipIcon.color = Color.gray;
                return;
            }

            // if player can build building for free
            if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst)
            {
                var val =  GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 1 &&
                           GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 1;
                turnSkipButton.interactable = val;
                turnSkipIcon.color = val ? turnSkipIconColor : Color.gray;
            }
            else if (GameManager.State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond)
            {
                var val =  GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetBuildingsNumber() == 2 &&
                           GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.GetPathsNumber() == 2;
                turnSkipButton.interactable = val;
                turnSkipIcon.color = val ? turnSkipIconColor : Color.gray;
            }
            // if the dice wasn't rolled
            else if (GameManager.State.CurrentDiceThrownNumber == 0)
            {
                turnSkipButton.interactable = false;
                turnSkipIcon.color = Color.gray;
            } 
            else
            {
                turnSkipButton.interactable = true;
                turnSkipIcon.color = turnSkipIconColor;
            }

            // if it's time to move the thief
            if (GameManager.State.MovingUserMode == MovingMode.MovingThief)
            {
                turnSkipButton.interactable = false;
                turnSkipIcon.color = Color.gray;
            }
        }

        /// <summary>
        /// Update text of turn skip button based on the fact if the game ended
        /// </summary>
        private void TurnSkipButtonText()
        {
            turnSkipButton.GetComponentInChildren<Text>().text = GameManager.EndGameCondition() ? endGameText : turnSkipText;
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
            cardsScrollrect.verticalNormalizedPosition = 1f;
            yield return null;
        }

        private void SetButtonsWithIcons()
        {
            turnSkipIcon.color = turnSkipIconColor;
            buyCardIcon.color = buyCardIconColor;
            tradeIcon.color = tradeIconColor;
            cancelIcon.color = cancelIconColor;
        }

        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
            buyCardButton.onClick.AddListener(OnBuyCardButton);
            tradeButton.onClick.AddListener(OnTradeButton);
            landTradeButton.onClick.AddListener(OnLandTradeButton);
            seaTradeButton.onClick.AddListener(OnSeaTradeButton);
            cancelButton.onClick.AddListener(OnCancelButton);
            
            SetButtonsWithIcons();

            if (GameManager.State.Mode == CatanMode.Basic)
                OnThrowDiceButton();
        }

        void Update()
        {
            if (GameManager.BuildManager.BuildRequests.Count > 0)
            {
                if (GameManager.BuildManager.BuildRequests.First())
                    InvokeBuildProcedure();
                GameManager.BuildManager.BuildRequests.RemoveAt(0);
            }
            
            if (GameManager.BuildManager.ThiefMoveRequests.Count > 0)
            {
                if (GameManager.BuildManager.ThiefMoveRequests.First())
                    InvokeMoveThiefProcedure();
                GameManager.BuildManager.ThiefMoveRequests.RemoveAt(0);
            }
            
            TradeButtonActivity();
            BuyCardButtonActivity();
            TurnSkipButtonActivity();
            TurnSkipButtonText();
        }
    }
}
