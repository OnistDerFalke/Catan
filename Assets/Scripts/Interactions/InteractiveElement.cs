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
        
        private void OnMouseDown()
        {
            SetAsSelected();
            SetGlowingMaterial();
        }

        private void Start()
        {
            rend = GetComponent<MeshRenderer>();
            SetDefaultMaterial();
        }

        private void Update()
        {
            CheckIfStillSelected();
        }

        private void CheckIfStillSelected()
        {
            if(!(GameManager.Selected.SelectedJunction == GetComponent<JunctionElement>() &&
               GameManager.Selected.SelectedPath == GetComponent<PathElement>()) ||
               !GameManager.Selected.IsSelected) 
                SetDefaultMaterial();
        }
        
        /// <summary>
        /// Set selected element on GameManager
        /// </summary>
        private void SetAsSelected()
        {
            GameManager.SetSelectedElement(GetComponent<JunctionElement>(), GetComponent<PathElement>());
        }
        
        /// <summary>
        /// Sets default material on element
        /// </summary>
        private void SetDefaultMaterial()
        {
            rend.material = normalMaterial;
        }

        /// <summary>
        /// Sets glowing material on element
        /// </summary>
        private void SetGlowingMaterial()
        {
            rend.material = glowingMaterial;
        }
    }
}