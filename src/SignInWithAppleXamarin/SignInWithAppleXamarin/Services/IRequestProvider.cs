using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignInWithAppleXamarin.Services
{
    public interface IRequestProvider
    {

        Task<string> GetAsync(string uri, string token = "");
        void Dispose();
    }
}
