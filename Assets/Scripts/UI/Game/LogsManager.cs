using System.Collections.Generic;
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
        
        private List<string> logsList;
        void Awake()
        {
            info.SetActive(true);
            content.SetActive(false);
            logsText.text = "";
            logsList = new List<string>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                content.SetActive(!content.activeSelf);
                info.SetActive(!info.activeSelf);
            }
        }

        private void UpdateLogs()
        {
            logsText.text = "";
            if (logsList.Count < rowsLimit)
            {
                for (var i = 0; i < rowsLimit - logsList.Count; i++)
                    logsText.text += '\n';

                for (var i = logsList.Count - 1; i >= 0; i--)
                    logsText.text += "◎ " + logsList[logsList.Count - 1 - i]  + '\n';

            }
            else
            {
                for (var i = rowsLimit - 1; i >= 0; i--)
                    logsText.text +=  "◎ " + logsList[logsList.Count - 1 - i] + '\n';
            }
        }

        public void Log(string log)
        {
            logsList.Add(log);
            UpdateLogs();
        }
    }
}
