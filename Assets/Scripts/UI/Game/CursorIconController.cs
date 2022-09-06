using Board;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;
using static Board.States.GameState;

namespace UI.Game
{
    public class CursorIconController : MonoBehaviour
    {
        [Header("Cursor Icon Image")][Space(5)]
        [Tooltip("Cursor Icon Image")][SerializeField]
        private Image cursorImage;
        [Tooltip("Cursor Icon Offset")][SerializeField] 
        private Vector2 cursorImageOffset;
        
        [Header("Cursor Icon Sprites")][Space(5)]
        [Tooltip("BuildingSprite")][SerializeField]
        private Sprite buildingSprite;


        enum IconType
        {
            None,
            Building
        }

        private IconType iconType;
        
        void Update()
        {
           SetCursorIconPosition();
           UpdateIconType();
           UpdateIconImage();
        }

        /// <summary>
        /// Setting the mouse cursor icon position
        /// </summary>
        private void SetCursorIconPosition()
        {
            //Destiny: Getting mouse position
            var mousePos = Input.mousePosition;
            
            //Destiny: Setting offset to place it near mouse cursor
            mousePos.x += cursorImageOffset.x;
            mousePos.y += cursorImageOffset.y;
            mousePos.z = 1f;
            
            //Destiny: Setting cursor image position to mouse position with offset parsed to world position
            cursorImage.gameObject.transform.position = UnityEngine.Camera.main!.ScreenToWorldPoint(mousePos);
        }

        /// <summary>
        /// Sets icon type
        /// </summary>
        private void UpdateIconType()
        {
            var pointed = GameManager.Selected.Pointed;
            iconType = IconType.None;

            if (pointed == null)
            {
                return;
            }
            
            if (pointed as JunctionElement != null)
            {
                if (((JunctionElement)pointed).Available(pointed) &&
                    GameManager.BuildManager.CheckIfPlayerCanBuildBuilding(((JunctionElement)pointed).State.id) && 
                    !CheckIfInteractionIsBlocked())
                {
                    iconType = IconType.Building;
                }
            }

            if (pointed as PathElement != null)
            {
                if(((PathElement)pointed).Available(pointed) &&
                    GameManager.BuildManager.CheckIfPlayerCanBuildPath(((PathElement)pointed).State.id) && 
                    !CheckIfInteractionIsBlocked())
                {
                    iconType = IconType.Building;
                }
            }
        }

        /// <summary>
        /// Updates icon image for the icon type set
        /// </summary>
        private void UpdateIconImage()
        {
            var col = cursorImage.color;

            switch (iconType)
            {
                case IconType.None:
                {
                    col.a = 0f;
                    cursorImage.color = col;
                    cursorImage.sprite = null;
                    break;
                }
                case IconType.Building:
                {
                    col.a = 1f;
                    cursorImage.color = col;
                    cursorImage.sprite = buildingSprite;
                    break;
                }
            }
        }

        /// <summary>
        /// Even if building can be built, it needs be checked if it's right time (building time) for building it
        /// </summary>
        /// <returns>If building is blocked</returns>
        private bool CheckIfInteractionIsBlocked()
        {
            return GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                   GameManager.State.CurrentDiceThrownNumber == 0 && 
                   GameManager.State.MovingUserMode == MovingMode.Normal;
        }
    }
}
