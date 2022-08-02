using DataStorage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu.Navigation
{
    public class LoadGameUIController : MonoBehaviour
    {
        //Destiny: Save slots elements
        [Header("Save Slots")][Space(5)]
        [Tooltip("Save Slots Buttons")] [SerializeField] private Button[] saveSlotsButtons;
        [Tooltip("Save Slots Frames")] [SerializeField] private Image[] saveSlotsFrames;
        [Tooltip("Save Slots Images")] [SerializeField] private Image[] saveSlotsImages;
        [Tooltip("Save Slots Names")] [SerializeField] private Text[] saveSlotsNames;
        
        //Destiny: Load game and abort buttons
        [Header("Control Buttons")][Space(5)]
        [Tooltip("Load Game Button")] [SerializeField] private Button loadGameButton;
        [Tooltip("Abort Button")] [SerializeField] private Button abortButton;

        //Destiny: Properties of slot when selected or not
        [Header("Selected Slot Properties")][Space(5)]
        [Tooltip("Standard Scale")] [SerializeField] private Vector3 standardScale;
        [Tooltip("Selected Scale")] [SerializeField] private Vector3 selectedScale;
        [Tooltip("Standard Frame Color")] [SerializeField] private Color standardFrameColor;
        [Tooltip("Selected Frame Color")] [SerializeField] private Color selectedFrameColor;
        
        //Destiny: Main Menu Navigation script holder
        [Header("Main Menu Navigation script holder")][Space(5)]
        [Tooltip("Main Menu Navigation script holder")] [SerializeField] private MainMenuNavigation mmnHolder;
        
        //Destiny: Save Elements
        [Header("Save Elements")][Space(5)]
        [Tooltip("Empty Slot Name")] [SerializeField] private string emptySlotName;
        [Tooltip("Empty Slot Sprite")] [SerializeField] private Sprite emptySlotSprite;
        [Tooltip("Taken Slot sprite")] [SerializeField] private Sprite takenSlotSprite;
        
        //Destiny: Slot that is actually selected
        private int selectedSlot;
        
        void Start()
        {
            //Destiny: Features on click abort and load game buttons
            abortButton.onClick.AddListener(OnAbortButton);
            loadGameButton.onClick.AddListener(OnLoadGameButton);
            
            //Destiny: Clicking on slot makes it chosen one
            for (var i = 0; i < saveSlotsButtons.Length; i++)
            {
                var slotIndex = i;
                saveSlotsButtons[i].onClick.AddListener(() =>
                {
                    //Destiny: Updates and set selected slot
                    selectedSlot = slotIndex;
                    UpdateSelected();
                });
            }
        }

        void OnEnable()
        {
            //Destiny: No slot is chosen on start
            selectedSlot = -1;
            BlockEmptySlots();
            UpdateSelected();
            UpdateSavesInfos();
        }

        void Update()
        {
            //Destiny: Block load game if save cannot be loaded or not chosen
            loadGameButton.interactable = CanLoadGameFromSlot();
        }

        /// <summary>
        /// Updates view of selected element
        /// </summary>
        private void UpdateSelected()
        {
            foreach (var slot in saveSlotsButtons) 
                slot.gameObject.transform.localScale = standardScale;
            foreach (var slot in saveSlotsFrames) 
                slot.color = standardFrameColor;
            foreach (var slot in saveSlotsNames)
                slot.gameObject.transform.localScale = standardScale;
            
            if (selectedSlot == -1 || !saveSlotsButtons[selectedSlot].interactable) 
                return;
            
            saveSlotsButtons[selectedSlot].gameObject.transform.localScale = selectedScale;
            saveSlotsFrames[selectedSlot].color = selectedFrameColor;
            saveSlotsNames[selectedSlot].gameObject.transform.localScale = selectedScale;
        }
        
        /// <summary>
        /// Updates visible saves slots infos
        /// </summary>
        private void UpdateSavesInfos()
        {
            //Destiny: Updates saves names
            foreach (var slotName in saveSlotsNames)
                slotName.text = emptySlotName;
            foreach (var save in DataManager.GetFiles())
                saveSlotsNames[save.SlotNumber].text = save.Name;
            
            //Destiny: Updates saves images
            foreach (var slotImage in saveSlotsImages)
                slotImage.sprite = emptySlotSprite;
            foreach (var save in DataManager.GetFiles())
                saveSlotsImages[save.SlotNumber].sprite = takenSlotSprite;
        }
        
        /// <summary>
        /// Blocks empty slot for loading game
        /// </summary>
        private void BlockEmptySlots()
        {
            foreach (var slot in saveSlotsButtons)
                slot.interactable = false;
            
            foreach (var save in DataManager.GetFiles())
                saveSlotsButtons[save.SlotNumber].interactable = true;
        }

        /// <summary>
        /// Defines event after clicking load game button
        /// </summary>
        private void OnLoadGameButton()
        {
            GameManager.LoadingGame = true;
            GameManager.Setup();
            GameManager.LoadSlotNumber = selectedSlot;

            DataManager.Load();
            SceneManager.LoadScene("Scenes/GameScreen", LoadSceneMode.Single);
        }

        /// <summary>
        /// Defines event after clicking abort button
        /// </summary>
        private void OnAbortButton()
        {
            mmnHolder.UnloadUIZoomAnimation();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Checks if game can be loaded
        /// </summary>
        /// <returns>If game can be loaded</returns>
        private bool CanLoadGameFromSlot()
        {
            if (selectedSlot == -1) 
                return false;
            
            //TODO: If there is no save on slot it should also return false
            
            return true;
        }
    }
}
