using System.Threading.Tasks;

namespace SignInWithAppleSample.Services
{
    public interface IRequestProvider
    {

        Task<string> GetAsync(string uri, string token = "");
        void Dispose();
    }
}
