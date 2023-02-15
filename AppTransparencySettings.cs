using System;

namespace Build1.PostMVC.Unity.AppTransparency
{
    [Flags]
    public enum AppTransparencySettings
    {
        RequestAuthorization = 0,
        DelayAuthorization   = 1 << 0
    }
}