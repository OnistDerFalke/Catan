using System.Collections.Generic;
using static Player.Cards;

namespace Assets.Scripts.DataStorage.Managers
{
    public class PopupManager
    {
        //Destiny: Popups names
        public string MONOPOL_POPUP = "Monopol Popup";
        public string INVENTION_POPUP = "Invention Popup";
        public string THIEF_PAY_POPUP = "Thief Pay Popup";
        public string THIEF_PLAYER_CHOICE_POPUP = "Thief Player Choice Popup";
        public string BOUGHT_CARD_POPUP = "Bought Card Popup";
        public string LAND_TRADE_POPUP = "Land trade Popup";
        public string LAND_TRADE_ACCEPT_POPUP = "Land trade Accept Popup";
        public string SEA_TRADE_POPUP = "Sea trade Popup";
        public string END_GAME_POPUP = "End game Popup";

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
        /// <returns>True if window is shown</returns>
        public bool CheckIfWindowShown()
        {
            foreach (var popupShown in PopupsShown)
            {
                if (popupShown.Value)
                    return true;
            }

            return false;
        }
    }
}
