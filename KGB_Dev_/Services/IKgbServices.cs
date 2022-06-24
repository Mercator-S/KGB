using KGB_Dev_.Data.KGB_Model;
using Microsoft.AspNetCore.Components.Forms;

namespace KGB_Dev_.DataRetrieving
{
    public interface IKgbServices
    {
        Task<List<KGB_Knowledge>> GetListOfKnowledge(int OrgJed);
        Task<List<KGB_Knowledge>> GetPublicListOfKnowledge();
        Task<KGB_Knowledge> GetKnowledge(long id);
        Task<Task<KGB_User>> GetCurrentUser();
        Task<bool> CreateKGB(KGB_Knowledge Model, IList<IBrowserFile> ListOfFile);
        Task<List<KGB_Category>> GetCategory();
        Task<List<string>> GetFile(string Path);
        Stream GetFileStream();
        Task NavigationManager(string nav);
        Task<List<KGB_Category>> GetCategory(int OrgJed);
        Task<List<KGB_Subcategory>> GetSubcategory(int category_id);
    }
}
