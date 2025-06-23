using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    public static class Local
    {
        public static LocalCode GetLocalCode()
        {
            var savedLocal = PlayerPrefs.GetInt("SavedLocal", 0);

            if(savedLocal == 0)
            {
                var currentSystemLanguage = Application.systemLanguage;

                switch(currentSystemLanguage)
                {
                    case SystemLanguage.Korean:
                        SetLocalCode(LocalCode.KR);
                        return LocalCode.KR;
                    case SystemLanguage.Japanese:
                        SetLocalCode(LocalCode.JP);
                        return LocalCode.JP;
                    default:
                        SetLocalCode(LocalCode.EN);
                        return LocalCode.EN;
                }
            }
            else
            {
                return (LocalCode)savedLocal;
            }
        }

        public static void SetLocalCode(LocalCode code)
        {
            PlayerPrefs.SetInt("SavedLocal", (int)code);
        }
    }

    public enum LocalCode
    {
        None, KR, EN, JP
    }
}