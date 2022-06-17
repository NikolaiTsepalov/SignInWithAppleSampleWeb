using SignInWithAppleSample.Services;

namespace SignInWithAppleSample.WebApi
{
    public abstract class QueriesBase
    {
        protected QueriesBase(bool isLocalServer)
        {
            RequestProvider = Services.RequestProvider.Instance;
            _baseApiUrl = isLocalServer ?
                "http://10.211.55.3:45455" : // This port is provided by the visual studio extension Conveyor.
              // From the Mac to the local (Running in the Parallels virtual machine) windows
              // Could be 45456
              "https://signinwithappleweb.azurewebsites.net";
        }

        private readonly string _baseApiUrl;
        protected abstract string ControllerUrl { get; }

        protected string AddMethod(string method) => $"{_baseApiUrl}/api/{ControllerUrl}/{method}";
        protected readonly IRequestProvider RequestProvider;
    }
}
