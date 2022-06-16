using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInWithAppleXamarin.WebApi;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SignInWithAppleXamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            _testQueries = new TestQueries(isLocalServer: false);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            AnonymousInfo.Text = await _testQueries.Hello();
        }
        private async void Signin_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var webAuthenticatorResult = await AppleSignInAuthenticator.AuthenticateAsync();
                AuthToken = webAuthenticatorResult?.AccessToken ?? webAuthenticatorResult?.IdToken;
                SigninInfo.Text = AuthToken;
            }
            catch (Exception exception)
            {
                SigninInfo.Text = exception.Message;
            }
        }

        private string AuthToken;


        private readonly TestQueries _testQueries;
    }
}
