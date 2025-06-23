using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StarCloudgamesLibrary
{
    public class ToggleObjects : MonoBehaviour
    {
        public GameObject[] objects;

        public void SetObjects(Toggle toggle)
        {
            objects[0].SetActive(!toggle.isOn);
            objects[1].SetActive(toggle.isOn);
        }
    }
}