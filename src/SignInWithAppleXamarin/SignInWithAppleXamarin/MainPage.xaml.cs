using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInWithAppleXamarin.WebApi;
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
            info.Text = await _testQueries.Hello();
        }

        private readonly TestQueries _testQueries;
    }
}
