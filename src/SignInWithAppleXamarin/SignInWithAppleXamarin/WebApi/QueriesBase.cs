using System;
using System.Linq;
using System.Reflection;
using System.Text;
using SignInWithAppleXamarin.Services;
using Xamarin.Essentials;
using DeviceInfo = Xamarin.Essentials.DeviceInfo;

namespace SignInWithAppleXamarin.WebApi
{
    public abstract class QueriesBase
    {
        protected QueriesBase(bool isLocalServer)
        {
            RequestProvider = Services.RequestProvider.Instance;
            _baseApiUrl = isLocalServer ?
                "http://10.211.55.3:45456" : // This port is provided by the visual studio extension Conveyor.
              // From the Mac to the local (Running in the Parallels virtual machine) windows
              // Could be 45455
              "https://signinwithappleweb.azurewebsites.net";
        }

        private readonly string _baseApiUrl;
        protected abstract string ControllerUrl { get; }

        protected string AddToUrl(string method) => $"{_baseApiUrl}/api/{ControllerUrl}/{method}";
        protected readonly IRequestProvider RequestProvider;
    }
}
