using Board;
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
        [Tooltip("Turn Skip Button")]
        [SerializeField] private Button turnSkipButton;
        [Tooltip("Build Button")]
        [SerializeField] private Button buildButton;

        /// <summary>
        /// Changes the current moving player to the next in the queue
        /// </summary>
        private void OnTurnSkipButton()
        {
            GameManager.SwitchToNextPlayer();
        }

        /// <summary>
        /// Builds the element on selection
        /// </summary>
        private void OnBuildButton()
        {
            switch (GameManager.Selected.Type)
            {
                case BoardElement.BoardElementType.Junction:
                    GameManager.Players[GameManager.CurrentPlayer].properties
                        .AddBuilding(GameManager.Selected.SelectedJunction.id,
                            GameManager.Selected.SelectedJunction.type == JunctionElement.JunctionType.Village);
                    break;
                case BoardElement.BoardElementType.Path:
                    GameManager.Players[GameManager.CurrentPlayer].properties
                        .AddPath(GameManager.Selected.SelectedPath.id);
                    break;
            }
        }

        /// <summary>
        /// Blocks build button if build conditions are not satisfied
        /// </summary>
        private void BuildButtonActivity()
        {
            if (!GameManager.Selected.IsSelected)
            {
                buildButton.interactable = false;
                return;
            }
            switch (GameManager.Selected.Type)
            {
                case BoardElement.BoardElementType.Junction:
                    if (!GameManager.Selected.SelectedJunction.canBuild)
                    {
                        
                        buildButton.interactable = false;
                        return;
                    }
                    break;
                case BoardElement.BoardElementType.Path:
                    if (!GameManager.Selected.SelectedPath.canBuild)
                    {
                        buildButton.interactable = false;
                        return;
                    }
                    break;
            }
            buildButton.interactable = true;
        }
        
        void Start()
        {
            turnSkipButton.onClick.AddListener(OnTurnSkipButton);
            buildButton.onClick.AddListener(OnBuildButton);
        }

        void Update()
        {
            BuildButtonActivity();
        }
    }
}
