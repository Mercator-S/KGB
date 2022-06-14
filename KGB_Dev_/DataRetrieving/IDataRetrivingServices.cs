using KGB_Dev_.Data.KGB_Model;

namespace KGB_Dev_.DataRetrieving
{
    public interface IDataRetrivingServices
    {
        Task<List<KGB_Knowledge>> GetListOfKnowledge();
        Task<KGB_Knowledge> GetKnowledge(long id);
        Task<int> GetCurrentUserRole();

    }
}
