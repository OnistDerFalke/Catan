using Board;
using DataStorage;

namespace Interactions
{
    public class InteractiveJunction : InteractiveElement
    {
        void Update()
        {
            if(GameManager.Selected.Element != GetComponent<BoardElement>()) 
                SetDefaultMaterial();
            blocked = GameManager.MovingUserMode == GameManager.MovingMode.MovingThief;
        }
    }
}