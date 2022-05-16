using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    //Destiny: Handling interactions with actions tab UI
    public class ActionsContentNavigation : MonoBehaviour
    {
        //Destiny: Buttons of action tab content
        [Header("Action tab buttons")][Space(5)]
        [Tooltip("TurnSkipButton")]
        [SerializeField] private Button turnSkipButton;

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            GameManager.CurrentPlayer = (GameManager.CurrentPlayer + 1) % GameManager.PlayersNumber;
            Debug.Log(GameManager.CurrentPlayer);
        }
        
        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
        }
    }
}
