using TMPro;
using UnityEngine;

namespace Board.NumberOverField
{
    //Destiny: Managing the value of number over the field (script should be attached to TextMeshPro number)
    public class NumberOverField : MonoBehaviour
    {
        TextMeshProUGUI numberOverField;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">The value that should appear over the field</param>
        public void SetNumberValue(int value)
        {
            //Destiny: TextMeshProUGUI must be set before changing value (because object instances are instantiated)
            numberOverField = GetComponentInChildren<TextMeshProUGUI>();

            //Destiny: Don't show the 0 number - it's for the desert
            if (value == 0)
            {
                numberOverField.text = "";
                return;
            }
            numberOverField.text = value.ToString();
        }
    }
}