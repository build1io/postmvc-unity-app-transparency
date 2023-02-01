using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.AppTransparency.Commands
{
    public sealed class AppTransparencyCheckCommand : Command
    {
        [Inject] public IAppTransparencyController AppTransparencyController { get; set; }
        [Inject] public IEventDispatcher           Dispatcher                { get; set; }

        public override void Execute()
        {
            Retain();

            Dispatcher.AddListenerOnce(AppTransparencyEvent.Ready, OnComplete);

            AppTransparencyController.Check();
        }

        private void OnComplete(AppTransparencyStatus status)
        {
            Release();
        }
    }
}