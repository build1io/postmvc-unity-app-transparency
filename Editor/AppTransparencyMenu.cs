#if UNITY_EDITOR

using Build1.PostMVC.Unity.AppTransparency.Impl;
using UnityEditor;

namespace Build1.PostMVC.Unity.AppTransparency.Editor
{
    public static class AppTransparencyMenu
    {
        private const string AllowMenuItem    = "Tools/Build1/PostMVC/App Transparency/Allow";
        private const string RestrictMenuItem = "Tools/Build1/PostMVC/App Transparency/Restrict";
        private const string DenyMenuItem     = "Tools/Build1/PostMVC/App Transparency/Deny";
        private const string ResetMenuItem    = "Tools/Build1/PostMVC/App Transparency/Reset";

        [MenuItem(AllowMenuItem, false, 0)]
        public static void Allow()
        {
            AppTransparencyControllerEditor.SetAuthorizationStatusStatic(AppTransparencyStatus.Authorized);
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(AllowMenuItem, true, 0)]
        public static bool AllowValidation()
        {
            return AppTransparencyControllerEditor.GetAuthorizationStatusStatic() != AppTransparencyStatus.Authorized;
        }

        [MenuItem(RestrictMenuItem, false, 1)]
        public static void Restrict()
        {
            AppTransparencyControllerEditor.SetAuthorizationStatusStatic(AppTransparencyStatus.Restricted);
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(RestrictMenuItem, true, 1)]
        public static bool RestrictValidation()
        {
            return AppTransparencyControllerEditor.GetAuthorizationStatusStatic() != AppTransparencyStatus.Restricted;
        }
        
        [MenuItem(DenyMenuItem, false, 1)]
        public static void Deny()
        {
            AppTransparencyControllerEditor.SetAuthorizationStatusStatic(AppTransparencyStatus.Denied);
            EditorUtility.DisplayDialog("App Transparency", $"Editor authorization status set: {AppTransparencyControllerEditor.GetAuthorizationStatusStatic()}", "Ok");
        }

        [MenuItem(DenyMenuItem, true, 1)]
        public static bool DenyValidation()
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