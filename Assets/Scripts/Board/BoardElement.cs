using UnityEngine;

namespace Board
{
    public class BoardElement : MonoBehaviour
    {
        public enum BoardElementType
        {
            None,
            Field,
            Path,
            Junction
        }
        
        //Destiny: Type of the board element
        public BoardElementType boardElementType;
        
        //Destiny: ID of board element
        public int id;
    }
}