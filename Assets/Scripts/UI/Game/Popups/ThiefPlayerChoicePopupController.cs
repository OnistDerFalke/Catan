using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class ThiefPlayerChoicePopupController : MonoBehaviour
    {
        [Header("Confirm Button")][Space(5)]
        [Tooltip("Confirm Button")]
        [SerializeField] private Button confirmButton;
        
        [Header("Players Buttons")][Space(5)]
        [Tooltip("Players Buttons")]
        [SerializeField] private Button[] playersButtons = new Button[4];
        
        [Header("Players Names")][Space(5)]
        [Tooltip("Players Names")]
        [SerializeField] private Text[] playersNames = new Text[4];
        
        [Header("Players Colors")][Space(5)]
        [Tooltip("Players Colors")]
        [SerializeField] private Image[] playersColors = new Image[4];

        //Destiny: Offset (space) between players buttons
        [SerializeField] private float iconOffset;

        
        // Start is called before the first frame update
        void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmButton);
            
            for (var i = 0; i<playersColors.Length; i++)
            {
                playersColors[i].color = GameManager.Players[i].color switch
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
                button.gameObject.SetActive(false);

            //TODO: Here we append list of players index that we need to show in window
            //temporary mocked list
            int[] playersToShow = {0, 1, 2};
            Debug.Log(playersToShow.Length);
            switch (playersToShow.Length)
            {
                case 0:
                    GameManager.ThiefPlayerChoicePopupShown = false;
                    break;
                case 1:
                {
                    //Destiny: Only one player, centered
                    var temp = playersButtons[playersToShow[0]].gameObject.transform.localPosition;
                    temp.x = 0;
                    playersButtons[playersToShow[0]].gameObject.transform.localPosition = temp;
                    break;
                }
                case 2:
                {
                    //Destiny: Two players, centered
                    var temp = playersButtons[playersToShow[0]].gameObject.transform.localPosition;
                    temp.x = -iconOffset/2f;
                    playersButtons[playersToShow[0]].gameObject.transform.localPosition = temp;
                    temp = playersButtons[playersToShow[1]].gameObject.transform.localPosition;
                    temp.x = iconOffset/2f;
                    playersButtons[playersToShow[1]].gameObject.transform.localPosition = temp;
                    break;
                }
                case 3:
                {
                    //Destiny: Three players, centered
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
                playersNames[index].text = GameManager.Players[index].name;
                playersButtons[index].gameObject.SetActive(true);
            }
        }

        private void OnConfirmButton()
        {
            
        }
    }
}
