using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Navigation
{
    public class SettingsUIController : MonoBehaviour
    {
        //Destiny: Abort, approve and default buttons
        [Header("Control Buttons")][Space(5)]
        [Tooltip("Approve Button")] [SerializeField] private Button approveButton;
        [Tooltip("Abort Button")] [SerializeField] private Button abortButton;
        [Tooltip("Default Button")] [SerializeField] private Button defaultButton;
        
        //Destiny: Elements in screen settings context
        [Header("Screen Settings Context Elements")][Space(5)]
        [Tooltip("Resolution Dropdown")] [SerializeField] private TMP_Dropdown resolutionDropdown;
        [Tooltip("Fullscreen Dropdown")] [SerializeField] private TMP_Dropdown fullscreenDropdown;
        
        //Destiny: Main Menu Navigation script holder
        [Header("Main Menu Navigation script holder")][Space(5)]
        [Tooltip("Main Menu Navigation script holder")] [SerializeField] private MainMenuNavigation mmnHolder;
       
        void Start()
        {
            //Destiny: Features on click
            approveButton.onClick.AddListener(OnApproveButton);
            abortButton.onClick.AddListener(OnAbortButton);
            defaultButton.onClick.AddListener(OnDefaultButton);
        }

        void OnEnable()
        {
            //Destiny: Update labels on window show
            UpdateLabels();
        }

        /// <summary>
        /// Event on clicking approve button
        /// </summary>
        private void OnApproveButton()
        {
            SetupScreen();
            mmnHolder.UnloadUIZoomAnimation();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Event on clicking abort button
        /// </summary>
        private void OnAbortButton()
        {
            mmnHolder.UnloadUIZoomAnimation();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Event on clicking default button
        /// </summary>
        private void OnDefaultButton()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        /// <summary>
        /// Sets screen resolution and fullscreen mode
        /// </summary>
        private void SetupScreen()
        {
            var isFullScreen = fullscreenDropdown.options[fullscreenDropdown.value].text == "PEŁNY EKRAN";
            switch (resolutionDropdown.options[resolutionDropdown.value].text)
            {
                case "1920 x 1080":
                    Screen.SetResolution(1920, 1080, isFullScreen);
                    break;
                case "1366 x 768":
                    Screen.SetResolution(1366, 768, isFullScreen);
                    break;
                case "2560 x 1440":
                    Screen.SetResolution(2560, 1440, isFullScreen);
                    break;
                case "3840 x 2160":
                    Screen.SetResolution(3840, 2160, isFullScreen);
                    break;
                case "1280 x 800":
                    Screen.SetResolution(1280, 800, isFullScreen);
                    break;
                case "1920 x 1200":
                    Screen.SetResolution(1920, 1200, isFullScreen);
                    break;
                case "800 x 600":
                    Screen.SetResolution(800, 600, isFullScreen);
                    break;
                case "1024 x 768":
                    Screen.SetResolution(1024, 768, isFullScreen);
                    break;
                case "1600 x 1200":
                    Screen.SetResolution(1600, 1200, isFullScreen);
                    break;
            }
        }

        /// <summary>
        /// Updates labels of chosen dropdown options
        /// </summary>
        private void UpdateLabels()
        {
            //Destiny: Update labels on Settings Dropdowns
            resolutionDropdown.options[resolutionDropdown.value].text = $"{Screen.width} x {Screen.height}";
            fullscreenDropdown.options[fullscreenDropdown.value].text = Screen.fullScreen ? "PEŁNY EKRAN" : "OKNO";
        }
    }
}
