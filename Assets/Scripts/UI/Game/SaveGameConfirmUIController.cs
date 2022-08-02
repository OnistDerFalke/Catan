using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class SaveGameConfirmUIController : MonoBehaviour
    {
        //Destiny: Control buttons
        [Header("Control Buttons")][Space(5)]
        [Tooltip("OK Button")] [SerializeField] private Button okButton;

        private void Start()
        {
            okButton.onClick.AddListener(() => {gameObject.SetActive(false);});
        }
    }
}
