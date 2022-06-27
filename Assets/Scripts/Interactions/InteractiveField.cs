using Board;
using DataStorage;
using UnityEngine;

namespace Interactions
{
    public class InteractiveField : InteractiveElement
    {
        void Update()
        {
            //Destiny: Blocks element if not assigned
            if(GameManager.Selected.Element != GetComponent<BoardElement>()) 
                SetDefaultMaterial();
            blocked = GameManager.MovingUserMode != GameManager.MovingMode.MovingThief;
            
            //Destiny: If thief, field cannot be selected
            if (gameObject.GetComponent<FieldElement>().IfThief()) SetDefaultMaterial();
        }
    }
}