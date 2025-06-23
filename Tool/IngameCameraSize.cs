using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class IngameCameraSize : MonoBehaviour
    {
        public readonly float minimumSize = 3.5f;

        private readonly float multiply = 3.0f;
        private readonly float offset = 0.5f;

        private Camera targetCamera;

        private void OnEnable()
        {
            SetCameraSize();
        }

        private void SetCameraSize()
        {
            targetCamera = GetComponent<Camera>();
            var modifiedSize = (multiply * ((float)Screen.height / Screen.width)) - offset;
            targetCamera.orthographicSize = Mathf.Clamp(modifiedSize, minimumSize, 10);
        }
    }
}
