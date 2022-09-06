using System.Collections;
using Camera.MainMenu;
using DataStorage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace UI.MainMenu.Navigation
{
    //Destiny: UI navigation for main menu
    public class MainMenuNavigation : MonoBehaviour
    {
        //Destiny: Menu buttons
        [Header("Menu buttons")][Space(5)]
        [Tooltip("Button directing to the first popup")][SerializeField]
        private Button startButton;
        [Tooltip("Button for loading game")][SerializeField] 
        private Button loadGameButton;
        [Tooltip("Button for settings")][SerializeField]
        private Button settingsButton;
        [Tooltip("Button for game exit")][SerializeField] 
        private Button exitButton;
    
        //Destiny: First popup buttons (basic settings)
        [Header("First popup buttons")][Space(5)]
        [Tooltip("Button for returning back to main menu")][SerializeField] 
        private Button backButton;
        [Tooltip("Button directing to the second popup")][SerializeField]
        private Button runGameButton;
        
        //Destiny: Second popup buttons (nicknames)
        [Header("Second popup buttons")][Space(5)]
        [Tooltip("Button for returning back to first popup")][SerializeField] 
        private Button backButton2;
        [Tooltip("Button for running the game")][SerializeField] 
        private Button finalAcceptButton;

        //Destiny: UI's
        [Header("UI's")][Space(5)]
        [Tooltip("Basic UI")][SerializeField]
        private GameObject basicContent;
        [Tooltip("First popup UI")][SerializeField]
        private GameObject dynamicGameSettings;
        [Tooltip("Second popup UI")][SerializeField]
        private GameObject dynamicPlayerNames;
        [Tooltip("Load Game UI")][SerializeField]
        private GameObject loadGameUI;
        [Tooltip("Settings UI")][SerializeField] 
        private GameObject settingsUI;

        //Destiny: Player input elements
        [Header("Player input elements")][Space(5)]
        [Tooltip("Button of 3 players choice")][SerializeField]
        private Button threePlayersButton;
        [Tooltip("Button of 4 players choice")][SerializeField]
        private Button fourPlayersButton;
        [Tooltip("Input error on second (nicknames) popup (showing if input is wrong)")][SerializeField] 
        private Text badNickErrorLabel;
        [Tooltip("Game mode dropdown")][SerializeField] 
        private TMP_Dropdown modeDropdown;
        [Tooltip("Players nicknames inputs")][SerializeField] 
        private TMP_InputField[] playerNamesInputs = new TMP_InputField[4];
        [Tooltip("Players colors inputs")][SerializeField] 
        private Button[] playerColorsInputs = new Button[4];
        
        //Destiny: Holders
        [Header("Holders")][Space(5)]
        [Tooltip("Camera zoom script holder")][SerializeField] 
        private GameObject zoomHolder;

        //Destiny: Colors of the buttons
        [Header("Button colors")][Space(5)]
        [Tooltip("Selected button color")][SerializeField] 
        private Sprite selectedButtonSprite;
        [Tooltip("Unselected button color")][SerializeField] 
        private Sprite unselectedButtonSprite;
        
        //Destiny: Colors settings
        private int[] playersColorsIndexes;
        private readonly Player.Player.Color[] availableColors = { 
            Player.Player.Color.Unset,
            Player.Player.Color.White, 
            Player.Player.Color.Yellow,
            Player.Player.Color.Blue,
            Player.Player.Color.Red
        };

        private int numberOfPlayers;
        
        void Start()
        {
            //Destiny: Default number of players is 3
            numberOfPlayers = 3;
            
            //Destiny: Binding buttons with it's features
            startButton.onClick.AddListener(OnStartButtonClick);
            loadGameButton.onClick.AddListener(OnLoadGameButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            exitButton.onClick.AddListener(OnExitButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
            backButton2.onClick.AddListener(OnBackButton2Click);
            runGameButton.onClick.AddListener(OnRunGameButtonClick);
            finalAcceptButton.onClick.AddListener(OnFinalAcceptButtonClick);
            threePlayersButton.onClick.AddListener(On3PlayersButtonClick);
            fourPlayersButton.onClick.AddListener(On4PlayersButtonClick);
            
            //Destiny: Binding color change buttons to right method
            for (var i = 0; i < playerColorsInputs.Length; i++)
            {
                var index = i;
                playerColorsInputs[i].onClick.AddListener(() => ChangePlayerColor(index));
            }

            //Destiny: Players colors are unset on start
            playersColorsIndexes = new []{1, 2, 3, 4};
            UpdatePlayerColor();
        }

        void Update()
        {
            SetPlayersNumberButtonsColors();
        }

        private void SetPlayersNumberButtonsColors()
        {
            switch (numberOfPlayers)
            {
                case 3:
                    threePlayersButton.GetComponent<Image>().sprite = selectedButtonSprite;
                    fourPlayersButton.GetComponent<Image>().sprite = unselectedButtonSprite;
                    break;
                case 4:
                    threePlayersButton.GetComponent<Image>().sprite = unselectedButtonSprite;
                    fourPlayersButton.GetComponent<Image>().sprite = selectedButtonSprite;
                    break;
            }
        }

        private void OnStartButtonClick()
        {
            //Destiny: Showing the first popup with zooming in animation
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(
                dynamicGameSettings, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        private void OnLoadGameButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(
                loadGameUI, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        private void OnSettingsButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(
                settingsUI, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }

        public void UnloadUIZoomAnimation()
        {
            StartCoroutine(WaitForAnimation(
                basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }

        private void OnBackButtonClick()
        {
            //Destiny: Hiding the first popup with zooming out animation 
            dynamicGameSettings.SetActive(false);
            StartCoroutine(WaitForAnimation(
                basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }

        private void On3PlayersButtonClick()
        {
            numberOfPlayers = 3;
        }

        private void On4PlayersButtonClick()
        {
            numberOfPlayers = 4;
        }
        private void OnBackButton2Click()
        {
            UpdatePlayerColor();
            
            //Destiny: Return to the first popup
            dynamicPlayerNames.SetActive(false);
            dynamicGameSettings.SetActive(true);
            
            //Destiny: Deactivating all second popup elements
            for (var i = 0; i < numberOfPlayers; i++)
            {
                playerNamesInputs[i].transform.parent.gameObject.SetActive(false);
            }
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
            for (var i = 0; i < numberOfPlayers; i++)
            {
                playerNamesInputs[i].transform.parent.gameObject.SetActive(true);
            }
        }
        
        private void OnFinalAcceptButtonClick()
        {
            //Destiny: Changing scene to the game with final animation
            if (!SetUpGameManager())
            {
                return;
            }

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
            GameManager.State.Players = new Player.Player[numberOfPlayers];

            for (var i = 0; i < GameManager.State.Players.Length; i++)
            {
                //Destiny: If player did not set color, show error
                if (availableColors[playersColorsIndexes[i]] == Player.Player.Color.Unset)
                {
                    badNickErrorLabel.text = "Gracz " + (i + 1) + " nie ma ustawionego koloru!";
                    return false;
                }
                
                //Destiny: If player did not set a nickname, set default name
                if (playerNamesInputs[i].text == "")
                {
                    GameManager.State.Players[i] = new Player.Player(
                        i, "Gracz " + (i+1), availableColors[playersColorsIndexes[i]]);
                    continue;
                }
                    
                //Destiny: Show error when nickname was too short
                if (playerNamesInputs[i].text.Length < 3)
                {
                    badNickErrorLabel.text = "Nazwa gracza " + (i + 1) + " jest za krótka!";
                    return false;
                }
                
                GameManager.State.Players[i] = new Player.Player(
                    i, playerNamesInputs[i].text, availableColors[playersColorsIndexes[i]]);
            }

            GameManager.Setup(modeDropdown.options[modeDropdown.value].text);
            return true;
        }

        /// <summary>
        /// Changes player color
        /// </summary>
        /// <param name="playerIndex">Index of player to update color</param>
        private void ChangePlayerColor(int playerIndex)
        {
            playersColorsIndexes[playerIndex] = (playersColorsIndexes[playerIndex] + 1) % availableColors.Length;
            for (var i = 0; i < playersColorsIndexes.Length; i++)
            {
                if (i == playerIndex)
                {
                    continue;
                }

                if (playersColorsIndexes[playerIndex] == 0)
                {
                    continue;
                }

                if (playersColorsIndexes[i] == playersColorsIndexes[playerIndex])
                {
                    ChangePlayerColor(playerIndex);
                }
            }
            UpdatePlayerColor();
        }

        /// <summary>
        /// Updates players colors on buttons
        /// </summary>
        private void UpdatePlayerColor()
        {
            for (var i = 0; i < playerColorsInputs.Length; i++)
            {
                playerColorsInputs[i].GetComponent<Image>().color =
                    GameManager.GetColorByPlayerColor(availableColors[playersColorsIndexes[i]]);
            }
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
