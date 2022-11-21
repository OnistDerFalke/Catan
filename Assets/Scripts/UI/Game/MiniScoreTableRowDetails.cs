using System.Security.Principal;
using DataStorage;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Game
{
    public class MiniScoreTableRowDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Details Info")][Space(5)]
        [Tooltip("Details Content")][SerializeField] 
        private Text detailsContent;
        [Tooltip("Details Window")][SerializeField] 
        private GameObject detailsWindow;
        [Tooltip("Player Index")][SerializeField] 
        private int playerIndex;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GameManager.TakingPlayer != -1) return;
            var windowPos = gameObject.transform.position;
            var size = detailsWindow.GetComponent<RectTransform>().sizeDelta;
            var offsetPos = gameObject.transform.localPosition + new Vector3(-2f*size.x/4f, size.y*0.9f, 0);
            detailsWindow.transform.position = windowPos;
            detailsWindow.transform.localPosition = offsetPos;

            string playerName = GameManager.State.Players[playerIndex].name.Contains("Gracz") ?
                $"{playerIndex + 1}" : GameManager.State.Players[playerIndex].name;
            detailsContent.text = $"Punktacja gracza {playerName}";
            detailsWindow.SetActive(true);
            GameManager.TakingPlayer = playerIndex;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!eventData.fullyExited) return;
            if (GameManager.TakingPlayer != playerIndex) return;
            detailsWindow.SetActive(false);
            GameManager.TakingPlayer = -1;
        }
    }
}
