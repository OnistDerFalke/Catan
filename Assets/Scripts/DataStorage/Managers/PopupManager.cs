using System.Collections.Generic;
using static Player.Cards;

namespace Assets.Scripts.DataStorage.Managers
{
    public class PopupManager
    {
        //Destiny: Popups names
        public const string MONOPOL_POPUP = "Monopol Popup";
        public const string INVENTION_POPUP = "Invention Popup";
        public const string THIEF_PAY_POPUP = "Thief Pay Popup";
        public const string THIEF_PLAYER_CHOICE_POPUP = "Thief Player Choice Popup";
        public const string BOUGHT_CARD_POPUP = "Bought Card Popup";
        public const string LAND_TRADE_POPUP = "Land trade Popup";
        public const string LAND_TRADE_ACCEPT_POPUP = "Land trade Accept Popup";
        public const string SEA_TRADE_POPUP = "Sea trade Popup";
        public const string END_GAME_POPUP = "End game Popup";

        //Destiny: Popups flow control (if popup is shown or not)
        public Dictionary<string, bool> PopupsShown;

        //Destiny: Some things that need to be passed to popups
        public CardType LastBoughtCard;
        public float PopupOffset;

        public void Setup()
        {
            PopupsShown = new();

            PopupsShown.Add(MONOPOL_POPUP, false);
            PopupsShown.Add(INVENTION_POPUP, false);
            PopupsShown.Add(THIEF_PAY_POPUP, false);
            PopupsShown.Add(THIEF_PLAYER_CHOICE_POPUP, false);
            PopupsShown.Add(BOUGHT_CARD_POPUP, false);
            PopupsShown.Add(LAND_TRADE_POPUP, false);
            PopupsShown.Add(LAND_TRADE_ACCEPT_POPUP, false);
            PopupsShown.Add(SEA_TRADE_POPUP, false);
            PopupsShown.Add(END_GAME_POPUP, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if any window is shown</returns>
        public bool CheckIfWindowShown()
        {
            foreach (var popupShown in PopupsShown)
            {
                if (popupShown.Value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if any windows that needs to have smart pricing is shown</returns>
        public bool CheckIfWindowWithSmartTabsShown()
        {
            return PopupsShown[INVENTION_POPUP] || PopupsShown[MONOPOL_POPUP] || PopupsShown[SEA_TRADE_POPUP] ||
                PopupsShown[LAND_TRADE_ACCEPT_POPUP] || PopupsShown[LAND_TRADE_POPUP];
        }
    }
}
