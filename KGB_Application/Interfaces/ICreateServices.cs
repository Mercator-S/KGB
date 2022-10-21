using KGB_Domain.KGB_SpecificDataType;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components.Forms;
namespace KGB_Dev_.Interfaces
{
    public interface ICreateServices
    {
        Task<bool> CreateSubCategory(KGB_SubcategoryViewModel SubCategory);
        Task<bool> CreateCategory(KGB_CategoryViewModel Category);
        Task<bool> CreateKGB(KGB_KnowledgeViewModel Model, IList<IBrowserFile> ListOfFile, List<KGB_OrgJed> OrgJed);
        Task<bool> EditKGBKnowledge(KGB_Knowledge KGB_Knowledge, IList<IBrowserFile> ListOfFile);
        Task<bool> DeleteKGBKnowledge(KGB_Knowledge KGB_Knowledge);

    }
}
