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
            return  RequestProvider.GetAsync(AddMethod(nameof(Hello)));
        }
        public Task<string> Hi(string token)
        {
            return  RequestProvider.GetAsync(AddMethod(nameof(Hi)),token);
        }

        protected override string ControllerUrl => "Test";
    }
}
