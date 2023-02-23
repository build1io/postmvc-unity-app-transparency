using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Modules.Logging;

namespace Build1.PostMVC.Unity.AppTransparency.Impl
{
    public abstract class AppTransparencyControllerBase
    {
        [Log(LogLevel.Warning)] public ILog             Log        { get; set; }
        [Inject]                public IEventDispatcher Dispatcher { get; set; }

        public AppTransparencyStatus Status       { get; private set; } = AppTransparencyStatus.Unknown;
        public bool                  Initializing { get; private set; }
        public bool                  Initialized  { get; private set; }
        public bool                  Autorizing   { get; private set; }

        private AppTransparencySettings _settings;

        /*
         * Initialization.
         */

        public void Initialize(AppTransparencySettings settings)
        {
            if (Initialized)
            {
                Log.Warn("Already initialized");
                return;
            }

            if (Initializing)
            {
                Log.Warn("Initialization is in progress");
                return;
            }

            _settings = settings;

            Initializing = true;

            if (!GetAuthorizationSupported())
            {
                Status = AppTransparencyStatus.Authorized;
                InitializeComplete();
                return;
            }

            var status = GetAuthorizationStatus();
            if (status != AppTransparencyStatus.NotDetermined)
            {
                Status = status;
                InitializeComplete();
                return;
            }

            Log.Debug("Initializing...");

            if ((_settings & AppTransparencySettings.DelayAuthorization) == AppTransparencySettings.DelayAuthorization)
            {
                Status = AppTransparencyStatus.NotDetermined;
                InitializeComplete();
                return;
            }

            RequestAuthorization();
        }

        protected void InitializeComplete()
        {
            if (!Initializing)
                return;

            Log.Debug(s => $"Initialized. Status: {s}", Status);

            Initializing = false;
            Initialized = true;
            Dispatcher.Dispatch(AppTransparencyEvent.Initialized);
        }

        /*
         * Authorization.
         */

        protected abstract bool                  GetAuthorizationSupported();
        protected abstract AppTransparencyStatus GetAuthorizationStatus();

        public void RequestAuthorization()
        {
            if (Status != AppTransparencyStatus.NotDetermined)
            {
                Log.Error("Authorization request rejected. User already allowed or denied data tracking.");
                return;
            }

            if (Autorizing)
            {
                Log.Error("Authorization already in progress");
                return;
            }

            Log.Debug("Requesting authorization...");

            Autorizing = true;
            OnAuthorizationRequest();
        }

        protected abstract void OnAuthorizationRequest();

        protected void OnCompleteAuthorization(AppTransparencyStatus status)
        {
            if (!Autorizing)
                return;

            Log.Debug(s => $"Authorization complete. Status: {s}", status);

            TryUpdateAuthorizationStatus(status);

            Autorizing = false;

            if (Initializing)
                InitializeComplete();
        }

        protected void TryUpdateAuthorizationStatus(AppTransparencyStatus status)
        {
            if (Status == status)
                return;

            Log.Debug(s => $"Authorization status updated: {s}", status);

            Status = status;
            Dispatcher.Dispatch(AppTransparencyEvent.AuthorizationStatusChanged, status);
        }
    }
}