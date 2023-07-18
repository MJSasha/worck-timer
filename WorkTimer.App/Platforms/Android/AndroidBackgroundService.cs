﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using WorkTimer.App.Services;

namespace WorkTimer.App.Platforms.Android
{
    [Service]
    public class AndroidBackgroundService : Service, IBackgroundService
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                RegisterNotification();
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                StopForeground(true);
                StopSelfResult(startId);
            }

            return StartCommandResult.NotSticky;
        }

        public void StartBackgroundProcess()
        {
            Intent startService = new(MainActivity.ActivityCurrent, typeof(AndroidBackgroundService));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }

        public void StopBackgroundProcess()
        {
            Intent stopIntent = new Intent(MainActivity.ActivityCurrent, Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }

        private void RegisterNotification()
        {
            NotificationChannel channel = new("ServiceChannel", "ServiceDemo", NotificationImportance.Max);
            NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(NotificationService);
            manager.CreateNotificationChannel(channel);
            Notification notification = new Notification.Builder(this, "ServiceChannel")
               .SetContentTitle("Service Working")
               .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
               .SetOngoing(true)
               .Build();

            StartForeground(100, notification);
        }
    }
}