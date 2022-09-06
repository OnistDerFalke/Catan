using System;
using Board.States;
using DataStorage;
using UnityEngine;

namespace UI.Game
{
    public class SmartTabs : MonoBehaviour
    {
        private GameState.MovingMode lastMovingUserMode;
        private dynamic lastSelectedElement;
        private TabsUINavigation.ActiveContent lastActiveContent;
        
        private bool canReturnToDefaultTab;
        private bool smartPopupSetOnce;
       
        void Start()
        {
            //Destiny: Saving last set elements to check if variable has changed
            lastMovingUserMode = GameManager.State.MovingUserMode;
            lastSelectedElement = GameManager.Selected.Element;
            lastActiveContent = GetComponent<TabsUINavigation>().activeContent;
            smartPopupSetOnce = false;
            canReturnToDefaultTab = false;

            //Destiny: First invoke of smart tabs
            InvokeSmartActions();
        }

        void Update()
        {
            //Destiny: Lowest priority - smart tabs on user moving mode
            if (lastMovingUserMode != GameManager.State.MovingUserMode)
            {
                lastMovingUserMode = GameManager.State.MovingUserMode;
                InvokeSmartActions();
            }
            
            //Destiny: Average priority - smart tabs on selecting element to build
            if (lastSelectedElement != GameManager.Selected.Element)
            {
                lastSelectedElement = GameManager.Selected.Element;
                if(GameManager.Selected.Element != null)
                {
                    InvokeSmartElementInteraction();
                }
            }
            
            //Destiny: Highest priority - smart tabs on popups
            if (GameManager.PopupManager.CheckIfWindowShown())
            {
                if(!smartPopupSetOnce)
                {
                    InvokeSmartPricing();
                }
            }
            else
            {
                //Destiny: If it is set, tabs returns to content which was set before
                if (canReturnToDefaultTab)
                {
                    GetComponent<TabsUINavigation>().activeContent = TabsUINavigation.ActiveContent.None;
                    switch(lastActiveContent)
                    {
                        case TabsUINavigation.ActiveContent.Actions:
                            GetComponent<TabsUINavigation>().OnActionButtonClick();
                            break;
                        case TabsUINavigation.ActiveContent.Cards:
                            GetComponent<TabsUINavigation>().OnCardsButtonClick();
                            break;
                        case TabsUINavigation.ActiveContent.Pricing:
                            GetComponent<TabsUINavigation>().OnPricingButtonClick();
                            break;
                    }
                    canReturnToDefaultTab = false;
                }
                smartPopupSetOnce = false;
            }
        }

        /// <summary>
        /// Smart tabs opening actions tab on user mode
        /// </summary>
        private void InvokeSmartActions()
        {
            if (GameManager.State.MovingUserMode is
                GameState.MovingMode.ThrowDice or
                GameState.MovingMode.MovingThief or
                GameState.MovingMode.OnePathForFree or
                GameState.MovingMode.TwoPathsForFree or
                GameState.MovingMode.BuildPath ||
                (GameManager.State.BasicMovingUserMode is GameState.BasicMovingMode.TradePhase or 
                     GameState.BasicMovingMode.BuildPhase && 
                 GameManager.State.MovingUserMode == GameState.MovingMode.Normal))
            {
                GetComponent<TabsUINavigation>().activeContent = TabsUINavigation.ActiveContent.None;
                GetComponent<TabsUINavigation>().OnActionButtonClick();
            }
        }

        /// <summary>
        /// Smart tabs opening actions tab on selecting element
        /// </summary>
        private void InvokeSmartElementInteraction()
        {
            GetComponent<TabsUINavigation>().activeContent = TabsUINavigation.ActiveContent.None;
            GetComponent<TabsUINavigation>().OnActionButtonClick();
        }

        /// <summary>
        /// Smart tabs opening pricing tabs on popups
        /// </summary>
        private void InvokeSmartPricing()
        {
            //Destiny: Popups that need to have smart pricing
            if (GameManager.PopupManager.PopupsShown[GameManager.PopupManager.INVENTION_POPUP] ||
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.MONOPOL_POPUP] ||
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.SEA_TRADE_POPUP] ||
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.LAND_TRADE_ACCEPT_POPUP] ||
                GameManager.PopupManager.PopupsShown[GameManager.PopupManager.LAND_TRADE_POPUP])
            {
                smartPopupSetOnce = true;
                canReturnToDefaultTab = true;
                lastActiveContent = GetComponent<TabsUINavigation>().activeContent;
                GetComponent<TabsUINavigation>().activeContent = TabsUINavigation.ActiveContent.None;
                GetComponent<TabsUINavigation>().OnPricingButtonClick();
            }
        }
        
    }
}
