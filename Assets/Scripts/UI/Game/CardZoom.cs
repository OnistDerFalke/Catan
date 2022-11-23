using UnityEngine;

namespace UI.Game
{
    public class CardZoom : MonoBehaviour
    {
        private bool isZoomed;
        private const float ZoomValue = 1.1f;

        public void PointerEnter()
        {
            if (isZoomed)
            {
                return;
            }
            gameObject.transform.localScale *= ZoomValue;
            isZoomed = true;
        }

        public void PointerExit()
        {
            if (!isZoomed)
            {
                return;
            }
            gameObject.transform.localScale /= ZoomValue;
            isZoomed = false;
        }
    }
}