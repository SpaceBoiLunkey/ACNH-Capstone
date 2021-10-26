using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(User userProfile);
        User GetByFirebaseUserId(string firebaseUserId);
        User GetById(int id);
    }
}