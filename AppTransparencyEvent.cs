using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.AppTransparency
{
    public static class AppTransparencyEvent
    {
        public static readonly Event                        Initialized                = new(typeof(AppTransparencyEvent), nameof(Initialized));
        public static readonly Event<AppTransparencyStatus> AuthorizationStatusChanged = new(typeof(AppTransparencyEvent), nameof(AuthorizationStatusChanged));
    }
}