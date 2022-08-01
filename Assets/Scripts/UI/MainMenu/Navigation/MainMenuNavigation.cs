using System.Collections;
using Camera.MainMenu;
using DataStorage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

namespace UI.MainMenu.Navigation
{
    //Destiny: UI navigation for main menu
    public class MainMenuNavigation : MonoBehaviour
    {
        //Destiny: Menu buttons
        [Header("Menu buttons")][Space(5)]
        [Tooltip("Button directing to the first popup")]
        [SerializeField] private Button startButton;
        [Tooltip("Button for loading game")]
        [SerializeField] private Button loadGameButton;
        [Tooltip("Button for settings")]
        [SerializeField] private Button settingsButton;
        [Tooltip("Button for game exit")]
        [SerializeField] private Button exitButton;
    
        //Destiny: First popup buttons (basic settings)
        [Header("First popup buttons")][Space(5)]
        [Tooltip("Button for returning back to main menu")]
        [SerializeField] private Button backButton;
        [Tooltip("Button directing to the second popup")]
        [SerializeField] private Button runGameButton;
        
        //Destiny: Second popup buttons (nicknames)
        [Header("Second popup buttons")][Space(5)]
        [Tooltip("Button for returning back to first popup")]
        [SerializeField] private Button backButton2;
        [Tooltip("Button for running the game")]
        [SerializeField] private Button finalAcceptButton;

        //Destiny: UI's
        [Header("UI's")][Space(5)]
        [Tooltip("Basic UI")]
        [SerializeField] private GameObject basicContent;
        [Tooltip("First popup UI")]
        [SerializeField] private GameObject dynamicGameSettings;
        [Tooltip("Second popup UI")]
        [SerializeField] private GameObject dynamicPlayerNames;
        [Tooltip("Load Game UI")]
        [SerializeField] private GameObject loadGameUI;
        [Tooltip("Settings UI")]
        [SerializeField] private GameObject settingsUI;

        //Destiny: Player input elements
        [Header("Player input elements")][Space(5)]
        [Tooltip("Slider with number of players")]
        [SerializeField] private Slider playersNumberSlider;
        [Tooltip("Number corresponding to the slider of players number")]
        [SerializeField] private Text playersNumber;
        [Tooltip("Input error on second (nicknames) popup (showing if input is wrong)")]
        [SerializeField] private Text badNickErrorLabel;
        [Tooltip("Game mode dropdown")]
        [SerializeField] private TMP_Dropdown modeDropdown;
        [Tooltip("Players nicknames inputs")]
        [SerializeField] private TMP_InputField[] playerNamesInputs = new TMP_InputField[4];
        
        //Destiny: Holders
        [Header("Holders")][Space(5)]
        [Tooltip("Camera zoom script holder")]
        [SerializeField] private GameObject zoomHolder;
    
        void Start()
        {
            //Destiny: Binding buttons with it's features
            startButton.onClick.AddListener(OnStartButtonClick);
            loadGameButton.onClick.AddListener(OnLoadGameButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            exitButton.onClick.AddListener(OnExitButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
            backButton2.onClick.AddListener(OnBackButton2Click);
            runGameButton.onClick.AddListener(OnRunGameButtonClick);
            finalAcceptButton.onClick.AddListener(OnFinalAcceptButtonClick);
        }

        void Update()
        {
            //Destiny: Updating number next to the players number slider
            playersNumber.text = playersNumberSlider.value.ToString();
        }
        
        private void OnStartButtonClick()
        {
            //Destiny: Showing the first popup with zooming in animation
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(dynamicGameSettings, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        private void OnLoadGameButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(loadGameUI, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        private void OnSettingsButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(settingsUI, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        public void UnloadUIZoomAnimation()
        {
            StartCoroutine(WaitForAnimation(basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }

        private void OnBackButtonClick()
        {
            //Destiny: Hiding the first popup with zooming out animation 
            dynamicGameSettings.SetActive(false);
            StartCoroutine(WaitForAnimation(basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }
    
        private void OnBackButton2Click()
        {
            //Destiny: Return to the first popup
            dynamicPlayerNames.SetActive(false);
            dynamicGameSettings.SetActive(true);
            
            //Destiny: Deactivating all second popup elements
            for (var i = 0; i < (int) playersNumberSlider.value; i++)
                playerNamesInputs[i].transform.parent.gameObject.SetActive(false);
        }

        private void OnExitButtonClick()
        {
            //Destiny: Exit the application
            Application.Quit();
        }
    
        private void OnRunGameButtonClick()
        {
            //Destiny: Changing from first to second popup
            dynamicGameSettings.SetActive(false);
            dynamicPlayerNames.SetActive(true);
            
            //Destiny: Activating players names inputs corresponding to number of players
            for (var i = 0; i < (int) playersNumberSlider.value; i++)
                playerNamesInputs[i].transform.parent.gameObject.SetActive(true);
        }
        
        private void OnFinalAcceptButtonClick()
        {
            //Destiny: Changing scene to the game with final animation
            if (!SetUpGameManager()) 
                return;
            dynamicPlayerNames.SetActive(false);
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.FinalZoom);
            zoomHolder.GetComponent<MenuCameraMove>().SetActive(false);
            StartCoroutine(WaitForGameStart(zoomHolder.GetComponent<MenuCameraZoom>().runGameAnimationDelay));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if all information were correct to set it to game manager and start the game;
        /// False if setting up information to game manager went wrong (there were problems in input)</returns>
        private bool SetUpGameManager()
        {
            GameManager.LoadingGame = false;
            badNickErrorLabel.text = "";
            GameManager.State.Players = new Player.Player[(int)playersNumberSlider.value];

            for (var i = 0; i < GameManager.State.Players.Length; i++)
            {
                //Destiny: If player did not set a nickname, set default name
                if (playerNamesInputs[i].text == "")
                {
                    GameManager.State.Players[i] = new Player.Player(i, "Gracz " + (i+1));
                    continue;
                }
                    
                //Destiny: Show error when nickname was too short
                if (playerNamesInputs[i].text.Length < 3)
                {
                    badNickErrorLabel.text = "Nazwa gracza " + (i + 1) + " jest za krótka!";
                    return false;
                }
                
                GameManager.State.Players[i] = new Player.Player(i, playerNamesInputs[i].text);
            }

            GameManager.Setup(modeDropdown.options[modeDropdown.value].text);
            return true;
        }

        /// <summary>
        /// Asynchronous waiting for camera animation play and showing the UI
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator WaitForAnimation(GameObject ui, float delay)
        {
            yield return new WaitForSeconds(delay);
            ui.SetActive(true);
        }

        /// <summary>
        /// Asynchronous waiting for camera animation play and changing the scene
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        IEnumerator WaitForGameStart(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Scenes/GameScreen", LoadSceneMode.Single);
        }
    }
}
