#if UNITY_IOS || UNITY_EDITOR

using System;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.App;
using Unity.Advertisement.IosSupport;
using Build1.PostMVC.Unity.App.Modules.Async;
using UnityEngine.iOS;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    internal sealed class AppTransparencyControllerIOS : AppTransparencyControllerBase, IAppTransparencyController
    {
        [Inject] public IAsyncResolver AsyncResolver { get; set; }

        private int _intervalId;

        [PostConstruct]
        public void PostConstruct()
        {
            Dispatcher.AddListener(AppEvent.Pause, OnAppPause);
            
            _intervalId = AsyncResolver.DefaultCallId;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Dispatcher.RemoveListener(AppEvent.Pause, OnAppPause);

            AsyncResolver.CancelCall(ref _intervalId);
        }

        /*
         * Authorization.
         */

        protected override bool GetAuthorizationSupported()
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
        
        protected override AppTransparencyStatus GetAuthorizationStatus()
        {
            return RawStatusToStatus(ATTrackingStatusBinding.GetAuthorizationTrackingStatus());
        }

        protected override void OnAuthorizationRequest()
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            _intervalId = AsyncResolver.IntervalCall(CheckAuthorizationRequestStatus, 1F);
        }
        
        private void CheckAuthorizationRequestStatus()
        {
            Log.Debug("Authorization iteration");

            var statusRaw = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (statusRaw == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                return;

            AsyncResolver.CancelCall(ref _intervalId);

            OnCompleteAuthorization(RawStatusToStatus(statusRaw));
        }

        /*
         * Private.
         */

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
        
        /*
         * Event Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (Initialized && !Autorizing && !paused)
                TryUpdateAuthorizationStatus(GetAuthorizationStatus());
        }
    }
}

#endif