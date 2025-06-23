using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public static class Yielder
    {
        private static Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();
        private static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        private static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();

        public static WaitForSeconds WaitForSeconds(float time)
        {
            if(waitForSeconds.TryGetValue(time, out WaitForSeconds result))
            {
                return result;
            }
            else
            {
                WaitForSeconds newWaitForSeconds = new WaitForSeconds(time);
                waitForSeconds.Add(time, newWaitForSeconds);
                return newWaitForSeconds;
            }
        }

        public static WaitForEndOfFrame EndOfFrame
        {
            get { return endOfFrame; }
        }

        public static WaitForFixedUpdate FixedUpdate
        {
            get { return fixedUpdate; }
        }
    }
}