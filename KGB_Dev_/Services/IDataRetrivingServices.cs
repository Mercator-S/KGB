using KGB_Dev_.Data.KGB_Model;

namespace KGB_Dev_.DataRetrieving
{
    public interface IDataRetrivingServices
    {
        Task<List<KGB_Knowledge>> GetListOfKnowledge(int OrgJed);
        Task<List<KGB_Knowledge>> GetPublicListOfKnowledge();
        Task<KGB_Knowledge> GetKnowledge(long id);
        Task<Task<KGB_User>> GetCurrentUser();
        Task<List<KGB_Category>> GetCategory();
        Task<List<string>> GetFile(string Path);
        Task NavigationManager(string nav);
        Task<List<KGB_Subcategory>> GetSubcategory(int category_id);
        Task<List<KGB_Subcategory>> GetSubcategory();
        Task<Dictionary<string, string>> GetUsersFromOj(int SifraOj);

    }
}
