#if UNITY_EDITOR

using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.App;
using Build1.PostMVC.Unity.AppTransparency.Editor;
using UnityEditor;
using UnityEngine;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    public sealed class AppTransparencyControllerEditor : AppTransparencyControllerBase, IAppTransparencyController
    {
        private const string AuthorizationSetPlayerPrefsKey = "PostMVC_AppTransparencyControllerEditor_AuthorizationSet";
        
        [PostConstruct]
        public void PostConstruct()
        {
            Dispatcher.AddListener(AppEvent.Pause, OnAppPause);
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Dispatcher.RemoveListener(AppEvent.Pause, OnAppPause);
        }
        
        /*
         * Authorization.
         */

        protected override bool GetAuthorizationSupported()
        {
            return true;
        }
        
        protected override AppTransparencyStatus GetAuthorizationStatus()
        {
            return GetAuthorizationStatusStatic();
        }

        protected override void OnAuthorizationRequest()
        {
            Log.Debug("Editor simulation. Showing authorization request...");

            var status = EditorUtility.DisplayDialog("App Transparency", AppTransparencyBuildProcessor.GetAppTransparencyMessage(), "Ask app not to track", "Allow")
                             ? AppTransparencyStatus.Restricted
                             : AppTransparencyStatus.Authorized;

            SetAuthorizationStatusStatic(status);
            OnCompleteAuthorization(status);
        }

        /*
         * Event Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (Initialized && !Autorizing && !paused)
                TryUpdateAuthorizationStatus(GetAuthorizationStatus());
        }

        /*
         * Static.
         */

        public static AppTransparencyStatus GetAuthorizationStatusStatic()
        {
            if (PlayerPrefs.HasKey(AuthorizationSetPlayerPrefsKey))
                return (AppTransparencyStatus)PlayerPrefs.GetInt(AuthorizationSetPlayerPrefsKey);
            return AppTransparencyStatus.NotDetermined;
        }

        public static void SetAuthorizationStatusStatic(AppTransparencyStatus status)
        {
            PlayerPrefs.SetInt(AuthorizationSetPlayerPrefsKey, (int)status);
        }
        
        public static void ResetAuthorizationStatusStatic()
        {
            PlayerPrefs.DeleteKey(AuthorizationSetPlayerPrefsKey);
        }
    }
}

#endif