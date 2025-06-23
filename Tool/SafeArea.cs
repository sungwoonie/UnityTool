using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public class SafeArea : MonoBehaviour
    {
        private ScreenOrientation orientation;
        private RectTransform safeAreaRectTransform;

        private Vector2 minimum_anchor, maximum_anchor;

        private void Awake()
        {
            Initialize();
            orientation = Screen.orientation;
            SetSafeArea();
        }

        private void Initialize()
        {
            safeAreaRectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if(Screen.orientation != orientation)
            {
                SetSafeArea();
            }
        }

        private void SetSafeArea()
        {
            minimum_anchor = Screen.safeArea.min;
            minimum_anchor.x /= Screen.width;
            minimum_anchor.y /= Screen.height;

            maximum_anchor = Screen.safeArea.max;
            maximum_anchor.x /= Screen.width;
            maximum_anchor.y /= Screen.height;


            safeAreaRectTransform.anchorMin = minimum_anchor;
            safeAreaRectTransform.anchorMax = maximum_anchor;

            orientation = Screen.orientation;
        }
    }
}