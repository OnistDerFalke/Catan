using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    //Destiny: Navigating the tabs UI on right side of the screen
    public class TabsUINavigation : MonoBehaviour
    {
        private enum SlideState
        {
            SlidedOff,
            SlidedOn
        }
        
        //Destiny: Tabs buttons
        [Header("Tabs buttons")][Space(5)]
        [Tooltip("Action tab button")]
        [SerializeField] private Button actionsButton;
        [Tooltip("Cards tab button")]
        [SerializeField] private Button cardsButton;
        [Tooltip("Pricing tab button")]
        [SerializeField] private Button pricingButton;
        [Tooltip("Score tab button")]
        [SerializeField] private Button scoreButton;
        
        //Destiny: Sliding UI (image)
        [Header("Sliding UI")][Space(5)]
        [Tooltip("Sliding UI")]
        [SerializeField] private Image slidingUI;
        [Tooltip("Sliding UI animation border (max x that it can slide to, then stops)")]
        [SerializeField] private float slidingUIAnimationBorderLeft;
        [Tooltip("Sliding UI smoothness - lower makes animation more smooth")]
        [SerializeField] private float slidingUIAnimationSmoothness;
        [Tooltip("Sliding UI animation speed")]
        [SerializeField] private float slidingUIAnimationSpeed;

        private bool isNowSliding;
        private float slidingUIAnimationBorderRight;
        private SlideState state;

        private void OnActionButtonClick()
        {
            if (isNowSliding) return;
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff());
                    break;
            }
        }
        
        private void OnCardsButtonClick()
        {
            if (isNowSliding) return;
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff());
                    break;
            }
        }
        
        private void OnPricingButtonClick()
        {
            if (isNowSliding) return;
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff());
                    break;
            }
        }
        
        private void OnScoreButtonClick()
        {
            if (isNowSliding) return;
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff());
                    break;
            }
        }

        IEnumerator SlideOn()
        {
            isNowSliding = true;
            
            while (slidingUI.transform.localPosition.x >= slidingUIAnimationBorderLeft)
            {
                slidingUI.transform.localPosition -= new Vector3(slidingUIAnimationSpeed, 0, 0);
                yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }

            state = SlideState.SlidedOn;
            isNowSliding = false;
        }
        
        IEnumerator SlideOff()
        {
            isNowSliding = true;
            while (slidingUI.transform.localPosition.x <= slidingUIAnimationBorderRight)
            {
                    slidingUI.transform.localPosition += new Vector3(slidingUIAnimationSpeed, 0, 0);
                    yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }

            state = SlideState.SlidedOff;
            isNowSliding = false;
        }
        
        void Start()
        {
            isNowSliding = false;
            state = SlideState.SlidedOff;
            slidingUIAnimationBorderRight = slidingUI.transform.localPosition.x;
            
            actionsButton.onClick.AddListener(OnActionButtonClick);
            cardsButton.onClick.AddListener(OnCardsButtonClick);
            pricingButton.onClick.AddListener(OnPricingButtonClick);
            scoreButton.onClick.AddListener(OnScoreButtonClick);
        }
    }
}
