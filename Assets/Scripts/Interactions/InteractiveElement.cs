using Board;
using DataStorage;
using UnityEngine;
using static Board.States.GameState;

namespace Interactions
{
    /// <summary>
    /// Definition class for interactive elements, to append it to game object a child class of it should be created
    /// </summary>
    public class InteractiveElement : MonoBehaviour
    {
        //Destiny: Default scale of element
        private Vector3 startScale;
        
        //Destiny: Default element height location
        private float startHeight;

        //Destiny: Offset height which is applied when element is selected (moved up)
        private const float StandardOffset = 0.4f;

        //Destiny: Flags that defines if element is interactable or not
        protected bool Blocked;
        protected bool IsPointed;

        /// <summary>
        /// Does basic mouse down event stuff (it can be overriden to add specific actions for specific element)
        /// </summary>
        protected virtual void OnMouseDown()
        {
            if (GameManager.Selected.Element == GetComponent<BoardElement>())
            {
                GameManager.Selected.Element = null;
                return;
            }
            GameManager.Selected.Element = null;

            if (Blocked)
            {
                return;
            }
            GameManager.Selected.Element = GetComponent<BoardElement>();

            if (GameManager.Selected.Element as JunctionElement != null || 
                GameManager.Selected.Element as PathElement != null)
                GameManager.BuildManager.BuildRequests.Add(true);
            else if(GameManager.Selected.Element as FieldElement != null)
                GameManager.BuildManager.ThiefMoveRequests.Add(true);
        }
        
        /// <summary>
        /// Does basic mouse enter event stuff (sets element as pointed -> there is a mouse over the element)
        /// </summary>
        private void OnMouseEnter()
        { 
            GameManager.Selected.Pointed = GetComponent<BoardElement>();
            IsPointed = true;
        }

        /// <summary>
        /// Does basic mouse exit event stuff (sets element as unpointed)
        /// </summary>
        private void OnMouseExit()
        {
            GameManager.Selected.Pointed = null;
            IsPointed = false;
        }

        void Start()
        {
            //Destiny: Starts to make specific actions on start (can be used in classes that inherit from this one)
            DoSpecificActionsOnStart();
            
            //Destiny: Setting default scales and height
            startScale = transform.localScale;
            startHeight = transform.position.y;
            
            //Destiny: Set element as unselected
            UnselectElement();
        }
        
        void Update()
        {
            //Destiny: Check if element is blocked
            Blocked = CheckBlockStatus();
            
            //Destiny: If element is not selected or blocked it is set as unselected
            if(GameManager.Selected.Element != GetComponent<BoardElement>())
            {
                UnselectElement();
            }
            
            //Destiny: Starts to make specific actions on update (can be used in classes that inherit from this one)
            DoSpecificActionsOnUpdate();
        }

        /// <summary>
        /// Method to override in specific interactive board element to make specific action for this element
        /// </summary>
        protected virtual void DoSpecificActionsOnUpdate()
        {
            //Destiny: Content should be overriden in element that inherits from interactive element on Update
        }
        
        /// <summary>
        /// Method to override in specific interactive board element to make specific action for this element on Start
        /// </summary>
        protected virtual void DoSpecificActionsOnStart()
        {
            //Destiny: Content should be overriden in element that inherits from interactive element
        }
        
        /// <summary>
        /// Checks if there is any action that should block pointing the elements
        /// </summary>
        protected virtual bool CheckBlockStatus()
        {
            //Destiny: Here there are block cases for all interactive elements
            if (GameManager.State.SwitchingGameMode == SwitchingMode.GameSwitching &&
                GameManager.State.CurrentDiceThrownNumber == 0 &&
                GameManager.State.MovingUserMode == MovingMode.Normal)
            {
                return true;
            }
            
            //Destiny: If there is no reason to block
            return false;
        }

        /// <summary>
        /// Lowers element back down
        /// </summary>
        protected virtual void UnselectElement()
        {
            transform.localScale = startScale;
            transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
        }

        /// <summary>
        /// Pushes element up
        /// </summary>
        protected virtual void SelectElement()
        {
            transform.localScale = 1.5f * startScale;
            transform.position = new Vector3(transform.position.x, startHeight + StandardOffset, transform.position.z);
        }
    }
}