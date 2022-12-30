using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Navigation
{
    public class ToggleHolder : MonoBehaviour
    {
        [SerializeField] private Button toggleButton;
        [SerializeField] private Image checkImage;

        public bool value = true;

        void Start()
        {
           toggleButton.onClick.AddListener(OnButtonToggleClick);
        }

        private void OnButtonToggleClick()
        {
            value = !value;
            toggleButton.image.color = value ? Color.white : Color.gray;
            checkImage.enabled = value;
        }
    }
}
