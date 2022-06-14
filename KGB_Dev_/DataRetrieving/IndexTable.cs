using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Identity;

namespace KGB_Dev_.Data_Retrieving
{
    public class IndexTable : IDataRetrivingServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly SignInManager<KGB_User> _SignInManager;
        private readonly UserManager<KGB_User> _UserManager;
        public IndexTable(ApplicationDbContext? context, SignInManager<KGB_User>? signInManager, UserManager<KGB_User> userManager)
        {
            _context = context;
            _SignInManager = signInManager;
            _UserManager = userManager;
        }
        public async Task<List<KGB_Knowledge>> GetListOfKnowledge()
        {
            var result = _context.KGB_Knowledge.ToList();
            return await Task.FromResult(result);
        }
        public async Task<KGB_Knowledge> GetKnowledge(long id)
        {
            var result = _context.KGB_Knowledge.Where(x => x.Id == id).FirstOrDefault();
            return await Task.FromResult(result);
        }
        public async Task<int> GetCurrentUserRole()
        {
            var result = _SignInManager.UserManager.Users.Select(x => x.Fk_Rola).FirstOrDefault();
            return result;
        }
    }
}
