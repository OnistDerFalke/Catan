using DataStorage;
using System.Collections;
using Board.States;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public enum ActiveContent
        {
            Actions,
            Cards,
            Pricing,
            None
        }
        
        //Destiny: Tabs buttons
        [Header("Tabs buttons")][Space(5)]
        [Tooltip("Action tab button")]
        [SerializeField] private Button actionsButton;
        [Tooltip("Cards tab button")]
        [SerializeField] private Button cardsButton;
        [Tooltip("Pricing tab button")]
        [SerializeField] private Button pricingButton;

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
        
        //Destiny: Tabs buttons sprites
        [Header("Tabs buttons sprites")][Space(5)]
        [Tooltip("Active button sprite")] [SerializeField] private Sprite activeButtonSprite;
        [Tooltip("Inactive button sprite")] [SerializeField] private Sprite inactiveButtonSprite;
        
        //Destiny: Tabs buttons images
        [Header("Tabs buttons images")][Space(5)]
        [Tooltip("Actions button image")] [SerializeField] private Image actionsButtonImage;
        [Tooltip("Cards button image")] [SerializeField] private Image cardsButtonImage;
        [Tooltip("Pricing button image")] [SerializeField] private Image pricingButtonImage;
        
        //Destiny: Canvas Rect
        [Header("Screen Resolution Elements")][Space(5)]
        [Tooltip("Canvas Rect")] [SerializeField] private RectTransform canvasRect;

        public ActiveContent activeContent;
        private bool isNowSliding;
        private float slidingUIAnimationBorderRight;
        private SlideState state;

        private Vector3 actionsButtonPosition;
        private Vector3 cardsButtonPosition;
        private Vector3 pricingButtonPosition;

        public void OnActionButtonClick()
        {
            if (isNowSliding) return;
            var lastActiveContent = activeContent;
            activeContent = ActiveContent.Actions;
            HideAllContents();
            actionsContent.SetActive(true);

            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                { 
                    if(lastActiveContent == activeContent)
                        StartCoroutine(SlideOff());
                    break;
                }
            }
            
            VisualiseTabsActivity();
        }
        
        public void OnCardsButtonClick()
        {
            if (isNowSliding) return;
            var lastActiveContent = activeContent;
            activeContent = ActiveContent.Cards;
            HideAllContents();
            cardsContent.SetActive(true);
            
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    if(lastActiveContent == activeContent)
                        StartCoroutine(SlideOff());
                    break;
            }
            
            VisualiseTabsActivity();
        }
        
        public void OnPricingButtonClick()
        {
            if (isNowSliding) return;
            var lastActiveContent = activeContent;
            activeContent = ActiveContent.Pricing;
            HideAllContents(); 
            pricingContent.SetActive(true);
            
            switch (state)
            {
                case SlideState.SlidedOff:
                    StartCoroutine(SlideOn());
                    break;
                case SlideState.SlidedOn:
                    if(lastActiveContent == activeContent)
                        StartCoroutine(SlideOff());
                    break;
            }
            
            VisualiseTabsActivity();
        }

        private void HideAllContents()
        {
            actionsContent.SetActive(false);
            cardsContent.SetActive(false);
            pricingContent.SetActive(false);
        }
        
        private void VisualiseTabsActivity()
        {
            actionsButtonImage.sprite = inactiveButtonSprite;
            cardsButtonImage.sprite = inactiveButtonSprite;
            pricingButtonImage.sprite = inactiveButtonSprite;

            switch (activeContent)
            {
                case ActiveContent.Actions:
                {
                    actionsButtonImage.sprite = activeButtonSprite;
                    break;
                }
                case ActiveContent.Cards:
                {
                    cardsButtonImage.sprite = activeButtonSprite;
                    break;
                }
                case ActiveContent.Pricing:
                {
                    pricingButtonImage.sprite = activeButtonSprite;
                    break;
                }
            }
        }
        
        

        IEnumerator SlideOn()
        {
            var buttonFromTabDelta = slidingUI.transform.localPosition.x - actionsButton.transform.localPosition.x;
            
            isNowSliding = true;

            while (slidingUI.transform.localPosition.x >= slidingUIAnimationBorderLeft)
            {
                GameManager.PopupManager.PopupOffset -= slidingUIAnimationSpeed * Time.deltaTime;
                
                slidingUI.transform.localPosition -= new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                if (slidingUI.transform.localPosition.x < slidingUIAnimationBorderLeft)
                {
                    SetTabPositionEqualToBorder(slidingUIAnimationBorderLeft, buttonFromTabDelta);
                    break;
                }
                actionsButton.transform.localPosition -= new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                cardsButton.transform.localPosition -= new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                pricingButton.transform.localPosition -= new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }

            state = SlideState.SlidedOn;
            isNowSliding = false;
        }
        
        IEnumerator SlideOff()
        {
            var buttonFromTabDelta = slidingUI.transform.localPosition.x - actionsButton.transform.localPosition.x;
            
            isNowSliding = true;
            activeContent = ActiveContent.None;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            VisualiseTabsActivity();
            while (slidingUI.transform.localPosition.x < slidingUIAnimationBorderRight)
            {
                slidingUI.transform.localPosition += new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                GameManager.PopupManager.PopupOffset += slidingUIAnimationSpeed * Time.deltaTime;
                if (slidingUI.transform.localPosition.x < slidingUIAnimationBorderLeft)
                {
                    SetTabPositionEqualToBorder(slidingUIAnimationBorderRight, buttonFromTabDelta);
                    break;
                }
                if(actionsButton.transform.localPosition.x < actionsButtonPosition.x) { }
                    actionsButton.transform.localPosition += new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                if(cardsButton.transform.localPosition.x < cardsButtonPosition.x)
                    cardsButton.transform.localPosition += new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                if (pricingButton.transform.localPosition.x < pricingButtonPosition.x)
                    pricingButton.transform.localPosition += new Vector3(slidingUIAnimationSpeed * Time.deltaTime, 0, 0);
                yield return new WaitForSeconds(slidingUIAnimationSmoothness);
            }

            state = SlideState.SlidedOff;
            isNowSliding = false;
        }

        private void SetTabPositionEqualToBorder(float borderX, float buttonDelta)
        {
            var slidingFinalPos = slidingUI.transform.localPosition;
            var actionsButtonPos = actionsButton.transform.localPosition;
            var cardsButtonPos = cardsButton.transform.localPosition;
            var pricingButtonPos = pricingButton.transform.localPosition;
            slidingFinalPos.x = borderX;
            actionsButtonPos.x = borderX - buttonDelta;
            cardsButtonPos.x = borderX - buttonDelta;
            pricingButtonPos.x = borderX - buttonDelta;
            slidingUI.transform.localPosition = slidingFinalPos;
            actionsButton.transform.localPosition = actionsButtonPos;
            cardsButton.transform.localPosition = cardsButtonPos;
            pricingButton.transform.localPosition = pricingButtonPos;
        }
        
        void Start()
        {
            activeContent = ActiveContent.None;
            isNowSliding = false;
            state = SlideState.SlidedOff;
            GameManager.PopupManager.PopupOffset = 0;
            slidingUIAnimationBorderRight = slidingUI.transform.localPosition.x;
            slidingUIAnimationBorderLeft = slidingUIAnimationBorderLeft/1920 * canvasRect.rect.width;
            
            actionsButtonPosition = actionsButton.transform.localPosition;
            cardsButtonPosition = cardsButton.transform.localPosition;
            pricingButtonPosition = pricingButton.transform.localPosition;

            actionsButton.onClick.AddListener(OnActionButtonClick);
            cardsButton.onClick.AddListener(OnCardsButtonClick);
            pricingButton.onClick.AddListener(OnPricingButtonClick);

            HideAllContents();
        }
    }
}
