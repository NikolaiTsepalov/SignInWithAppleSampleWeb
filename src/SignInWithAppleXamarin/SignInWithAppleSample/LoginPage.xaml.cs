using System;
using SignInWithAppleSample.Services;
using SignInWithAppleSample.WebApi;
using Xamarin.Forms;

namespace SignInWithAppleSample
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            _testQueries = new TestQueries(isLocalServer: false);
        }
        
        private void AppleSignInButton_Clicked(object sender, EventArgs e)
        {
            ClientSignInService.Instance.AppleSignInRequest();
        }
        private async void Anonymous_OnClicked(object sender, EventArgs e)
        {
            AnonymousInfo.Text = await _testQueries.Hello();
        }
        private readonly TestQueries _testQueries;
    }
}
