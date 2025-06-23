using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Android;
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

namespace StarCloudgamesLibrary
{
    public class NotificationManager : SingleTon<NotificationManager>
    {
        public List<NotificationChannel> commonNotifications;

        public bool canNotify;
        public bool canNotifyAtNight;

        #region "Unity"

        private void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidNotificationCenter.Initialize();
#endif

            CheckPermission();
        }

        private void OnDisable()
        {
            if(canNotify)
            {
                SetActions(false);
            }
        }

        #endregion

        #region "Action"

        private void SetActions(bool add)
        {
            if(add)
            {
                DebugManager.DebugInGameMessage("Add Notification Actions");
                ApplicationController.instance.AddAction(SystemActionType.Pause, RefreshNotifications);
                ApplicationController.instance.AddAction(SystemActionType.Resume, RemoveCommonNotifications);
                ApplicationController.instance.AddAction(SystemActionType.Quit, RefreshNotifications);
            }
            else
            {
                DebugManager.DebugInGameMessage("Remove Notification Actions");
                ApplicationController.instance.RemoveAction(SystemActionType.Pause, RefreshNotifications);
                ApplicationController.instance.RemoveAction(SystemActionType.Resume, RemoveCommonNotifications);
                ApplicationController.instance.RemoveAction(SystemActionType.Quit, RefreshNotifications);
            }
        }

        #endregion

        #region "Notification Permission"

        private void CheckPermission()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            canNotify = Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS");
#elif UNITY_IOS && !UNITY_EDITOR
            canNotify = iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Authorized;
#endif

            canNotifyAtNight = AntiCheatManager.instance.Get("CanNotifyAtNight", false);
        }

        public void SetNotification(bool notify, bool nightNotify)
        {
            if(notify && !canNotify)
            {
                SetActions(true);
            }
            else if(!notify && canNotify)
            {
                SetActions(false);
                RemoveAllScheduledNotification();
            }

            canNotify = notify;

            if(nightNotify != canNotifyAtNight)
            {
                AntiCheatManager.instance.Set("CanNotifyAtNight", nightNotify);
            }
            canNotifyAtNight = nightNotify;
        }

        public void AskNotificationPermission(Action<bool> action)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AskAOSPermission(action);
#elif UNITY_IOS && !UNITY_EDITOR
            StartCoroutine(AskIOSPermission(action));
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR

        private void AskAOSPermission(Action<bool> action)
        {
            if(!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                var permissionCallback = new PermissionCallbacks();
                permissionCallback.PermissionGranted += _ =>
                {
                    action(true);
                };
                permissionCallback.PermissionDenied += _ =>
                {
                    action(false);
                    RemoveAllScheduledNotification();
                };
                permissionCallback.PermissionDeniedAndDontAskAgain += _ =>
                {
                    action(false);
                    RemoveAllScheduledNotification();
                };

                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", permissionCallback);
            }
            else
            {
                action(true);
            }
        }

#elif UNITY_IOS && !UNITY_EDITOR

        private IEnumerator AskIOSPermission(Action<bool> action)
        {
            if(iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Authorized)
            {
                action(true);
                yield break;
            }

            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using(var req = new AuthorizationRequest(authorizationOption, true))
            {
                while(!req.IsFinished)
                {
                    yield return null;
                };

                action(req.Granted);
            }
        }

#endif

        #endregion

        #region "Remove Notification"

        private void RemoveAllScheduledNotification()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS && !UNITY_EDITOR
            iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
        }

        private void RemoveCommonNotifications()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var commonNotifies = AndroidNotificationCenter.GetNotificationChannels().ToList().FindAll(x => x.Id.Contains("Common"));
            if(commonNotifies != null && commonNotifies.Count > 0)
            {
                commonNotifies.ForEach(x => AndroidNotificationCenter.DeleteNotificationChannel(x.Id));
            }
#elif UNITY_IOS && !UNITY_EDITOR
            var commonNotifies = iOSNotificationCenter.GetScheduledNotifications().ToList().FindAll(x => x.Identifier.Contains("Common"));
            if(commonNotifies != null && commonNotifies.Count > 0)
            {
                commonNotifies.ForEach(x => iOSNotificationCenter.RemoveScheduledNotification(x.Identifier));
            }
#endif
        }

        #endregion

        #region "Register Notification"

        public void RegisterNotification(NotificationChannel channel)
        {
            if(!CanNotify(channel.fireMinute))
            {
                DebugManager.DebugInGameWarningMessage($"Cannot send Notification.");
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            var newAOSNofitication = new AndroidNotification()
            {
                Title = LocalizationManager.instance.GetLocalizedString(channel.title),
                Text = LocalizationManager.instance.GetLocalizedString(channel.explain),
                FireTime = DateTime.Now.AddMinutes(channel.fireMinute),

                SmallIcon = "icon_0",
                LargeIcon = "icon_1",
                ShowInForeground = false
            };

            if(string.IsNullOrEmpty(AndroidNotificationCenter.GetNotificationChannel(channel.channelID).Id))
            {
                var aosChannel = new AndroidNotificationChannel()
                {
                    Id = channel.channelID,
                    Name = channel.title,
                    Importance = Importance.Default,
                    Description = channel.channelDescription
                };

                AndroidNotificationCenter.RegisterNotificationChannel(aosChannel);
                DebugManager.DebugInGameMessage($"{aosChannel.Id} registered");
            }

            AndroidNotificationCenter.SendNotification(newAOSNofitication, channel.channelID);
            DebugManager.DebugInGameMessage($"{newAOSNofitication.Title} scheduled. fireTime : {DateTime.Now.AddMinutes(channel.fireMinute)}");

#elif UNITY_IOS && !UNITY_EDITOR
            var newTimeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, channel.fireMinute, 0),
                Repeats = false
            };

            var newIOSNofitication = new iOSNotification()
            {
                Identifier = channel.channelID,
                Title = LocalizationManager.instance.GetLocalizedString(channel.title),
                Body = LocalizationManager.instance.GetLocalizedString(channel.explain),
                Trigger = newTimeTrigger,

                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "default_category",
                ThreadIdentifier = "thread1",
                ShowInForeground = false
            };

            iOSNotificationCenter.ScheduleNotification(newIOSNofitication);

            DebugManager.DebugInGameMessage($"{newIOSNofitication.Identifier} scheduled. fireTime : {DateTime.Now.AddMinutes(channel.fireMinute)}");
#endif
        }

        #endregion

        private void RefreshNotifications()
        {
            RemoveCommonNotifications();
            commonNotifications.ForEach(x => RegisterNotification(x));
        }

        private bool CanNotify(int fireTime)
        {
            if(!canNotify)
            {
                return false;
            }
            else
            {
                var fireHour = DateTime.Now.AddMinutes(fireTime).Hour;

                if(fireHour < 6 || fireHour > 22)
                {
                    if(!canNotifyAtNight)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }

    [System.Serializable]
    public struct NotificationChannel
    {
        public string title;
        public string explain;
        public int fireMinute;
        public string channelID;
        public string channelDescription;
    }
}