using System;
using System.Threading.Tasks;
using SignInWithAppleSample.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SignInWithAppleSample.Services
{
    public class ClientSignInService
    {
        public static ClientSignInService Instance => Lazy.Value;

        private static readonly Lazy<ClientSignInService> Lazy =
            new Lazy<ClientSignInService>(() => new ClientSignInService(), true);

        public ClientSignInService()
        {
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            appleSignInService.CompleteWithAuthorization = CompleteWithAuthorization;
            appleSignInService.CompleteWithError = CompleteWithError;
        }
        public void AppleSignInRequest()
        {
            appleSignInService.SignInAsync();
        }
        private async Task CompleteWithError(string obj)
        {
            Application.Current.MainPage = new LoginPage();
        }

        private async Task CompleteWithAuthorization(AppleAccount account)
        {
            if (account != null)
            {
                App.AppleAccount = account;
                Preferences.Set(App.LoggedInKey, true);
                if (account.Email != null) await SecureStorage.SetAsync(nameof(AppleAccount.Email), account.Email);
                if (account.Name != null) await SecureStorage.SetAsync(nameof(AppleAccount.Name), account.Name);
                if (account.Token != null) await SecureStorage.SetAsync(nameof(AppleAccount.Token), account.Token);
                if (account.UserId != null) await SecureStorage.SetAsync(nameof(AppleAccount.UserId), account.UserId);
                if (account.RealUserStatus != null) await SecureStorage.SetAsync(nameof(AppleAccount.RealUserStatus), account.RealUserStatus);
                Application.Current.MainPage = new MainPage();
            }
        }

        readonly IAppleSignInService appleSignInService;
        
    }
}