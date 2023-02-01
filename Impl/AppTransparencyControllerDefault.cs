using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    internal sealed class AppTransparencyControllerDefault : IAppTransparencyController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        public AppTransparencyStatus Status          => AppTransparencyStatus.Authorized;
        public bool                  Initialized     { get; private set; }
        public bool                  TrackingAllowed => true;

        public void Check()
        {
            Initialized = true;
            Dispatcher.Dispatch(AppTransparencyEvent.Ready, AppTransparencyStatus.Authorized);
        }
    }
}