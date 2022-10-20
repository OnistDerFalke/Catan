using Assets.Scripts.DataStorage.Managers;
using Board;
using DataStorage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player.Resources;

namespace UI.Game.Popups
{
    public class ThiefPlayerChoicePopupController : MonoBehaviour
    {
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")][SerializeField] 
        private Button confirmButton;
        
        [Header("Players Buttons")][Space(5)]
        [Tooltip("Players Buttons")][SerializeField]
        private Button[] playersButtons = new Button[4];
        [Tooltip("Players Names")][SerializeField]
        private Text[] playersNames = new Text[4];
        [Tooltip("Players Buttons Backgrounds")][SerializeField] 
        private Image[] playersButtonsBackgrounds = new Image[4];
        [Tooltip("Player Button Zoom Scale")][SerializeField]
        private float playerButtonZoomScale;
        [Tooltip("Player Button Clicked Color")][SerializeField]
        private Color playerButtonClickedColor;
        [Tooltip("Player Button Normal Color")][SerializeField]
        private Color playerButtonNormalColor;

        [Header("Players Colors")][Space(5)]
        [Tooltip("Players Colors")][SerializeField]
        private Image[] playersColors = new Image[4];

        [SerializeField] private LogsManager logsManager;

        //Destiny: Offset (space) between players buttons
        [SerializeField] private float iconOffset;

        //Destiny: Final value that window returns after the choice
        private int chosenPlayerIndex;
        
        void Start()
        {
            chosenPlayerIndex = -1;
            for (var i = 0; i < playersButtons.Length; i++)
            {
                var index = i;
                playersButtons[i].onClick.AddListener(() => { chosenPlayerIndex = index; });
            }
            confirmButton.onClick.AddListener(OnConfirmButton);
            
            for (var i = 0; i<playersColors.Length; i++)
            {
                if (i == 3 && GameManager.State.Players.Length == 3) continue;
                playersColors[i].color = GameManager.State.Players[i].color switch
                {
                    Player.Player.Color.Blue => Color.blue,
                    Player.Player.Color.Red => Color.red,
                    Player.Player.Color.Yellow => Color.yellow,
                    Player.Player.Color.White => Color.white,
                    _ => playersColors[i].color
                };
            }
        }
        
        void OnEnable()
        {
            //Destiny: Clearing window before new setting up
            foreach (var button in playersButtons)
            {
                button.gameObject.SetActive(false);
            }

            //Destiny: List of players indexes that we need to show in window
            List<int> playersToShow = GameManager.AdjacentPlayerIdToField(BoardManager.FindThief());

            switch (playersToShow.Count)
            {
                case 0:
                {
                    GameManager.PopupManager.PopupsShown[PopupManager.THIEF_PLAYER_CHOICE_POPUP] = false;
                    break;
                }
                //Destiny: Only one player, centered
                case 1:
                {
                    var temp = playersButtons[playersToShow[0]].gameObject.transform.localPosition;
                    temp.x = 0;
                    playersButtons[playersToShow[0]].gameObject.transform.localPosition = temp;
                    break;
                }
                //Destiny: Two players, centered
                case 2:
                {
                    var temp = playersButtons[playersToShow[0]].gameObject.transform.localPosition;
                    temp.x = -iconOffset/2f;
                    playersButtons[playersToShow[0]].gameObject.transform.localPosition = temp;
                    temp = playersButtons[playersToShow[1]].gameObject.transform.localPosition;
                    temp.x = iconOffset/2f;
                    playersButtons[playersToShow[1]].gameObject.transform.localPosition = temp;
                    break;
                }
                //Destiny: Three players, centered
                case 3:
                {
                    var temp = playersButtons[playersToShow[1]].gameObject.transform.localPosition;
                    temp.x = 0;
                    playersButtons[playersToShow[1]].gameObject.transform.localPosition = temp;
                    temp = playersButtons[playersToShow[0]].gameObject.transform.localPosition;
                    temp.x = -iconOffset;
                    playersButtons[playersToShow[0]].gameObject.transform.localPosition = temp;
                    temp = playersButtons[playersToShow[2]].gameObject.transform.localPosition;
                    temp.x = iconOffset;
                    playersButtons[playersToShow[2]].gameObject.transform.localPosition = temp;
                    break;
                }
            }
            
            //Destiny: Activating buttons that are available to choose
            foreach (var index in playersToShow)
            {
                playersNames[index].text = GameManager.State.Players[index].name;
                playersButtons[index].gameObject.SetActive(true);
            }
        }

        void Update()
        {
            //Destiny: Enable confirm if choice was made
            confirmButton.enabled = chosenPlayerIndex >= 0;
            
            //Destiny: Zoom choice
            for (var i = 0; i < playersButtons.Length; i++)
            {
                playersButtons[i].gameObject.transform.localScale = Vector3.one;
                playersButtonsBackgrounds[i].color = playerButtonNormalColor;
            }

            if (chosenPlayerIndex >= 0 && chosenPlayerIndex < playersButtons.Length)
            {
                playersButtons[chosenPlayerIndex].gameObject.transform.localScale =
                    new Vector3(playerButtonZoomScale, playerButtonZoomScale, playerButtonZoomScale);
                playersButtonsBackgrounds[chosenPlayerIndex].color = playerButtonClickedColor;
            }
        }

        private void OnConfirmButton()
        {
            //Destiny: The chosen player gives a random resource to the current player
            var resource = GameManager.State.Players[chosenPlayerIndex].resources.GetRandomResource();
            logsManager.Log($"Gracz {GameManager.State.Players[chosenPlayerIndex].name} oddaje 1 " +
                            $"{GameManager.State.Players[chosenPlayerIndex].resources.GetResourceName(resource)} graczowi " +
                            $"{GameManager.State.Players[GameManager.State.CurrentPlayerId].name}.");
            GameManager.State.Players[chosenPlayerIndex].resources.SubtractSpecifiedResource(resource);
            GameManager.State.Players[GameManager.State.CurrentPlayerId].resources.AddSpecifiedResource(resource);

            //Destiny: Closing the window/popup
            chosenPlayerIndex = -1;
            confirmButton.enabled = false;
            GameManager.PopupManager.PopupsShown[PopupManager.THIEF_PLAYER_CHOICE_POPUP] = false;
        }
    }
}
