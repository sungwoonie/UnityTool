using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public static class DebugManager
    {
        [Conditional("USE_DEBUG")]
        public static void DebugInGameMessage(string debugMessage)
        {
            UnityEngine.Debug.Log(debugMessage);
        }

        [Conditional("USE_DEBUG")]
        public static void DebugInGameWarningMessage(string debugMessage)
        {
            UnityEngine.Debug.LogWarning(debugMessage);
        }

        [Conditional("USE_DEBUG")]
        public static void DebugInGameErrorMessage(string debugMessage)
        {
            UnityEngine.Debug.LogError(debugMessage);
        }

        [Conditional("USE_DEBUG_SERVER")]
        public static void DebugServerMessage(string debugMessage)
        {
            UnityEngine.Debug.Log(debugMessage);
        }

        [Conditional("USE_DEBUG_SERVER")]
        public static void DebugServerWarningMessage(string debugMessage)
        {
            UnityEngine.Debug.LogWarning(debugMessage);
        }

        [Conditional("USE_DEBUG_SERVER")]
        public static void DebugServerErrorMessage(string debugMessage)
        {
            UnityEngine.Debug.LogError(debugMessage);
        }
    }
}