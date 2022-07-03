using Board;
using DataStorage;
using UnityEngine;

namespace Interactions
{
    public class InteractiveElement : MonoBehaviour
    {
        [Header("Materials")][Space(5)]
        [Tooltip("Normal material")] [SerializeField]
        private Material normalMaterial;
        [Tooltip("Glowing material")] [SerializeField]
        private Material glowingMaterial;
        
        private MeshRenderer rend;
        private Vector3 startScale;
        private float startHeight;

        private const float StandardOffset = 0.4f;

        private bool blocked;

        private void OnMouseDown()
        {
            if (GameManager.Selected.Element == GetComponent<BoardElement>())
            {
                GameManager.Selected.Element = null;
                return;
            }

            if (blocked) return;
            GameManager.Selected.Element = GetComponent<BoardElement>();
            SetGlowingMaterial();
        }

        void Start()
        {
            rend = GetComponent<MeshRenderer>();
            startScale = transform.localScale;
            startHeight = transform.position.y;
            SetDefaultMaterial();
        }
        
        void Update()
        {
            blocked = CheckBlockStatus();
            
            if(GameManager.Selected.Element != GetComponent<BoardElement>() || blocked) 
                SetDefaultMaterial();
        }

        /// <summary>
        /// Checks if there is any action that should block pointing the elements
        /// </summary>
        protected virtual bool CheckBlockStatus()
        {
            //Destiny: Here there are block cases for all interactive elements
            if (GameManager.SwitchingGameMode == GameManager.SwitchingMode.GameSwitching &&
                GameManager.CurrentDiceThrownNumber == 0 &&
                GameManager.MovingUserMode == GameManager.MovingMode.Normal) return true;
            
            //Destiny: If there is no reason to block
            return false;
        }

        /// <summary>
        /// Sets default material on element
        /// </summary>
        private void SetDefaultMaterial()
        {
            transform.localScale = startScale;
            transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
            rend.material = normalMaterial;
        }

        /// <summary>
        /// Sets glowing material on element
        /// </summary>
        private void SetGlowingMaterial()
        {
            transform.localScale = 1.5f * startScale;
            transform.position = new Vector3(transform.position.x, startHeight + StandardOffset, transform.position.z);
            rend.material = glowingMaterial;
        }
    }
}