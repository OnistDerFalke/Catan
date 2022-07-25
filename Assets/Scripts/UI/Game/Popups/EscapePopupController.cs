using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class EscapePopupController : MonoBehaviour
    {
        [Header("Popup Windows Contents")][Space(5)]
        [Tooltip("Escape Popup Window")] [SerializeField] private GameObject escapePopupWindow;
        [Tooltip("Standard Escape Popup Content")] [SerializeField] private GameObject standardEscapePopupContent;
        [Tooltip("Real Escape Popup Content")] [SerializeField] private GameObject realEscapePopupContent;
        
        [Header("Standard Escape Control Buttons")][Space(5)]
        [Tooltip("Quit Game Button")] [SerializeField] private Button quitButton;
        [Tooltip("Cancel Button")] [SerializeField] private Button cancelButton;
        
        [Header("Quit Confirmation Escape Control Buttons")][Space(5)]
        [Tooltip("Really Quit Yes Button")] [SerializeField] private Button realQuitYesButton;
        [Tooltip("Really Quit No Button")] [SerializeField] private Button realQuitNoButton;

        void Start()
        {
            //Destiny: Assigning events on clicking buttons
            cancelButton.onClick.AddListener(HidePopup);
            quitButton.onClick.AddListener(ShowRealQuitInterface);
            realQuitYesButton.onClick.AddListener(GoToMainMenu);
            realQuitNoButton.onClick.AddListener(HidePopup);
        }
        void Update()
        {
            //Destiny: If escape clicked, show window
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Destiny: Show standard popup content
                escapePopupWindow.SetActive(true);
                realEscapePopupContent.SetActive(false);
                standardEscapePopupContent.SetActive(true);
            }
        }

        /// <summary>
        /// Hides popup
        /// </summary>
        private void HidePopup()
        {
            escapePopupWindow.SetActive(false);
        }

        /// <summary>
        /// Changes interface to real quit
        /// </summary>
        private void ShowRealQuitInterface()
        {
            standardEscapePopupContent.SetActive(false);
            realEscapePopupContent.SetActive(true);
        }

        /// <summary>
        /// Returns to main menu scene
        /// </summary>
        private void GoToMainMenu()
        {
            SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
        }
    }
}