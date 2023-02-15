using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.AppTransparency.Commands
{
    public sealed class AppTransparencyInitializeCommand : Command<AppTransparencySettings>
    {
        [Inject] public IEventDispatcher           Dispatcher                { get; set; }
        [Inject] public IAppTransparencyController AppTransparencyController { get; set; }
        
        public override void Execute(AppTransparencySettings settings)
        {
            if (AppTransparencyController.Initialized)
                return;
            
            Dispatcher.AddListenerOnce(AppTransparencyEvent.Initialized, Release);
            
            AppTransparencyController.Initialize(settings);
        }
    }
}