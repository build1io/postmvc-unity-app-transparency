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
            #if UNITY_EDITOR
                InjectionBinder.Bind<IAppTransparencyController, Impl.AppTransparencyControllerEditor>();
            #elif UNITY_IOS
                InjectionBinder.Bind<IAppTransparencyController, Impl.AppTransparencyControllerIOS>();
            #elif UNITY_ANDROID
                InjectionBinder.Bind<IAppTransparencyController, Impl.AppTransparencyControllerAndroid>();
            #endif
        }
    }
}