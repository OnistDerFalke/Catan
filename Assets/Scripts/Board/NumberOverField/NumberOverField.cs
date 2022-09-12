using TMPro;
using UnityEngine;

namespace Board.NumberOverField
{
    //Destiny: Managing the value of number over the field (script should be attached to TextMeshPro number)
    public class NumberOverField : MonoBehaviour
    {
        TextMeshProUGUI numberOverField;
       
        //Destiny: Colors of the numbers
        [Header("Colors of the numbers")][Space(5)]
        [Tooltip("Normal color")][SerializeField] 
        private Color32 normalColor;
        [Tooltip("Color of most recent number")][SerializeField] 
        private Color32 recentColor;
        
        //Destiny: It is const and not set from inspector because it's one for all numbers
        private const float StandardFontSize = 1f;
        private const float CommonFontSize = 0.8f;
        
        /// <summary>
        /// Sets the number value and text style
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

            //Destiny: Most recent numbers (6,8) have different colors and styles
            numberOverField.color = value is 6 or 8 ? recentColor : normalColor;
            numberOverField.fontStyle = value is 6 or 8 ? FontStyles.Bold : FontStyles.Normal;
            numberOverField.fontSize = value is 6 or 8 ? StandardFontSize : CommonFontSize;

            //Destiny: Update the number value
            numberOverField.text = value.ToString();
        }
    }
}