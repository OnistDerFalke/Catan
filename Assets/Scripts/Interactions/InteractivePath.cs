using System.Collections;
using Board;
using DataStorage;
using UnityEngine;
using static Board.States.GameState;

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
        
        //Destiny: Defines if junction should blink if can be build
        private bool isBlinking;

        public MovingMode MovingUserMode { get; private set; }

        /// <summary>
        /// Realizes basic procedure of blocking of all paths and additional conditions for the path
        /// </summary>
        /// <returns>If path is blocked</returns>
        protected override bool CheckBlockStatus()
        {
            //Destiny: There is no possibility to build a path
            if (!gameObject.GetComponent<PathElement>().Available(GameManager.Selected.Pointed))
            {
                return true;
            }

            //Destiny: Here we return true in cases we want to block the paths pointing
            if (MovingUserMode == MovingMode.MovingThief)
            {
                return true;
            }
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }
        
        /// <summary>
        /// Checks if player is able to build the path
        /// </summary>
        /// <returns>If path can be built</returns>
        private bool CheckInteractableStatus()
        {
            return GameManager.BuildManager.CheckIfPlayerCanBuildPath(gameObject.GetComponent<PathElement>().State.id);
        }

        /// <summary>
        /// Does specific action for the path on start (it is run on start in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnStart()
        {
            rend = gameObject.GetComponent<MeshRenderer>();
            var pb = new MaterialPropertyBlock();
            rend.GetPropertyBlock(pb);
            pb.SetColor("_Color", rend.material.color);
            rend.SetPropertyBlock(pb);
        }
        
        /// <summary>
        /// Does specific action for the path on update (it is run on update in class InteractiveElement)
        /// </summary>
        protected override void DoSpecificActionsOnUpdate()
        {
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(Blink());
            }

            canBeBuilt = CheckInteractableStatus();
            
            var pb = new MaterialPropertyBlock();
            rend.GetPropertyBlock(pb);
            var color = pb.GetColor("_Color");
            pb.SetColor("_Color", IsPointed && canBeBuilt && !Blocked ? Color.black : color);
            rend.SetPropertyBlock(pb);
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
        
        private IEnumerator Blink()
        {
            var pb = new MaterialPropertyBlock();
            rend.GetPropertyBlock(pb);
            var color = pb.GetColor("_Color");
            float hue = 0;
            var raisingUp = true;
            while(gameObject.GetComponent<PathElement>().Available(gameObject.GetComponent<PathElement>()))
            {
                if (hue >= 0.2f) raisingUp = false;
                else if (hue <= 0f) raisingUp = true;
                
                if (raisingUp) hue += 0.5f * Time.deltaTime;
                else hue -= 0.5f * Time.deltaTime;
                if (canBeBuilt && !IsPointed && GameManager.Selected.Element != GetComponent<BoardElement>())
                {
                    pb.SetColor("_Color", new Color(hue, hue, hue));
                    rend.SetPropertyBlock(pb);
                }
                yield return new WaitForSeconds(0.01f);
            }
            pb.SetColor("_Color", color);
            rend.SetPropertyBlock(pb);
            isBlinking = false;
        }
    }
}