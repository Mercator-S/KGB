using KGB_Models.KGB_Model;

namespace KGB_Application.Interfaces
{
    public interface IUserRepository
    {
        Task<KGB_User> GetCurrentUser();
    }
}
