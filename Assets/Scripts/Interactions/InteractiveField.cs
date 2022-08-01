using Board;
using DataStorage;
using UnityEngine;
using static Board.States.GameState;

namespace Interactions
{
    public class InteractiveField : InteractiveElement
    {
        //Destiny: All renderers on the field that will be blacked on point or select
        [Header("Field Mesh Renderers")][Space(5)]
        [Tooltip("Field Mesh Renderers")] [SerializeField]
        private MeshRenderer[] fieldRenderers;

        //Destiny: Here colors of renderer materials are stored
        private Color[] renderersMaterialsColors;
        
        /// <summary>
        /// Does basic mouse down event stuff and then specific actions for the field element
        /// </summary>
        protected override void OnMouseDown()
        {
            base.OnMouseDown();
            SelectElement();
        }
        
        /// <summary>
        /// Realizes basic procedure of blocking of all fields and additional conditions for the field
        /// </summary>
        /// <returns>If field is blocked</returns>
        protected override bool CheckBlockStatus()
        {
            //Destiny: Here we return true in cases we want to block the fields pointing
            if (GameManager.State.MovingUserMode != MovingMode.MovingThief) 
                return true;
            if (gameObject.GetComponent<FieldElement>().IfThief())
                return true;
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }

        /// <summary>
        /// Does specific action for the field on start (it is run on start in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnStart()
        {
            //Destiny: Setting default materials for every mesh renderer
            renderersMaterialsColors = new Color[fieldRenderers.Length];
            for (var i = 0; i < fieldRenderers.Length; i++)
                renderersMaterialsColors[i] = fieldRenderers[i].material.color;
        }
        
        /// <summary>
        /// Does specific action for the field on update (it is run on update in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnUpdate()
        {
            SetMaterialOnPoint();
        }

        /// <summary>
        /// Changes material color of all field meshes on point or select
        /// </summary>
        private void SetMaterialOnPoint()
        {
            for (var i = 0; i < fieldRenderers.Length; i++)
            {
                //Destiny: Change color if selected field element is interactable
                var color = (IsPointed || GameManager.Selected.Element == gameObject.GetComponent<FieldElement>()) &&
                            !Blocked ? Color.black : renderersMaterialsColors[i];
                fieldRenderers[i].material.color = color;
            }
        }
    }
}