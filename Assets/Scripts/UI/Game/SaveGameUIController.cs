using System;
using DataStorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class SaveGameUIController : MonoBehaviour
    {
        //Destiny: Save slots elements
        [Header("Save Slots")][Space(5)]
        [Tooltip("Save Slots Buttons")][SerializeField] 
        private Button[] saveSlotsButtons;
        [Tooltip("Save Slots Frames")][SerializeField] 
        private Image[] saveSlotsFrames;
        [Tooltip("Save Slots Images")][SerializeField]
        private Image[] saveSlotsImages;
        [Tooltip("Save Slots Names")][SerializeField]
        private Text[] saveSlotsNames;
        
        //Destiny: Save game and abort buttons
        [Header("Control Buttons")][Space(5)]
        [Tooltip("Save Game Button")][SerializeField]
        private Button saveGameButton;
        [Tooltip("Abort Button")][SerializeField]
        private Button abortButton;

        //Destiny: Properties of slot when selected or not
        [Header("Selected Slot Properties")][Space(5)]
        [Tooltip("Standard Scale")][SerializeField]
        private Vector3 standardScale;
        [Tooltip("Selected Scale")][SerializeField]
        private Vector3 selectedScale;

        //Destiny: Save game name set elements
        [Header("Save Game Name Set Elements")][Space(5)]
        [Tooltip("Save Game Name Input")][SerializeField]
        private TMP_InputField saveGameNameInput;
        [Tooltip("Save Game Name Tip")][SerializeField] 
        private TMP_Text saveGameNameTip;
        
        //Destiny: Save Elements
        [Header("Save Elements")][Space(5)]
        [Tooltip("Empty Slot Name")][SerializeField]
        private string emptySlotName;
        [Tooltip("Empty Slot Sprite")][SerializeField]
        private Sprite emptySlotSprite;
        [Tooltip("Taken Slot sprite")][SerializeField]
        private Sprite takenSlotSprite;
        [Tooltip("Taken Selected Slot sprite")][SerializeField]
        private Sprite takenSelectedSlotSprite;
        [Tooltip("Taken Selected Slot sprite")][SerializeField] 
        private Sprite unselectedSlotSprite;
        
        //Destiny: Content of popup
        [Header("UI content")][Space(5)]
        [Tooltip("UI content")][SerializeField] 
        private GameObject content;
        
        //Destiny: Override confirm ui
        [Header("Save Game Override Alert UI")][Space(5)]
        [Tooltip("Save Game Override Alert UI")][SerializeField] 
        private GameObject saveGameOverrideAlertUI;
        [Tooltip("Yes Button")][SerializeField]
        private Button yesButton;
        [Tooltip("No Button")][SerializeField]
        private Button noButton;
        
        //Destiny: Save confirm ui
        [Header("Save Game Confirm UI")][Space(5)]
        [Tooltip("Save Game Confirm UI")][SerializeField] 
        private GameObject saveGameConfirmUI;
        [Tooltip("OK Button")][SerializeField] 
        private Button okButton;
        [Tooltip("Additional Info Text")][SerializeField]
        private Text additionalInfo;
        [Tooltip("Additional Info Overriden")][SerializeField]
        private string additionalInfoContentOverriden;
        [Tooltip("Additional Info Saved")][SerializeField] 
        private string additionalInfoContentSaved;
        
        
        //Destiny: Slot that is actually selected
        private int selectedSlot;

        void Start()
        {
            //Destiny: Features on click abort and save game buttons
            abortButton.onClick.AddListener(() => {gameObject.SetActive(false);});
            saveGameButton.onClick.AddListener(OnSaveGameButton);
            
            //Destiny: Features on click override confirm UI buttons
            yesButton.onClick.AddListener(SaveGame);
            noButton.onClick.AddListener(() =>
            {
                content.SetActive(true);
                saveGameOverrideAlertUI.SetActive(false);
            });
            
            //Destiny: Features on click save confirm UI buttons
            okButton.onClick.AddListener(() =>
            {
                saveGameConfirmUI.SetActive(false);
                gameObject.SetActive(false);
            });
            
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
            //Destiny: No slot is chosen on start and save name text is cleared
            selectedSlot = -1;
            saveGameNameInput.text = "";
            UpdateSelected();
            UpdateSavesInfos();
            content.SetActive(true);
        }

        void Update()
        {
            //Destiny: Block save game if not chosen
            saveGameButton.interactable = CanSaveGameFromSlot();

            //Destiny: Save game name tip
            saveGameNameTip.text = $"save {DateTime.Now:MM/dd/yyyy h:mm tt}";
        }

        /// <summary>
        /// Updates view of selected element
        /// </summary>
        private void UpdateSelected()
        {
            foreach (var slot in saveSlotsButtons)
            {
                slot.gameObject.transform.localScale = standardScale;
            }

            foreach (var slot in saveSlotsNames)
            {
                slot.gameObject.transform.localScale = standardScale;
            }

            foreach (var slot in saveSlotsFrames)
            {
                slot.gameObject.transform.localScale = standardScale;
            }

            foreach (var slot in saveSlotsFrames)
            {
                slot.sprite = unselectedSlotSprite;
            }

            if (selectedSlot == -1)
            {
                return;
            }

            saveSlotsButtons[selectedSlot].gameObject.transform.localScale = selectedScale;
            saveSlotsNames[selectedSlot].gameObject.transform.localScale = selectedScale;
            saveSlotsFrames[selectedSlot].gameObject.transform.localScale = selectedScale;
            saveSlotsFrames[selectedSlot].sprite = takenSelectedSlotSprite;
        }

        /// <summary>
        /// Updates visible saves slots infos
        /// </summary>
        private void UpdateSavesInfos()
        {
            //Destiny: Updates saves empty names
            foreach (var slotName in saveSlotsNames)
            {
                slotName.text = emptySlotName;
            }

            //Destiny: Updates saves empty images
            foreach (var slotImage in saveSlotsImages)
            {
                slotImage.sprite = emptySlotSprite;
            }

            //Destiny: Updates saves names and images
            foreach (var save in DataManager.GetFiles())
            {
                saveSlotsNames[save.SlotNumber].text = save.Name;
                saveSlotsImages[save.SlotNumber].sprite = takenSlotSprite;
            }
        }

        /// <summary>
        /// Defines event after clicking save game button
        /// </summary>
        private void OnSaveGameButton()
        {
            bool isSelectedSlotTaken = false;
            foreach (var save in DataManager.GetFiles())
            {
                if (selectedSlot == save.SlotNumber)
                {
                    isSelectedSlotTaken = true;
                }
            }

            if (isSelectedSlotTaken)
            {
                content.SetActive(false);
                saveGameOverrideAlertUI.SetActive(true);
                additionalInfo.text = additionalInfoContentOverriden;
            }
            else
            {
                content.SetActive(false);
                SaveGame();
                additionalInfo.text = additionalInfoContentSaved;
            }
        }

        /// <summary>
        /// Saves game on slot chosen
        /// </summary>
        private void SaveGame()
        {
            var saveName = saveGameNameInput.text == "" ? 
                $"save {DateTime.Now:MM/dd/yyyy h:mm tt}" : saveGameNameInput.text;
            DataManager.Save(selectedSlot, saveName);
            saveGameOverrideAlertUI.SetActive(false);
            saveGameConfirmUI.SetActive(true);
        }

        /// <summary>
        /// Checks if game can be saved
        /// </summary>
        /// <returns>If game can be saved</returns>
        private bool CanSaveGameFromSlot()
        {
            if (selectedSlot == -1)
            {
                return false;
            }

            return true;
        }
    }
}
