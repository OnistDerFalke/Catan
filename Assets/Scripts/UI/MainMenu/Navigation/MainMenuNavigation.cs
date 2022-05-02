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
    public class MainMenuNavigation : MonoBehaviour
    {
        [Tooltip("Przycisk startu gry")]
        [SerializeField] private Button startButton;
    
        [Tooltip("Przycisk wyjscia z gry")]
        [SerializeField] private Button exitButton;
    
        [Tooltip("Przycisk wyjscia z ustawien gry")]
        [SerializeField] private Button backButton;
        
        [Tooltip("Przycisk wyjscia z ustawien nazw graczy")]
        [SerializeField] private Button backButton2;
    
        [Tooltip("Przycisk przechodzący do wyboru nicków")]
        [SerializeField] private Button runGameButton;
        
        [Tooltip("Przycisk przechodzący do właściwej gry")]
        [SerializeField] private Button finalAcceptButton;

        [Tooltip("Bazowe UI")]
        [SerializeField] private GameObject basicContent;

        [Tooltip("UI ustawień")]
        [SerializeField] private GameObject dynamicGameSettings;
        
        [Tooltip("UI nazw graczy")]
        [SerializeField] private GameObject dynamicPlayerNames;

        [Tooltip("Slider z liczbą graczy")]
        [SerializeField] private Slider playersNumberSlider;
        
        [Tooltip("Tekst z liczbą graczy")]
        [SerializeField] private Text playersNumber;
        
        [Tooltip("Tekst z błędem przy inpucie")]
        [SerializeField] private Text badNickErrorLabel;
        
        [Tooltip("Dropdown z wyborem trybu")]
        [SerializeField] private TMP_Dropdown modeDropdown;
        
        [Tooltip("Pola na wpisanie nazwy graczy")]
        [SerializeField] private TMP_InputField[] playerNamesInputs = new TMP_InputField[4];
        
        [Tooltip("Holder skryptu zoomowego")]
        [SerializeField] private GameObject zoomHolder;
    
        void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
            exitButton.onClick.AddListener(OnExitButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
            backButton2.onClick.AddListener(OnBackButton2Click);
            runGameButton.onClick.AddListener(OnRunGameButtonClick);
            finalAcceptButton.onClick.AddListener(OnFinalAcceptButtonClick);
        }

        void Update()
        {
            playersNumber.text = playersNumberSlider.value.ToString();
        }
        
        private void OnStartButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(dynamicGameSettings, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }
    
        private void OnBackButtonClick()
        {
            dynamicGameSettings.SetActive(false);
            StartCoroutine(WaitForAnimation(basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }
    
        private void OnBackButton2Click()
        {
            dynamicPlayerNames.SetActive(false);
            dynamicGameSettings.SetActive(true);
            
            //dezaktywować wszystkie miejsca przed zmianą okienka
            for (var i = 0; i < (int) playersNumberSlider.value; i++)
                playerNamesInputs[i].transform.parent.gameObject.SetActive(false);
        }
        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    
        private void OnRunGameButtonClick()
        {
            dynamicGameSettings.SetActive(false);
            dynamicPlayerNames.SetActive(true);
            
            //aktywować tyle miejsc do wpisania ile potrzeba
            for (var i = 0; i < (int) playersNumberSlider.value; i++)
                playerNamesInputs[i].transform.parent.gameObject.SetActive(true);
        }
        
        private void OnFinalAcceptButtonClick()
        {
            if (SetUpGameManager())
            {
                dynamicPlayerNames.SetActive(false);
                zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.FinalZoom);
                zoomHolder.GetComponent<MenuCameraMove>().SetActive(false);
                StartCoroutine(WaitForGameStart(zoomHolder.GetComponent<MenuCameraZoom>().runGameAnimationDelay));
            }
        }
        

        private bool SetUpGameManager()
        {
            badNickErrorLabel.text = "";
            GameManager.Mode = modeDropdown.options[modeDropdown.value].text == "PODSTAWOWY" ? 
                GameManager.CatanMode.Basic : GameManager.CatanMode.Advanced;
            GameManager.PlayersNumber = (int)playersNumberSlider.value;
            GameManager.PlayersNames = new string[GameManager.PlayersNumber];
            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
                if (playerNamesInputs[i].text.Length < 3)
                {
                    badNickErrorLabel.text = "Nazwa gracza " + (i + 1) + " jest za krótka!";
                    return false;
                }
                GameManager.PlayersNames[i] = playerNamesInputs[i].text;
            }
            return true;

        }
    
        //Async
        IEnumerator WaitForAnimation(GameObject ui, float delay)
        {
            yield return new WaitForSeconds(delay);
            ui.SetActive(true);
        }
    
        IEnumerator WaitForGameStart(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Scenes/GameScreen", LoadSceneMode.Single);
        }
    }
}
