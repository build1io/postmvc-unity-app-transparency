namespace Build1.PostMVC.Unity.AppTransparency
{
    public interface IAppTransparencyController
    {
        AppTransparencyStatus Status          { get; }
        bool                  Initialized     { get; }
        bool                  TrackingAllowed { get; }

        void Check();
    }
}