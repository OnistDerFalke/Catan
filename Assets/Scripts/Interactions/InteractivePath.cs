using Board;
using DataStorage;
using UnityEngine;

namespace Interactions
{
    public class InteractivePath : InteractiveElement
    {
        //Destiny: Materials for selected and unselected path
        [Header("Materials")][Space(5)]
        [Tooltip("Normal material")] [SerializeField] private Material normalMaterial;
        [Tooltip("Glowing material")] [SerializeField] private Material glowingMaterial;
        
        //Destiny: Path element renderer
        private MeshRenderer rend;
        
        //Destiny: Defines if path can be selected
        private bool canBeBuilt;

        /// <summary>
        /// Does basic mouse down event stuff and then specific actions for the path element
        /// </summary>
        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            if(canBeBuilt) SelectElement();
        }
        
        /// <summary>
        /// Realizes basic procedure of blocking of all paths and additional conditions for the path
        /// </summary>
        /// <returns>If path is blocked</returns>
        protected override bool CheckBlockStatus()
        {
            //Destiny: There is no possibility to build a path
            if (!gameObject.GetComponent<PathElement>().Available(GameManager.Selected.Pointed))
                return true;

            //Destiny: Here we return true in cases we want to block the paths pointing
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief) 
                return true;
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }
        
        /// <summary>
        /// Checks if player is able to build the path
        /// </summary>
        /// <returns>If path can be built</returns>
        private bool CheckInteractableStatus()
        {
            return GameManager.CheckIfPlayerCanBuildPath(gameObject.GetComponent<PathElement>().id);
        }

        /// <summary>
        /// Does specific action for the path on start (it is run on start in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnStart()
        {
            rend = gameObject.GetComponent<MeshRenderer>();
        }
        
        /// <summary>
        /// Does specific action for the path on update (it is run on update in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnUpdate()
        {
            canBeBuilt = CheckInteractableStatus();
            
            var color = rend.material.color;
            color = IsPointed && canBeBuilt && !Blocked ? Color.black : color;
            rend.material.color = color;
        }

        /// <summary>
        /// Moves path down and then changes it's material to default one
        /// </summary>
        protected override void UnselectElement()
        {
            base.UnselectElement();
            rend.material = normalMaterial;
        }
        
        /// <summary>
        /// Moves path up and then changes it's material to selected one
        /// </summary>
        protected override void SelectElement()
        {
            base.SelectElement();
            rend.material = glowingMaterial;
        }
    }
}