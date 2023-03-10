namespace Build1.PostMVC.Unity.AppTransparency
{
    public interface IAppTransparencyController
    {
        AppTransparencyStatus Status       { get; }
        bool                  Initializing { get; }
        bool                  Initialized  { get; }
        bool                  Autorizing   { get; }

        void Initialize(AppTransparencySettings settings);
        void RequestAuthorization();
    }
}