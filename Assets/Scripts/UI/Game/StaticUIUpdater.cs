using Board;
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
        
        [Header("Selected Element UI")][Space(5)]
        [Tooltip("Selected element owner text")] [SerializeField]
        private Text selectedElementOwner;
        [Tooltip("Selected element name text")] [SerializeField]
        private Text selectedElementName;

        void Update()
        {
            UpdateSelectedElement();
            UpdateCurrentPlayer();
        }

        /// <summary>
        /// Updates color of the current player on the UI
        /// </summary>
        private void UpdateCurrentPlayer()
        {
            playerNameText.text = GameManager.Players[GameManager.CurrentPlayer].name;
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
        /// Updates UI information about selected element
        /// </summary>
        private void UpdateSelectedElement()
        {
            if (!GameManager.Selected.IsSelected)
            {
                selectedElementName.text = "Nie wybrano";
                selectedElementOwner.text = "Brak";
                return;
            }
            
            //Destiny: Setting the object text
            selectedElementName.text = GameManager.Selected.Type switch
            {
                BoardElement.BoardElementType.Path => "Droga",
                BoardElement.BoardElementType.Junction => GameManager.Selected.SelectedJunction.type switch
                {
                    JunctionElement.JunctionType.None => "Puste skrzyżowanie",
                    JunctionElement.JunctionType.Village => "Wioska",
                    JunctionElement.JunctionType.City => "Miasto",
                    _ => selectedElementName.text
                },
                _ => selectedElementName.text
            };

            //Destiny: Setting the owner name
            foreach (var player in GameManager.Players)
            {
                if (GameManager.Selected.Type == BoardElement.BoardElementType.Junction)
                {
                    if (!player.OwnsBuilding(GameManager.Selected.SelectedJunction.id)) continue;
                    selectedElementOwner.text = player.name;
                    break;
                }
                
                if (GameManager.Selected.Type == BoardElement.BoardElementType.Path)
                {
                    if (!player.OwnsPath(GameManager.Selected.SelectedPath.id)) continue;
                    selectedElementOwner.text = player.name;
                    break;
                }
            }
        }
    }
}