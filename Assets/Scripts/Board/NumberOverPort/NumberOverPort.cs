using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Board.NumberOverPort
{
    public class NumberOverPort : MonoBehaviour
    {
        [Header("Info elements")] [Space(5)] 
        [Tooltip("Image")] [SerializeField] 
        private Image image;
        [Tooltip("Text")] [SerializeField] 
        private TextMeshProUGUI text;

        [Header("Resources Sprites")] [Space(5)]
        [Tooltip("Clay")] [SerializeField]
        private Sprite claySprite;

        [Tooltip("Wood")] [SerializeField] 
        private Sprite woodSprite;
        [Tooltip("Wheat")] [SerializeField] 
        private Sprite wheatSprite;
        [Tooltip("Wool")] [SerializeField] 
        private Sprite woolSprite;
        [Tooltip("Iron")] [SerializeField] 
        private Sprite ironSprite;
        
        [Header("Texts")] [Space(5)] 
        [Tooltip("ThreeForOneText")] [SerializeField]
        private string threeForOneText;
        [Tooltip("TwoForOneText")] [SerializeField]
        private string twoForOneText;

        /// <summary>
        /// Sets info over the port with hardcoded information
        /// </summary>
        /// <param name="index">index of the info</param>
        /// <param name="type">type of the resource</param>
        public void SetInfo(int index)
        {
            text.text = GetTextFromHardcodedConditions(index);
            var sprite = GetSpriteFromHardcodedConditions(index);
            if (sprite == null)
                image.gameObject.SetActive(false);
            else image.sprite = sprite;
        }

        /// <summary>
        /// Gives text that should be shown for index given
        /// </summary>
        /// <param name="index">Index of the information</param>
        /// <returns>Text to show on the information</returns>
        private string GetTextFromHardcodedConditions(int index)
        {
            var textToShow = twoForOneText;
            int[] threeForOneIndex = { 0, 3, 5, 6 };
            foreach (var i in threeForOneIndex)
                if (index == i) 
                    textToShow = threeForOneText;
            return textToShow;
        }

        /// <summary>
        /// Gives sprite that should be shown for index given
        /// </summary>
        /// <param name="index">Index of the information</param>
        /// <returns>Sprite to show on the information (null if icon should not be shown</returns>
        private Sprite GetSpriteFromHardcodedConditions(int index)
        {
            return index switch
            {
                1 => wheatSprite,
                2 => ironSprite,
                4 => woolSprite,
                7 => claySprite,
                8 => woodSprite,
                _ => null
            };
        }
    }
}