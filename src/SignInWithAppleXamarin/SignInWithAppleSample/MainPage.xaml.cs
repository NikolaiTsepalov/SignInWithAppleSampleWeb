using System;
using System.ComponentModel;
using SignInWithAppleSample.WebApi;
using Xamarin.Forms;

namespace SignInWithAppleSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            _testQueries = new TestQueries(isLocalServer: false);
        }
        private async void MainPage_OnAppearing(object sender, EventArgs e)
        {
            _vm = new MainPageViewModel
            {
                UserId = App.AppleAccount?.UserId,
                Token = App.AppleAccount?.Token,
                Name = App.AppleAccount?.Name,
                Email = App.AppleAccount?.Email,
                RealUserStatus = App.AppleAccount?.RealUserStatus
            };
            BindingContext = _vm;
        }

        private void SignOutSignOutButton_OnClicked(object sender, EventArgs e)
        {
           App.Logout();
           Application.Current.MainPage = new LoginPage();
        }
        private async void Authorized_OnClicked(object sender, EventArgs e)
        {
            AuthorizedInfo.Text = await _testQueries.Hi(_vm.Token);
        }
        private readonly TestQueries _testQueries;
        MainPageViewModel _vm;
    }
}
