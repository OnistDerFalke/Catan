using System;
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

        [Header("Resources")][Space(5)]
        [Tooltip("Wood resource text")] [SerializeField]
        private Text woodResourceText;
        [Tooltip("Clay resource text")] [SerializeField]
        private Text clayResourceText;
        [Tooltip("Wool resource text")] [SerializeField]
        private Text woolResourceText;
        [Tooltip("Iron resource text")] [SerializeField]
        private Text ironResourceText;
        [Tooltip("Wheat resource text")] [SerializeField]
        private Text wheatResourceText;
        
        void Update()
        {
            UpdateSelectedElement();
            UpdateCurrentPlayer();
            UpdateResources();
        }

        /// <summary>
        /// Updates all of resources numbers that player has
        /// </summary>
        private void UpdateResources()
        {
            woodResourceText.text = GameManager.Players[GameManager.CurrentPlayer].resources
                .GetResourceNumber(Player.Resources.ResourceType.Wood).ToString();
            clayResourceText.text = GameManager.Players[GameManager.CurrentPlayer].resources
                .GetResourceNumber(Player.Resources.ResourceType.Clay).ToString();
            woolResourceText.text = GameManager.Players[GameManager.CurrentPlayer].resources
                .GetResourceNumber(Player.Resources.ResourceType.Wool).ToString();
            ironResourceText.text = GameManager.Players[GameManager.CurrentPlayer].resources
                .GetResourceNumber(Player.Resources.ResourceType.Iron).ToString();
            wheatResourceText.text = GameManager.Players[GameManager.CurrentPlayer].resources
                .GetResourceNumber(Player.Resources.ResourceType.Wheat).ToString();
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
            if (GameManager.Selected.Pointed == null)
            {
                selectedElementName.text = "Nie wybrano";
                selectedElementOwner.text = "Brak";
                return;
            }
            
            //Destiny: Setting the object text
            if (GameManager.Selected.Pointed as FieldElement != null)
            {
                var element = (FieldElement) GameManager.Selected.Pointed;
                selectedElementName.text = element.type switch
                {
                    FieldElement.FieldType.Forest => "Las",
                    FieldElement.FieldType.Pasture => "Pastwisko",
                    FieldElement.FieldType.Field => "Pole uprawne",
                    FieldElement.FieldType.Hills => "Złoże miedzi",
                    FieldElement.FieldType.Mountains => "Góry",
                    FieldElement.FieldType.Desert => "Pustynia",
                    _ => selectedElementName.text
                };
            }
            else if (GameManager.Selected.Pointed as JunctionElement != null)
            {
                var element = (JunctionElement) GameManager.Selected.Pointed;
                selectedElementName.text = element.type switch
                {
                    JunctionElement.JunctionType.None => "Puste skrzyżowanie",
                    JunctionElement.JunctionType.Village => "Wioska",
                    JunctionElement.JunctionType.City => "Miasto",
                    _ => selectedElementName.text
                };
            }
            else if (GameManager.Selected.Pointed as PathElement != null)
            {
                selectedElementName.text = "Droga";
            }
            
            //Destiny: Setting additional info
            selectedElementOwner.text = "Brak";
            foreach (var player in GameManager.Players)
            {
                if (GameManager.Selected.Pointed as FieldElement != null)
                {
                    var element = (FieldElement) GameManager.Selected.Pointed;
                    selectedElementOwner.text = element.type switch
                    {
                        FieldElement.FieldType.Forest => "Dostarcza: Drewno",
                        FieldElement.FieldType.Pasture => "Dostarcza: Wełna",
                        FieldElement.FieldType.Field => "Dostarcza: Zboże",
                        FieldElement.FieldType.Hills => "Dostarcza: Glina",
                        FieldElement.FieldType.Mountains => "Dostarcza: Żelazo",
                        FieldElement.FieldType.Desert => "Nie dostarcza surowców",
                        _ => selectedElementOwner.text
                    };
                    break;
                }
                
                if (GameManager.Selected.Pointed as JunctionElement != null)
                {
                    var element = (JunctionElement) GameManager.Selected.Pointed;
                    if (!player.OwnsBuilding(element.id)) continue;
                    selectedElementOwner.text = player.name;
                    break;
                }
                
                if (GameManager.Selected.Pointed as PathElement != null)
                {
                    var element = (PathElement) GameManager.Selected.Pointed;
                    if (!player.OwnsPath(element.id)) continue;
                    selectedElementOwner.text = player.name;
                    break;
                }
            }
        }
    }
}