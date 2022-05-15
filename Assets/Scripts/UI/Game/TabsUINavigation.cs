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
        
        //Destiny: Tabs content
        [Header("Tabs")][Space(5)]
        [Tooltip("Action content")]
        [SerializeField] private GameObject actionsContent;
        [Tooltip("Cards content")]
        [SerializeField] private GameObject cardsContent;
        [Tooltip("Pricing content")]
        [SerializeField] private GameObject pricingContent;
        [Tooltip("Score content")]
        [SerializeField] private GameObject scoreContent;

        private bool isNowSliding;
        private float slidingUIAnimationBorderRight;
        private SlideState state;

        private Vector3 actionsButtonPosition;
        private Vector3 cardsButtonPosition;
        private Vector3 pricingButtonPosition;
        private Vector3 scoreButtonPosition;

        private void OnActionButtonClick()
        {
            if (isNowSliding) return;

            //Destiny: If changing the tab, content should be replaced
            if (state == SlideState.SlidedOff)
            {
                HideAllContents();
                actionsContent.SetActive(true);
            }

            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn(actionsButton));
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff(actionsButton, actionsButtonPosition));
                    break;
            }
        }
        
        private void OnCardsButtonClick()
        {
            if (isNowSliding) return;

            //Destiny: If changing the tab, content should be replaced
            if (state == SlideState.SlidedOff)
            {
                HideAllContents();
                cardsContent.SetActive(true);
            }

            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn(cardsButton));
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff(cardsButton, cardsButtonPosition));
                    break;
            }
        }
        
        private void OnPricingButtonClick()
        {
            if (isNowSliding) return;

            //Destiny: If changing the tab, content should be replaced
            if (state == SlideState.SlidedOff)
            {
                HideAllContents();
                pricingContent.SetActive(true);
            }

            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn(pricingButton));
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff(pricingButton, pricingButtonPosition));
                    break;
            }
        }
        
        private void OnScoreButtonClick()
        {
            if (isNowSliding) return;

            //Destiny: If changing the tab, content should be replaced
            if (state == SlideState.SlidedOff)
            {
                HideAllContents();
                scoreContent.SetActive(true);
            }

            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn(scoreButton));
                    break;
                case SlideState.SlidedOn:
                    StartCoroutine(SlideOff(scoreButton, scoreButtonPosition));
                    break;
            }
        }

        private void HideAllContents()
        {
            actionsContent.SetActive(false);
            cardsContent.SetActive(false);
            pricingContent.SetActive(false);
            scoreContent.SetActive(false);
        }

        IEnumerator SlideOn(Button button)
        {
            isNowSliding = true;

            while (slidingUI.transform.localPosition.x >= slidingUIAnimationBorderLeft)
            {
                slidingUI.transform.localPosition -= new Vector3(slidingUIAnimationSpeed, 0, 0);
                button.transform.localPosition -= new Vector3(slidingUIAnimationSpeed, 0, 0);
                yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }

            state = SlideState.SlidedOn;
            isNowSliding = false;
        }
        
        IEnumerator SlideOff(Button button, Vector3 tempButtonPosition)
        {
            isNowSliding = true;

            while (slidingUI.transform.localPosition.x <= slidingUIAnimationBorderRight)
            {
                    slidingUI.transform.localPosition += new Vector3(slidingUIAnimationSpeed, 0, 0);
                    button.transform.localPosition += new Vector3(slidingUIAnimationSpeed, 0, 0);
                    yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }
            button.transform.localPosition = tempButtonPosition;

            state = SlideState.SlidedOff;
            isNowSliding = false;
        }
        
        void Start()
        {
            isNowSliding = false;
            state = SlideState.SlidedOff;
            slidingUIAnimationBorderRight = slidingUI.transform.localPosition.x;

            actionsButtonPosition = actionsButton.transform.localPosition;
            cardsButtonPosition = cardsButton.transform.localPosition;
            pricingButtonPosition = pricingButton.transform.localPosition;
            scoreButtonPosition = scoreButton.transform.localPosition;
            
            actionsButton.onClick.AddListener(OnActionButtonClick);
            cardsButton.onClick.AddListener(OnCardsButtonClick);
            pricingButton.onClick.AddListener(OnPricingButtonClick);
            scoreButton.onClick.AddListener(OnScoreButtonClick);
            
            HideAllContents();
        }
    }
}
