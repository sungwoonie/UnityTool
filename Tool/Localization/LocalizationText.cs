using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class LocalizationText : MonoBehaviour
    {
        public string key;

        private TMP_Text tmpText;

        private void Awake()
        {
            InitializeComponent();
        }

        private void Start()
        {
            Localize();
        }

        private void InitializeComponent()
        {
            tmpText = GetComponent<TMP_Text>();
        }

        public void Localize()
        {
            if (!string.IsNullOrEmpty(key))
            {
                tmpText.text = LocalizationManager.instance.GetLocalizedString(key);
            }
        }

        public void SetText(string text)
        {
            tmpText.text = text;
        }
    }

}