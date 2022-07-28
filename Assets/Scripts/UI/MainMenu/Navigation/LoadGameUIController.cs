using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Navigation
{
    public class LoadGameUIController : MonoBehaviour
    {
        //Destiny: Save slots elements
        [Header("Save Slots")][Space(5)]
        [Tooltip("Save Slots Buttons")] [SerializeField] private Button[] saveSlotsButtons;
        [Tooltip("Save Slots Frames")] [SerializeField] private Image[] saveSlotsFrames;
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
            UpdateSelected();
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
        /// Defines event after clicking load game button
        /// </summary>
        private void OnLoadGameButton()
        {
            //TODO: Loading save from slot chosen
            //TODO: Passing to game scene somehow and setting loaded game state
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
