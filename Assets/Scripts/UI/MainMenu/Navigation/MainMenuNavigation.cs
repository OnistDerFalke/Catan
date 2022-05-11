using System.Collections;
using Camera.MainMenu;
using DataStorage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
            if (!SetUpGameManager()) return;
            dynamicPlayerNames.SetActive(false);
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.FinalZoom);
            zoomHolder.GetComponent<MenuCameraMove>().SetActive(false);
            StartCoroutine(WaitForGameStart(zoomHolder.GetComponent<MenuCameraZoom>().runGameAnimationDelay));
        }
        
        /*
           Returns:
           bool -> false if setting up information to game manager went wrong (there were problems in input)
                -> true if all information were correct to set it to game manager and start the game
       */
        private bool SetUpGameManager()
        {
            badNickErrorLabel.text = "";
            
            //Destiny: Setting up game manager basic information based on popups inputs
            GameManager.Mode = modeDropdown.options[modeDropdown.value].text == "PODSTAWOWY" ? 
                GameManager.CatanMode.Basic : GameManager.CatanMode.Advanced;
            GameManager.PlayersNumber = (int)playersNumberSlider.value;
            GameManager.PlayersNames = new string[GameManager.PlayersNumber];
            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
                //Destiny: Show error when nickname was too short
                if (playerNamesInputs[i].text.Length < 3)
                {
                    badNickErrorLabel.text = "Nazwa gracza " + (i + 1) + " jest za krótka!";
                    return false;
                }
                GameManager.PlayersNames[i] = playerNamesInputs[i].text;
            }
            return true;
        }
    
        //Destiny: Asynchronous waiting for camera animation play and showing the UI
        IEnumerator WaitForAnimation(GameObject ui, float delay)
        {
            yield return new WaitForSeconds(delay);
            ui.SetActive(true);
        }
    
        //Destiny: Asynchronous waiting for camera animation play and changing the scene
        IEnumerator WaitForGameStart(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Scenes/GameScreen", LoadSceneMode.Single);
        }
    }
}
