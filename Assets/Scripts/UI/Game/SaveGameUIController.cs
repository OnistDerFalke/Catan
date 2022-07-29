using Assets.Scripts.DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class SaveGameUIController : MonoBehaviour
    {
        //Destiny: Save slots elements
        [Header("Save Slots")][Space(5)]
        [Tooltip("Save Slots Buttons")] [SerializeField] private Button[] saveSlotsButtons;
        [Tooltip("Save Slots Frames")] [SerializeField] private Image[] saveSlotsFrames;
        [Tooltip("Save Slots Names")] [SerializeField] private Text[] saveSlotsNames;
        
        //Destiny: Save game and abort buttons
        [Header("Control Buttons")][Space(5)]
        [Tooltip("Save Game Button")] [SerializeField] private Button saveGameButton;
        [Tooltip("Abort Button")] [SerializeField] private Button abortButton;

        //Destiny: Properties of slot when selected or not
        [Header("Selected Slot Properties")][Space(5)]
        [Tooltip("Standard Scale")] [SerializeField] private Vector3 standardScale;
        [Tooltip("Selected Scale")] [SerializeField] private Vector3 selectedScale;
        [Tooltip("Standard Frame Color")] [SerializeField] private Color standardFrameColor;
        [Tooltip("Selected Frame Color")] [SerializeField] private Color selectedFrameColor;
        
        //Destiny: Slot that is actually selected
        private int selectedSlot;
        
        void Start()
        {
            //Destiny: Features on click abort and save game buttons
            abortButton.onClick.AddListener(() => {gameObject.SetActive(false);});
            saveGameButton.onClick.AddListener(OnSaveGameButton);
            
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
            UpdateSelected();
        }

        void Update()
        {
            //Destiny: Block save game if not chosen
            saveGameButton.interactable = CanSaveGameFromSlot();
        }

        /// <summary>
        /// Updates view of selected element
        /// </summary>
        private void UpdateSelected()
        {
            if (selectedSlot == -1) 
                return;

            foreach (var slot in saveSlotsButtons) 
                slot.gameObject.transform.localScale = standardScale;
            foreach (var slot in saveSlotsFrames) 
                slot.color = standardFrameColor;
            foreach (var slot in saveSlotsNames)
                slot.gameObject.transform.localScale = standardScale;

            saveSlotsButtons[selectedSlot].gameObject.transform.localScale = selectedScale;
            saveSlotsFrames[selectedSlot].color = selectedFrameColor;
            saveSlotsNames[selectedSlot].gameObject.transform.localScale = selectedScale;
        }

        /// <summary>
        /// Defines event after clicking save game button
        /// </summary>
        private void OnSaveGameButton()
        {
            //TODO: Creating the save
            DataManager.Save($"game{selectedSlot}");
        }

        /// <summary>
        /// Checks if game can be saved
        /// </summary>
        /// <returns>If game can be saved</returns>
        private bool CanSaveGameFromSlot()
        {
            if (selectedSlot == -1) 
                return false;
            return true;
        }
    }
}
