using System;
using System.Threading.Tasks;

namespace SignInWithAppleXamarin.WebApi
{
    public class TestQueries : QueriesBase
    {
        public TestQueries(bool isLocalServer): base(isLocalServer)
        {
        }
        public Task<string> Hello()
        {
            return  RequestProvider.GetAsync(AddToUrl(nameof(Hello)));
        }

        protected override string ControllerUrl => "Test";
    }
}
