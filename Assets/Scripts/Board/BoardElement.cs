using Assets.Scripts.Board.States;
using UnityEngine;

namespace Board
{
    public class BoardElement : MonoBehaviour
    {
        public enum ElementType
        {
            None,
            Field,
            Path,
            Junction
        }
        
        //Destiny: Type of the board element
        public ElementType elementType;

        //Destiny: Data of an element that should be saved
        public ElementState State = new();
    }
}