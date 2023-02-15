#if UNITY_EDITOR

using Build1.PostMVC.Unity.AppTransparency.Impl;
using UnityEditor;

namespace Build1.PostMVC.Unity.AppTransparency.Editor
{
    public static class AppTransparencyMenu
    {
        private const string EnabledMenuItem  = "Tools/Build1/PostMVC/App Transparency/Enable";
        private const string DisabledMenuItem = "Tools/Build1/PostMVC/App Transparency/Disable";
        private const string ResetMenuItem    = "Tools/Build1/PostMVC/App Transparency/Reset";

        [MenuItem(EnabledMenuItem, false, 0)]
        public static void Enable()
        {
            
            AppTransparencyControllerEditor.SetAuthorizationStatusStatic(AppTransparencyStatus.Authorized);
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(EnabledMenuItem, true, 0)]
        public static bool EnableValidation()
        {
            return AppTransparencyControllerEditor.GetAuthorizationStatusStatic() != AppTransparencyStatus.Authorized;
        }

        [MenuItem(DisabledMenuItem, false, 1)]
        public static void Disable()
        {
            AppTransparencyControllerEditor.SetAuthorizationStatusStatic(AppTransparencyStatus.Denied);
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(DisabledMenuItem, true, 1)]
        public static bool DisableValidation()
        {
            return AppTransparencyControllerEditor.GetAuthorizationStatusStatic() != AppTransparencyStatus.Denied;
        }

        [MenuItem(ResetMenuItem, false, 20)]
        public static void Reset()
        {
            AppTransparencyControllerEditor.ResetAuthorizationStatusStatic();
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(ResetMenuItem, true, 20)]
        public static bool ResetValidation()
        {
            return AppTransparencyControllerEditor.GetAuthorizationStatusStatic() != AppTransparencyStatus.NotDetermined;
        }
    }
}

#endif