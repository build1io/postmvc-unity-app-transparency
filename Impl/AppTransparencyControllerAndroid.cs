#if UNITY_ANDROID || UNITY_EDITOR

using System;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    internal sealed class AppTransparencyControllerAndroid : AppTransparencyControllerBase, IAppTransparencyController
    {
        protected override bool GetAuthorizationSupported()
        {
            return false;
        }

        protected override AppTransparencyStatus GetAuthorizationStatus() { throw new NotImplementedException(); }
        protected override void                  OnAuthorizationRequest() { throw new NotImplementedException(); }
    }
}

#endif