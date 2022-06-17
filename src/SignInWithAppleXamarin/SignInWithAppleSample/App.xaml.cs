using System;
using System.Diagnostics;
using SignInWithAppleSample.Models;
using SignInWithAppleSample.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SignInWithAppleSample
{
    public partial class App : Application
    {
        public const string LoggedInKey = "LoggedIn";
        string userId;
        public App()
        {
            InitializeComponent();
            Debug.WriteLine("App 1");
            MainPage = new StartPage();
            Debug.WriteLine("App 2");
        }

        protected override async void OnStart()
        {
            Debug.WriteLine("OnStart 1");
            var appleSignInService = DependencyService.Get<IAppleSignInService>();
            Debug.WriteLine("OnStart 2");
            if(appleSignInService != null)
            {
                Debug.WriteLine($"OnStart 3 appleSignInService.IsAvailable = {appleSignInService.IsAvailable}");
                userId = await SecureStorage.GetAsync(nameof(AppleAccount.UserId));
                Debug.WriteLine($"OnStart 4 userId IsNullOrWhiteSpace {string.IsNullOrWhiteSpace(userId)} {userId}" );
                if (appleSignInService.IsAvailable && false == string.IsNullOrEmpty(userId))
                {
                    Debug.WriteLine("OnStart 5");
                    var credentialState = await appleSignInService.GetCredentialStateAsync(userId);
                    Debug.WriteLine("OnStart 6");
                    switch (credentialState)
                    {
                        case AppleSignInCredentialState.Authorized:
                            Debug.WriteLine("OnStart 7");
                            AppleAccount = new AppleAccount();
                            AppleAccount.UserId = await SecureStorage.GetAsync(nameof(AppleAccount.UserId));
                            AppleAccount.Email = await SecureStorage.GetAsync(nameof(AppleAccount.Email)); 
                            AppleAccount.Name = await SecureStorage.GetAsync(nameof(AppleAccount.Name));
                            AppleAccount.Token = await SecureStorage.GetAsync(nameof(AppleAccount.Token));
                            AppleAccount.RealUserStatus = await SecureStorage.GetAsync(nameof(AppleAccount.RealUserStatus));
                            if (string.IsNullOrWhiteSpace(AppleAccount.Token))
                            {
                                Logout();
                                MainPage = new LoginPage();
                            }
                            else
                            {
                                MainPage = new MainPage();    
                            }
                            
                            Debug.WriteLine("OnStart 8");
                            break;
                        case AppleSignInCredentialState.NotFound:
                        case AppleSignInCredentialState.Revoked:
                            Debug.WriteLine("OnStart 9");
                            Logout();
                            Debug.WriteLine("OnStart 12");
                            MainPage = new LoginPage();
                            break;
                    }
                }
                else
                {
                    Debug.WriteLine("OnStart 13");
                    Logout();
                    MainPage = new LoginPage();
                    Debug.WriteLine("OnStart 14");
                }
            }
            else
            {
                Debug.WriteLine("OnStart 15");
                await Current.MainPage.DisplayAlert("Apple Sign In Service","Apple Sign In Service error!","Ok");
                Debug.WriteLine("OnStart 16");
            }
        }

        public static void Logout()
        {
            SecureStorage.Remove(nameof(AppleAccount.UserId));
            SecureStorage.Remove(nameof(AppleAccount.Token));
            Debug.WriteLine("OnStart 10");
            Preferences.Set(LoggedInKey, false);
            Debug.WriteLine("OnStart 11");
            Debug.WriteLine("OnStart 12");
        }

        public static AppleAccount AppleAccount { get; set; }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
