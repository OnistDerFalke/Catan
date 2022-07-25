using Microsoft.Win32;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace UI
{
    public class PlacementController : MonoBehaviour
    {
        [Header("Default Aspect")][Space(5)]
        [Tooltip("Default Aspect")] [SerializeField] private Vector2 defaultResolution;
        [Tooltip("Canvas")] [SerializeField] private RectTransform currentRectTransform;
        [Tooltip("Rect Holder")] [SerializeField] private RectTransform rectHolder;
        [Tooltip("Extendable")] [SerializeField] private bool extendable;

        void Start()
        {
           HeightPlacement();
        }

        private void HeightPlacement()
        {
            var pos = transform.localPosition;
            var fixedHeight = pos.y / (defaultResolution.y / 2f) * (currentRectTransform.rect.height / 2f);
            var offset = Mathf.Abs(rectHolder.rect.height/(defaultResolution.y / 2f) * 
                (currentRectTransform.rect.height / 2f)-rectHolder.rect.height)/2;
            pos.y = fixedHeight;
            
            if (extendable)
            {
                var scale = transform.localScale;
                scale.y = scale.y / (defaultResolution.y / 2f) * (currentRectTransform.rect.height / 2f);
                transform.localScale = scale;
            }
            else
            {
                var defaultAspectRatio = defaultResolution.x / defaultResolution.y;
                var currentAspectRatio = currentRectTransform.rect.width / currentRectTransform.rect.height;
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
            }

            transform.localPosition = pos;
        }
    }
}
