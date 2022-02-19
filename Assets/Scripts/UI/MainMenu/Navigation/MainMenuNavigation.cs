using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuNavigation : MonoBehaviour
{
    //przyciski
    [Tooltip("Przycisk przechodzący do ustawień gry.")]
    [SerializeField] private Button startButton;
    
    [Tooltip("Przycisk wychodzący z gry.")]
    [SerializeField] private Button exitButton;
    
    [Tooltip("Przycisk opuszczający ustawienia gry.")]
    [SerializeField] private Button backButton;
    
    [Tooltip("Przycisk rozpoczynający właściwą grę.")]
    [SerializeField] private Button runGameButton;
    
    //elementy UI
    [Tooltip("Podstawowe UI. Podstawowa zawartość.")]
    [SerializeField] private GameObject basicContent;

    [Tooltip("UI ustawień gry. Dynamiczna zawartość.")]
    [SerializeField] private GameObject dynamicContent;
    
    [Tooltip("Slider z liczbą graczy.")]
    [SerializeField] private Slider playersNumberSlider;
    
    [Tooltip("Tekst z liczbą graczy.")]
    [SerializeField] private Text playersNumber;
    
    //holder skryptu odpowiedzialnego za zoom
    [Tooltip("Holder skryptu odpowiedzialnego za zoom.")]
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
        playersNumber.text = playersNumberSlider.value.ToString();
    }

    private void OnStartButtonClick()
    {
        basicContent.SetActive(false);
        zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomIn);
        dynamicContent.SetActive(true);
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnBackButtonClick()
    {
        dynamicContent.SetActive(false);
        zoomHolder.GetComponent<MenuCameraZoom>().SetZoomMode(MenuCameraZoom.ZoomMode.ZoomOut);
        basicContent.SetActive(true);
    }

    private void OnRunGameButtonClick()
    {
        //TODO
    }
}
