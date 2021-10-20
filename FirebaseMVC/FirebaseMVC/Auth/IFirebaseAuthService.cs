using System.Threading.Tasks;
using ACNHWorldMVC.Auth.Models;

namespace ACNHWorldMVC.Auth
{
    public interface IFirebaseAuthService
    {
        Task<FirebaseUser> Login(Credentials credentials);
        Task<FirebaseUser> Register(Registration registration);
    }
}