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

        protected bool blocked = false;

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

        private void Start()
        {
            rend = GetComponent<MeshRenderer>();
            startScale = transform.localScale;
            startHeight = transform.position.y;
            SetDefaultMaterial();
        }
        
        private void Update()
        {
            if(GameManager.Selected.Element != GetComponent<BoardElement>() || blocked) 
                SetDefaultMaterial();
        }

        /// <summary>
        /// Sets default material on element
        /// </summary>
        protected void SetDefaultMaterial()
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