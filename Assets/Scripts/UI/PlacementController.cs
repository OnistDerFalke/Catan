using System;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.Animations;

namespace UI
{
    public class PlacementController : MonoBehaviour
    {
        [Header("Settings")][Space(5)]
        [Tooltip("Default Aspect")] [SerializeField] private Vector2 defaultResolution;
        [Tooltip("Canvas")] [SerializeField] private RectTransform currentRectTransform;
        [Tooltip("Rect Holder")] [SerializeField] private RectTransform rectHolder;
        [Tooltip("UI Type")] [SerializeField] private UIType type;

        private enum UIType
        {
            Vertical,
            Horizontal
        }
        void Start()
        {
            switch (type)
            {
                case UIType.Horizontal:
                    HorizontalPlacement();
                    break;
                case UIType.Vertical:
                    VerticalPlacement();
                    break;
            }
        }

        /// <summary>
        /// Places elements that ale placed horizontally on the screen
        /// </summary>
        private void HorizontalPlacement()
        {
            var pos = transform.localPosition;
            var currentHeight = currentRectTransform.rect.height;
            var holderHeight = rectHolder.rect.height;
            var fixedHeight = pos.y / (defaultResolution.y / 2f) * (currentHeight / 2f);
            var offset = Mathf.Abs(holderHeight/(defaultResolution.y / 2f) * (currentHeight / 2f)-holderHeight)/2;
            var defaultAspectRatio = defaultResolution.x / defaultResolution.y; 
            var currentAspectRatio = currentRectTransform.rect.width / currentHeight;
            
            pos.y = fixedHeight;
            if (pos.y < 0)
            {
                if (currentAspectRatio > defaultAspectRatio)
                    pos.y += offset;
                else
                    pos.y -= offset;
            }
            else if (pos.y > 0)
            {
                pos.y -= offset;
            }
            transform.localPosition = pos;
        }

        /// <summary>
        /// Places elements that are placed vertically on the screen
        /// </summary>
        private void VerticalPlacement()
        {
            var pos = transform.localPosition;
            var currentHeight = currentRectTransform.rect.height;
            var currentWidth = currentRectTransform.rect.width;
            var holderHeight = rectHolder.rect.height;
            var holderWidth = rectHolder.rect.width;
            var fixedHeight = pos.y / (defaultResolution.y / 2f) * (currentHeight / 2f);
            pos.y = fixedHeight;

            var elementTransform = rectHolder.gameObject.transform;
            var scale = elementTransform.localScale;
            var position = elementTransform.localPosition;
            rectHolder.sizeDelta = new Vector2(rectHolder.sizeDelta.x,
                holderHeight / (defaultResolution.y / 2f) * (currentHeight / 2f));
           
            /*scale.y = scale.y / (defaultResolution.y / 2f) * (currentHeight / 2f);
            scale.x = scale.x / (defaultResolution.y / 2f) * (currentHeight / 2f);
            var offset = Mathf.Abs(holderWidth/(defaultResolution.y / 2f) * (currentHeight / 2f)-holderWidth)/2;
            Debug.Log($"position.x {position.x}, offset: {offset}");
            position.x -= offset;
            elementTransform.localPosition = position;
            elementTransform.localScale = scale;*/
        }
    }
}
