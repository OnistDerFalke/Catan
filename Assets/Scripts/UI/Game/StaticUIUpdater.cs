using Board;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Assets.Scripts.Board.States;
using static Assets.Scripts.Board.States.FieldState;
using static Assets.Scripts.Board.States.JunctionState;
using DataStorage;

namespace UI.Game
{
    /// <summary>
    /// Class for updating the content in static UI on the down part of the interface
    /// </summary>
    public class StaticUIUpdater : MonoBehaviour
    {
        //Destiny: Current player info
        [Header("Current Player UI")][Space(5)]
        [Tooltip("Current player color image")] [SerializeField] private Image playerColorImage;
        [Tooltip("Current player name text")] [SerializeField] private Text playerNameText;
        
        //Destiny: Selected element with additional info
        [Header("Selected Element UI")][Space(5)]
        [Tooltip("Selected element owner text")] [SerializeField] private Text selectedElementAdditionalInfo;
        [Tooltip("Selected element name text")] [SerializeField] private Text selectedElementName;

        //Destiny: Current player's resources info
        [Header("Resources")][Space(5)]
        [Tooltip("Wood resource text")] [SerializeField] private Text woodResourceText;
        [Tooltip("Clay resource text")] [SerializeField] private Text clayResourceText;
        [Tooltip("Wool resource text")] [SerializeField] private Text woolResourceText;
        [Tooltip("Iron resource text")] [SerializeField] private Text ironResourceText;
        [Tooltip("Wheat resource text")] [SerializeField] private Text wheatResourceText;
        
        //Destiny: Board elements names shown in UI
        [Header("Board elements names")][Space(5)]
        [Tooltip("Empty junction name")] [SerializeField] private string junctionEmptyName;
        [Tooltip("Village junction name")] [SerializeField] private string junctionVillageName;
        [Tooltip("City junction name")] [SerializeField] private string junctionCityName;
        [Tooltip("Path name")] [SerializeField] private string pathName;
        [Tooltip("Clay field name")] [SerializeField] private string clayFieldName;
        [Tooltip("Desert field name")] [SerializeField] private string desertFieldName;
        [Tooltip("Field field name")] [SerializeField] private string fieldFieldName;
        [Tooltip("Forest field name")] [SerializeField] private string forestFieldName;
        [Tooltip("Mountains field name")] [SerializeField] private string mountainsFieldName;
        [Tooltip("Pasture field name")] [SerializeField] private string pastureFieldName;

        //Destiny: When we point a field, it gives info which resource is supplied by this field
        [Header("Supplied resources names")][Space(5)]
        [Tooltip("Supplied prefix")] [SerializeField] private string suppliedPrefix;
        [Tooltip("Wood")] [SerializeField] private string suppliedWood;
        [Tooltip("Wheat")] [SerializeField] private string suppliedWheat;
        [Tooltip("Clay")] [SerializeField] private string suppliedClay;
        [Tooltip("Iron")] [SerializeField] private string suppliedIron;
        [Tooltip("Wool")] [SerializeField] private string suppliedWool;
        [Tooltip("Supplies nothing")] [SerializeField] private string suppliedNothing;
        
        [Header("Element owner")][Space(5)]
        [Tooltip("Owner prefix")] [SerializeField] private string ownerPrefix;
        [Tooltip("No owner")] [SerializeField] private string noOwner;

        
        void Update()
        {
            //Destiny: Updates all info in UI
            UpdateSelectedElement();
            UpdateCurrentPlayer();
            UpdateResources();
        }

        /// <summary>
        /// Updates all of resources numbers that player has
        /// </summary>
        private void UpdateResources()
        {
            var resources = GameManager.State.Players[GameManager.State.CurrentPlayerId].resources.GetResourcesNumber();

            woodResourceText.text = resources[Player.Resources.ResourceType.Wood].ToString();
            clayResourceText.text = resources[Player.Resources.ResourceType.Clay].ToString();
            woolResourceText.text = resources[Player.Resources.ResourceType.Wool].ToString();
            ironResourceText.text = resources[Player.Resources.ResourceType.Iron].ToString();
            wheatResourceText.text = resources[Player.Resources.ResourceType.Wheat].ToString();
        }
        
        /// <summary>
        /// Updates color and name of the current player on the UI
        /// </summary>
        private void UpdateCurrentPlayer()
        {
            playerNameText.text = GameManager.State.Players[GameManager.State.CurrentPlayerId].name;
            playerColorImage.color = GameManager.State.Players[GameManager.State.CurrentPlayerId].color switch
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
                selectedElementName.text = "";
                selectedElementAdditionalInfo.text = "";
                return;
            }
            
            //Destiny: Setting the object text
            if (GameManager.Selected.Pointed as FieldElement != null)
            {
                var element = (FieldElement)GameManager.Selected.Pointed;
                selectedElementName.text = ((FieldState)element.State).type switch
                {
                    FieldType.Forest => forestFieldName,
                    FieldType.Pasture => pastureFieldName,
                    FieldType.Field => fieldFieldName,
                    FieldType.Hills => clayFieldName,
                    FieldType.Mountains => mountainsFieldName,
                    FieldType.Desert => desertFieldName,
                    _ => selectedElementName.text
                };
            }
            else if (GameManager.Selected.Pointed as JunctionElement != null)
            {
                var element = (JunctionElement)GameManager.Selected.Pointed;
                selectedElementName.text = ((JunctionState)element.State).type switch
                {
                    JunctionType.None => junctionEmptyName,
                    JunctionType.Village => junctionVillageName,
                    JunctionType.City => junctionCityName,
                    _ => selectedElementName.text
                };
            }
            else if (GameManager.Selected.Pointed as PathElement != null)
            {
                selectedElementName.text = pathName;
            }
            
            //Destiny: Setting additional info
            selectedElementAdditionalInfo.text = "";
            foreach (var player in GameManager.State.Players)
            {
                //Destiny: For field additional info is which resource it supplies
                if (GameManager.Selected.Pointed as FieldElement != null)
                {
                    var element = (FieldElement)GameManager.Selected.Pointed;
                    selectedElementAdditionalInfo.text = ((FieldState)element.State).type switch
                    {
                        FieldType.Forest => suppliedPrefix + suppliedWood,
                        FieldType.Pasture => suppliedPrefix + suppliedWool,
                        FieldType.Field => suppliedPrefix + suppliedWheat,
                        FieldType.Hills => suppliedPrefix + suppliedClay,
                        FieldType.Mountains => suppliedPrefix + suppliedIron,
                        FieldType.Desert => suppliedNothing,
                        _ => selectedElementAdditionalInfo.text
                    };
                    break;
                }
                
                //Destiny: For path and junction additional info is an owner
                if (GameManager.Selected.Pointed as JunctionElement != null)
                {
                    var element = (JunctionElement)GameManager.Selected.Pointed;
                    if (!player.OwnsBuilding(element.State.id))
                    {
                        selectedElementAdditionalInfo.text = noOwner;
                        continue;
                    }
                    selectedElementAdditionalInfo.text = ownerPrefix + player.name;
                    break;
                }
                if (GameManager.Selected.Pointed as PathElement != null)
                {
                    var element = (PathElement)GameManager.Selected.Pointed;
                    if (!player.OwnsPath(element.State.id))
                    {
                        selectedElementAdditionalInfo.text = noOwner;
                        continue;
                    }
                    selectedElementAdditionalInfo.text = ownerPrefix + player.name;
                    break;
                }
            }
        }
    }
}