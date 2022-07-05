using Board;
using DataStorage;

namespace Interactions
{
    public class InteractiveField : InteractiveElement
    {
        protected override bool CheckBlockStatus()
        {
            //Destiny: Here we return true in cases we want to block the fields pointing
            if (GameManager.MovingUserMode != GameManager.MovingMode.MovingThief) 
                return true;
            if (gameObject.GetComponent<FieldElement>().IfThief())
                return true;
            
            //Destiny: Here there are block cases for all interactive elements
            return base.CheckBlockStatus();
        }
    }
}