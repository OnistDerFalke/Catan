using Board;
using DataStorage;
using UnityEngine;

namespace Interactions
{
    public class InteractiveJunction : InteractiveElement
    {
        //Destiny: Materials for selected and unselected junction
        [Header("Materials")][Space(5)]
        [Tooltip("Normal material")] [SerializeField] private Material normalMaterial;
        [Tooltip("Glowing material")] [SerializeField] private Material glowingMaterial;
        
        //Destiny: Junction element renderer
        private MeshRenderer rend;
        
        //Destiny: Defines if junction can be selected
        private bool canBeBuilt;
        
        /// <summary>
        /// Does basic mouse down event stuff and then specific actions for the junction element
        /// </summary>
        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            if(canBeBuilt) SelectElement();
        }
        
        /// <summary>
        /// Realizes basic procedure of blocking of all junctions and additional conditions for the junction
        /// </summary>
        /// <returns>If junction is blocked</returns>
        protected override bool CheckBlockStatus()
        {
            // //Destiny: There is no possibility to build on junction
            // if (!gameObject.GetComponent<JunctionElement>().Available())
            //     return true;
            
            //Destiny: Here we return true in cases we want to block the junctions pointing
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief)
                return true;
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }
        
        /// <summary>
        /// Checks if player is able to build on the junction
        /// </summary>
        /// <returns>If player can build on junction</returns>
        private bool CheckInteractableStatus()
        {
            return GameManager.CheckIfPlayerCanBuildBuilding(gameObject.GetComponent<JunctionElement>().id);
        }
        
        /// <summary>
        /// Does specific action for the junction on start (it is run on start in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnStart()
        {
            rend = gameObject.GetComponent<MeshRenderer>();
        }
        
        /// <summary>
        /// Does specific action for the junction on update (it is run on update in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnUpdate()
        {
            canBeBuilt = CheckInteractableStatus();
            
            var color = rend.material.color;
            color = IsPointed && canBeBuilt && !Blocked ? Color.black : color;
            rend.material.color = color;
        }

        /// <summary>
        /// Moves junction down and then changes it's material to default one
        /// </summary>
        protected override void UnselectElement()
        {
            base.UnselectElement();
            rend.material = normalMaterial;
        }
        
        /// <summary>
        /// Moves junction up and then changes it's material to selected one
        /// </summary>
        protected override void SelectElement()
        {
            base.SelectElement();
            rend.material = glowingMaterial;
        }
    }
}