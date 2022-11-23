using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class LogsManager : MonoBehaviour
    {
        [SerializeField] private Text logsText;
        [SerializeField] private int rowsLimit;
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject info;
        
        void Awake()
        {
            info.SetActive(true);
            content.SetActive(false);
            logsText.text = "";
        }

        void Update()
        {
            UpdateLogs();

            if (Input.GetKeyDown(KeyCode.L))
            {
                content.SetActive(!content.activeSelf);
                info.SetActive(!info.activeSelf);
            }
        }

        private void UpdateLogs()
        {
            logsText.text = "";
            if (GameManager.Logs.Count < rowsLimit)
            {
                for (var i = 0; i < rowsLimit - GameManager.Logs.Count; i++)
                    logsText.text += '\n';

                for (var i = GameManager.Logs.Count - 1; i >= 0; i--)
                    logsText.text += "◎ " + GameManager.Logs[GameManager.Logs.Count - 1 - i]  + '\n';

            }
            else
            {
                for (var i = rowsLimit - 1; i >= 0; i--)
                    logsText.text +=  "◎ " + GameManager.Logs[GameManager.Logs.Count - 1 - i] + '\n';
            }
        }
    }
}
