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

        private Resources.ResourceType resourceChosen;
        private Vector3 standardScale;

        void Start()
        {
            confirmButton.enabled = false;
            standardScale = clayButton.gameObject.transform.localScale;
            
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
            resourceChosen = Resources.ResourceType.None;
            ResetAllZooms();
        }

        /// <summary>
        /// Zooms only the chosen button
        /// </summary>
        /// <param name="button">Button to zoom</param>
        private void ZoomResourceButton(Button button)
        {
           ResetAllZooms();
           button.gameObject.transform.localScale = standardScale * 1.5f;
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
            GameManager.PopupsShown[GameManager.MONOPOL_POPUP] = false;

            GameManager.Players[GameManager.CurrentPlayer].properties.cards.MonopolCardEffect(resourceChosen);
        }
    }
}
