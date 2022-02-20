using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    /*Przyciski*/
    [Tooltip("Przycisk przechodzący do ustawień gry.")]
    [SerializeField] private Button startButton;
    
    [Tooltip("Przycisk wychodzący z gry.")]
    [SerializeField] private Button exitButton;
    
    [Tooltip("Przycisk opuszczający ustawienia gry.")]
    [SerializeField] private Button backButton;
    
    [Tooltip("Przycisk rozpoczynający właściwą grę.")]
    [SerializeField] private Button runGameButton;
    
    /*Elementy UI*/
    [Tooltip("Podstawowe UI. Podstawowa zawartość.")]
    [SerializeField] private GameObject basicContent;

    [Tooltip("UI ustawień gry. Dynamiczna zawartość.")]
    [SerializeField] private GameObject dynamicContent;
    
    [Tooltip("Slider z liczbą graczy.")]
    [SerializeField] private Slider playersNumberSlider;
    
    [Tooltip("Tekst z liczbą graczy.")]
    [SerializeField] private Text playersNumber;
    
    /*Holder skryptu odpowiedzialnego za zoom*/
    [Tooltip("Holder skryptu odpowiedzialnego za zoom.")]
    [SerializeField] private GameObject zoomHolder;
    
    void Start()
    {
        /*Przypisanie funkcji przyciskom*/
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        runGameButton.onClick.AddListener(OnRunGameButtonClick);
    }

    void Update()
    {
        /*Aktualizacja wartości liczbowej przy przesuwaniu suwakiem*/
        playersNumber.text = playersNumberSlider.value.ToString();
    }

    /*Funkcje obsługi przycisków*/
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
    
    /*Zdarzenia asynchroniczne*/
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
