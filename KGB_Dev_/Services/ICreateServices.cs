using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;
using Microsoft.AspNetCore.Components.Forms;

namespace KGB_Dev_.Services
{
    public interface ICreateServices
    {
        Task<bool> CreateSubCategory(KGB_SubcategoryViewModel SubCategory);
        Task<bool> CreateCategory(KGB_CategoryViewModel Category);
        Task<bool> CreateKGB(KGB_KnowledgeViewModel Model, IList<IBrowserFile> ListOfFile);
        Task<bool> EditKGBKnowledge(KGB_Knowledge KGB_Knowledge, IList<IBrowserFile> ListOfFile);
    }
}
