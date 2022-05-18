using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI.Game
{
    /// <summary>
    /// Class for updating the content in static UI on the down part of the interface
    /// </summary>
    public class StaticUIUpdater : MonoBehaviour
    {
        [Header("Current Player UI")][Space(5)]
        [Tooltip("Current player color image")] [SerializeField]
        private Image playerColorImage;
        [Tooltip("Current player name text")] [SerializeField]
        private Text playerNameText;

        void Update()
        {
            UpdateColorImage();
            UpdateCurrentPlayerName();
        }

        /// <summary>
        /// Updates color of the current player on the UI
        /// </summary>
        private void UpdateColorImage()
        {
            playerColorImage.color = GameManager.Players[GameManager.CurrentPlayer].color switch
            {
                Player.Player.Color.Blue => Color.blue,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.White => Color.white,
                Player.Player.Color.Yellow => Color.yellow,
                _ => playerColorImage.color
            };
        }

        /// <summary>
        /// Updates the name of the current player on the UI
        /// </summary>
        private void UpdateCurrentPlayerName()
        {
            playerNameText.text = GameManager.Players[GameManager.CurrentPlayer].name;
        }
    }
}