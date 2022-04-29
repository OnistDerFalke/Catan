using DataStorage;
using UnityEngine;

namespace Board
{
    public class BoardElement : MonoBehaviour
    {
        void OnMouseDown()
        {
            GameManager.Instance.selectedElement = this;
        }
    }
}
