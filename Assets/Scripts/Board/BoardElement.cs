using DataStorage;
using UnityEngine;

namespace Board
{
    //Destiny: Holding information about boards elements and handling interactions
    public class BoardElement : MonoBehaviour
    {
        void OnMouseDown()
        {
            GameManager.SelectedElement = this;
        }
    }
}
