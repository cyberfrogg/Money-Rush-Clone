using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace Notifications
{
    public class MobileNotifications : MonoBehaviour
    {
        private void Awake()
        {
            AndroidNotificationChannel chanel = new AndroidNotificationChannel()
            {
                Id = "default_chanel",
                Name = "Default Chanel",
                Description = "For generic notifications",
                Importance = Importance.High
            };

            AndroidNotificationCenter.RegisterNotificationChannel(chanel);

            AndroidNotification notification = new AndroidNotification()
            {
                Title = "Return to Money Rush Clone!",
                Text = "Awaiting you for another session <3",
                SmallIcon = "default",
                LargeIcon = "default",
                FireTime = DateTime.Now.AddHours(2)
            };
        }
    }
}