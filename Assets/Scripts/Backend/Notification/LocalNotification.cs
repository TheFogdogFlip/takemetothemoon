using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class LocalNotification
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";
    private static string unityClass = "com.unity3d.player.UnityPlayerNativeActivity";
#elif UNITY_IOS && !UNITY_EDITOR
    private Dictionary notificationDictionary = new Dictionary<string, string>();
#endif
    public static void SendNotification(int id, long delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("SetNotification", id, unityClass, delay, title, message, message, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        var notif = new UnityEngine.iOS.LocalNotification();
        notif.fireDate = System.DateTime.Now.AddSeconds(delay);
        notif.alertBody = message;
        notif.hasAction = true;
        notif.alertAction = title;
        notif.applicationIconBadgeNumber = 1;
        notif.soundName = LocalNotification.defaultSoundName;
        notif.userInfo = notificationDictionary;
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
#endif
    }
    public static void SendRepeatingNotification(int id, long delay, long timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("SetRepeatingNotification", id, unityClass, delay, title, message, message, timeout * 1000, sound ? 1 : 0, vibrate ? 1 : 0, lights ? 1 : 0, bigIcon, "notify_icon_small", bgColor.r * 65536 + bgColor.g * 256 + bgColor.b);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        var notif = new UnityEngine.iOS.LocalNotification();
        notif.fireDate = System.DateTime.Now.AddSeconds(delay);
        notif.alertBody = message;
        notif.hasAction = true;
        notif.alertAction = title;
        notif.applicationIconBadgeNumber = 1;
        notif.soundName = LocalNotification.defaultSoundName;
        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
#endif
    }
    public static void CancelNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass pluginClass = new AndroidJavaClass(fullClassName);
        if (pluginClass != null) {
            pluginClass.CallStatic("CancelNotification", id);
        }
#elif UNITY_IOS && !UNITY_EDITOR
        if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) {
            UnityEngine.iOS.LocalNotification.applicationIconBadgeNumber = -1;
            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        }
#endif
    }
}
