#if UNITY_IOS

using System;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Unity.Advertisement.IosSupport;
using Build1.PostMVC.Unity.App.Modules.Async;
using Build1.PostMVC.Unity.App.Modules.Logging;
using UnityEngine.iOS;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    public sealed class AppTransparencyControllerIOS : IAppTransparencyController
    {
        [Log(LogLevel.Warning)] public ILog             Log           { get; set; }
        [Inject]                public IAsyncResolver   AsyncResolver { get; set; }
        [Inject]                public IEventDispatcher Dispatcher    { get; set; }

        public AppTransparencyStatus Status      { get; private set; } = AppTransparencyStatus.NotDetermined;
        public bool                  Initialized { get; private set; }
        
        public bool TrackingAllowed => Status == AppTransparencyStatus.Authorized ||
                                       Status == AppTransparencyStatus.Restricted;

        private int _intervalId;

        /*
         * Public.
         */

        public void Check()
        {
            Log.Debug("Check");

            if (!CheckAuthorizationSupported())
            {
                Complete(AppTransparencyStatus.NotDetermined);
                return;
            }

            var statusRaw = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (statusRaw == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                _intervalId = AsyncResolver.IntervalCall(CheckAuthorizationStatus, 1F);
                return;
            }

            Complete(RawStatusToStatus(statusRaw));
        }

        /*
         * Private.
         */

        private bool CheckAuthorizationSupported()
        {
            bool supported;

            var systemVersion = Device.systemVersion;

            try
            {
                var versionNumbers = systemVersion.Split('.');
                var majorNumber = int.Parse(versionNumbers[0]);
                supported = majorNumber >= 14;

                Log.Debug(sv => $"SystemVersion: {sv}", systemVersion);
            }
            catch (Exception exception)
            {
                Log.Error(exception);

                supported = false;
            }

            Log.Debug((sv, s) => $"CheckAuthorizationSupported: SystemVersion: {sv}; Supported: {s}", systemVersion, supported);

            return supported;
        }

        private void CheckAuthorizationStatus()
        {
            Log.Debug("CheckAuthorizationStatus");

            var statusRaw = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (statusRaw == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                return;

            AsyncResolver.CancelCall(ref _intervalId);

            var status = RawStatusToStatus(statusRaw);

            Complete(status);
            
            Dispatcher.Dispatch(AppTransparencyEvent.Changed, status);
        }

        private AppTransparencyStatus RawStatusToStatus(ATTrackingStatusBinding.AuthorizationTrackingStatus statusRaw)
        {
            return statusRaw switch
            {
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED => AppTransparencyStatus.NotDetermined,
                ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED     => AppTransparencyStatus.Restricted,
                ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED         => AppTransparencyStatus.Denied,
                ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED     => AppTransparencyStatus.Authorized,
                _                                                                  => throw new ArgumentOutOfRangeException()
            };
        }

        private void Complete(AppTransparencyStatus status)
        {
            Log.Debug("Complete");

            Status = status;
            Initialized = true;
            
            Dispatcher.Dispatch(AppTransparencyEvent.Ready, status);
        }
    }
}

#endif