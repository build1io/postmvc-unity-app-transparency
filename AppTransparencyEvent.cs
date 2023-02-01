using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Unity.AppTransparency
{
    public static class AppTransparencyEvent
    {
        public static readonly Event<AppTransparencyStatus> Ready   = new(typeof(AppTransparencyEvent), nameof(Ready));
        public static readonly Event<AppTransparencyStatus> Changed = new(typeof(AppTransparencyEvent), nameof(Changed));
    }
}