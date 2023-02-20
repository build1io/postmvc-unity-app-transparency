#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Build1.PostMVC.Unity.AppTransparency.Editor
{
    public static class AppTransparencyBuildProcessor
    {
        [PostProcessBuild]
        private static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget == BuildTarget.iOS)
                AddPListValues(buildPath);
        }
        
        private static void AddPListValues(string buildPath)
        {
            var path = buildPath + "/Info.plist";
            var obj = new PlistDocument();
            obj.ReadFromString(File.ReadAllText(path));
            obj.root.SetString("NSUserTrackingUsageDescription", GetAppTransparencyMessage());

            File.WriteAllText(path, obj.WriteToString());
        }

        internal static string GetAppTransparencyMessage()
        {
            return "Your data will be used to provide you and other users a better and personalized experience.";
        }
    }
}

#endif