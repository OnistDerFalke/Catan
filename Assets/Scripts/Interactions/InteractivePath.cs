using DataStorage;

namespace Interactions
{
    public class InteractivePath : InteractiveElement
    {
        protected override bool CheckBlockStatus()
        {
            //Destiny: Here we return true in cases we want to block the paths pointing
            if (GameManager.MovingUserMode == GameManager.MovingMode.MovingThief) 
                return true;
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }
    }
}