using DataStorage;
using UnityEngine;

namespace Tests
{
    public class EndGamePopupTest : MonoBehaviour
    {
        [Tooltip("Test Key Code")] [SerializeField] private KeyCode testKey;
        
        private bool invoked;
        private void Invoke()
        {
            GameManager.PopupsShown[GameManager.END_GAME_POPUP] = !invoked;
        }

        void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                Invoke();
                invoked = !invoked;
            }
        }
    }
}