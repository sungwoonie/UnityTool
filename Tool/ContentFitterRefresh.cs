using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarCloudgamesLibrary
{
    public class Content_Fitter_Refresh : MonoBehaviour
    {
        public bool refreshOnStart;

        private ContentSizeFitter contentSizeFitter;

        #region "Unity"

        private void Start()
        {
            CheckOnStart();
        }

        #endregion

        #region "Check Start"

        private void CheckOnStart()
        {
            if (refreshOnStart)
            {
                RefreshContentFitters();
            }
        }

        #endregion

        #region "Refresh"

        public void RefreshContentFitters()
        {
            StartCoroutine(Refresh());
        }

        private IEnumerator Refresh()
        {
            yield return new WaitForEndOfFrame();

            if (contentSizeFitter == null)
            {
                contentSizeFitter = GetComponent<ContentSizeFitter>();
            }

            if (contentSizeFitter != null)
            {
                contentSizeFitter.enabled = false;
                contentSizeFitter.enabled = true;
            }
        }

        #endregion
    }
}