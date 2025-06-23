using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace StarCloudgamesLibrary
{
    public class DebugUI : SingleTon<DebugUI>
    {
        private TMP_Text[] debugTexts;
        private List<TMP_Text> activatingTexts = new List<TMP_Text>();

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void Start()
        {
            Initialize();
            Application.logMessageReceived += HandleLog;
        }

        private void Initialize()
        {
            debugTexts = GetComponentsInChildren<TMP_Text>(true);
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            var color = "color=white";
            switch(type)
            {
                case LogType.Log:
                    color = "color=white";
                    break;
                case LogType.Warning:
                    color = "color=yellow";
                    break;
                default:
                    color = "color=red";
                    break;
            }

            var message = $"<{color}>{logMessage} \n {(type == LogType.Log || type == LogType.Warning ? "" : stackTrace)}</color>";
            SetDebugText(message);
        }

        public void SetDebugText(string text)
        {
            foreach (var item in debugTexts)
            {
                if (!item.gameObject.activeSelf)
                {
                    item.text = text;
                    item.gameObject.SetActive(true);
                    activatingTexts.Add(item);
                    item.transform.SetAsFirstSibling();
                    return;
                }
            }

            activatingTexts[0].text = text;
            activatingTexts[0].transform.SetAsFirstSibling();
        }
    }
}