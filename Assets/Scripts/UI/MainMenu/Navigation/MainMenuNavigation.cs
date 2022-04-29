using System.Collections;
using Camera.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu.Navigation
{
    public class MainMenuNavigation : MonoBehaviour
    {
        [Tooltip("Przycisk startu gry")]
        [SerializeField] private Button startButton;
    
        [Tooltip("Przycisk wyjscia z gry")]
        [SerializeField] private Button exitButton;
    
        [Tooltip("Przycisk wyjscia z ustawien")]
        [SerializeField] private Button backButton;
    
        [Tooltip("Przycisk rozpoczynajacy rozgrywke")]
        [SerializeField] private Button runGameButton;

        [Tooltip("Bazowe UI")]
        [SerializeField] private GameObject basicContent;

        [Tooltip("UI dynamiczne")]
        [SerializeField] private GameObject dynamicContent;

        [Tooltip("Slider z liczbą graczy")]
        [SerializeField] private Slider playersNumberSlider;
        
        [Tooltip("Tekst z liczbą graczy")]
        [SerializeField] private Text playersNumber;
        
        [Tooltip("Holder skryptu zoomowego")]
        [SerializeField] private GameObject zoomHolder;
    
        void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClick);
            exitButton.onClick.AddListener(OnExitButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
            runGameButton.onClick.AddListener(OnRunGameButtonClick);
        }

        void Update()
        {
            //Aktualizacja suwaka liczby graczy
            playersNumber.text = playersNumberSlider.value.ToString();
        }
        
        private void OnStartButtonClick()
        {
            basicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(dynamicContent, zoomHolder.GetComponent<MenuCameraZoom>().showDynamicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        }
    
        private void OnBackButtonClick()
        {
            dynamicContent.SetActive(false);
            StartCoroutine(WaitForAnimation(basicContent, zoomHolder.GetComponent<MenuCameraZoom>().showBasicContentUIDelay));
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        }
    
        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    
        private void OnRunGameButtonClick()
        {
            dynamicContent.SetActive(false);
            zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.FinalZoom);
            zoomHolder.GetComponent<MenuCameraMove>().SetActive(false);
            StartCoroutine(WaitForGameStart(zoomHolder.GetComponent<MenuCameraZoom>().runGameAnimationDelay));
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
