using DataStorage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
    public class InteractiveOcean : MonoBehaviour
    {
        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) GameManager.Selected.Element = null;
        }
    }
}