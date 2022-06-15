using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KGB_Dev_.Data_Retrieving
{
    public class IndexTable : IDataRetrivingServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly SignInManager<KGB_User> _SignInManager;
        private readonly UserManager<KGB_User> _UserManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public IndexTable(ApplicationDbContext? context, SignInManager<KGB_User>? signInManager, UserManager<KGB_User> userManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _SignInManager = signInManager;
            _UserManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
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
        public async Task<Task<KGB_User>> GetCurrentUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return _UserManager.GetUserAsync(user);
        }
        public async Task<bool> CreateKGB(KGB_Knowledge Model)
        {
            var User = GetCurrentUser().Result;
            Model.Sifra_Oj = User.Result.Sifra_Oj;
            Model.k_ins = User.Result.Ime + " " + User.Result.Prezime;
            Model.k_upd = User.Result.Id;
            Model.d_ins = DateTime.Now;
            Model.Sifra_Prijave = "Nepoznato";
            Model.Putanja_Fajl = "Ime foldera gde su dokumenta za tu kategoriju";
            _context.Add(Model);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<KGB_Category>> GetCategory()
        {
            var result = _context.KGB_Category.ToList();
            return await Task.FromResult(result);
        }
    }
}
