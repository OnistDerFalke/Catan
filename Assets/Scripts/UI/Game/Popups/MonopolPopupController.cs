using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using Resources = Player.Resources;

namespace UI.Game.Popups
{
    public class MonopolPopupController : MonoBehaviour
    {
        [Header("Resources Buttons")][Space(5)]
        [Tooltip("Clay Button")]
        [SerializeField] private Button clayButton;
        [Tooltip("Iron Button")]
        [SerializeField] private Button ironButton;
        [Tooltip("Wheet Button")]
        [SerializeField] private Button wheatButton;
        [Tooltip("Wood Button")]
        [SerializeField] private Button woodButton;
        [Tooltip("Wool Button")]
        [SerializeField] private Button woolButton;
        
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")]
        [SerializeField] private Button confirmButton;
        
        [Header("Scale Properties")][Space(5)]
        [Tooltip("Standard Scale")]
        [SerializeField] private Vector3 standardScale;
        [Tooltip("Selected Scale Multiplier")]
        [SerializeField] private float selectedScaleMultiplier;

        private Resources.ResourceType resourceChosen;

        void Start()
        {
            //Destiny: Confirm button is blocked on start
            confirmButton.enabled = false;

            //Destiny: Choosing the resource
            clayButton.onClick.AddListener(() =>
            {
                resourceChosen = Resources.ResourceType.Clay;
                ZoomResourceButton(clayButton);
                confirmButton.enabled = true;
            });
            ironButton.onClick.AddListener(() =>
            {
                resourceChosen = Resources.ResourceType.Iron;
                ZoomResourceButton(ironButton);
                confirmButton.enabled = true;
            });
            wheatButton.onClick.AddListener(() =>
            {
                resourceChosen = Resources.ResourceType.Wheat;
                ZoomResourceButton(wheatButton);
                confirmButton.enabled = true;
            });
            woodButton.onClick.AddListener(() =>
            {
                resourceChosen = Resources.ResourceType.Wood;
                ZoomResourceButton(woodButton);
                confirmButton.enabled = true;
            });
            woolButton.onClick.AddListener(() =>
            {
                resourceChosen = Resources.ResourceType.Wool;
                ZoomResourceButton(woolButton);
                confirmButton.enabled = true;
            });
            
            //Destiny: Choice confirmation
            confirmButton.onClick.AddListener(OnConfirmButton);
        }

        void OnEnable()
        {
            //Destiny: Reset all choices in popup
            ResetAllZooms();
        }

        /// <summary>
        /// Zooms only the chosen button
        /// </summary>
        /// <param name="button">Button to zoom</param>
        private void ZoomResourceButton(Button button)
        {
           ResetAllZooms();
           button.gameObject.transform.localScale = standardScale * selectedScaleMultiplier;
        }
        
        private void ResetAllZooms()
        {
            clayButton.gameObject.transform.localScale = standardScale;
            ironButton.gameObject.transform.localScale = standardScale;
            wheatButton.gameObject.transform.localScale = standardScale;
            woodButton.gameObject.transform.localScale = standardScale;
            woolButton.gameObject.transform.localScale = standardScale;
        }
        
        private void OnConfirmButton()
        {
            confirmButton.enabled = false;
            GameManager.PopupManager.PopupsShown[GameManager.PopupManager.MONOPOL_POPUP] = false;

            GameManager.State.Players[GameManager.State.CurrentPlayerId].properties.cards.MonopolCardEffect(resourceChosen);
        }
    }
}
