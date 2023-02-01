#if UNITY_EDITOR && UNITY_IOS

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
            var description = $"Allow \"{Application.productName}\" to track your activity across other companies' apps and websites?";
            
            var path = buildPath + "/Info.plist";
            var obj = new PlistDocument();
            obj.ReadFromString(File.ReadAllText(path));
            obj.root.SetString("NSUserTrackingUsageDescription", description);

            File.WriteAllText(path, obj.WriteToString());
        }
    }
}

#endif