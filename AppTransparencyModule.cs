using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Unity.AppTransparency
{
    public sealed class AppTransparencyModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        public void PostConstruct()
        {
            #if UNITY_IOS && !UNITY_EDITOR
                InjectionBinder.Bind<IAppTransparencyController, Impl.AppTransparencyControllerIOS>();
            #else
                InjectionBinder.Bind<IAppTransparencyController, Impl.AppTransparencyControllerDefault>();
            #endif
        }
    }
}