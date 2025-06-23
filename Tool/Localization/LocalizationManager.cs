using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class LocalizationManager : SingleTon<LocalizationManager>
    {
        public TextAsset csvData;

        private Dictionary<string, Dictionary<string, string>> localzationData = new Dictionary<string, Dictionary<string, string>>();

        #region "Unity"

        protected override void Awake()
        {
            base.Awake();

            InitializeCSVData();
        }

        #endregion

        #region "Initialize"

        public bool Initialized()
        {
            return localzationData.Count > 0;
        }

        private void InitializeCSVData()
        {
            localzationData = CSVReader.Read(csvData);
            DebugManager.DebugInGameMessage("Finished Localization Data finished");
        }

        #endregion

        #region "Localize"

        public string GetLocalizedString(string key)
        {
            if (localzationData.ContainsKey(key))
            {
                var localCode = Local.GetLocalCode().ToString();
                if (localzationData[key].ContainsKey(localCode))
                {
                    return localzationData[key][localCode].ToString();
                }
                else
                {
                    DebugManager.DebugServerWarningMessage($"{localCode} is not found in localization data");
                    return key;
                }
            }
            else
            {
                DebugManager.DebugServerWarningMessage($"{key} is not found in localization data");
                return key;
            }
        }

        #endregion
    }
}