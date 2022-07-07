using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using KGB_Dev_.Data_Retrieving;
using AutoMapper;

namespace KGB_Dev_.Services
{
    public class Create : DataRetriving, ICreateServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly NavigationManager _navigationManager;
        private readonly IMapper _mapper;
        public Task<KGB_User> User { get; set; }

        public Create(ApplicationDbContext? context, SignInManager<KGB_User> signInManager, UserManager<KGB_User> userManager, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager, IMapper mapper) : base(context, signInManager, userManager, authenticationStateProvider, navigationManager)
        {
            _context = context;
            _navigationManager = navigationManager;
            _mapper = mapper;
            User = GetCurrentUser().Result;
        }

        public async Task<bool> CreateKGB(KGB_KnowledgeViewModel Model, IList<IBrowserFile> ListOfFile)
        {
            KGB_Knowledge result = _mapper.Map<KGB_Knowledge>(Model);
            result.Naziv_Oj = User.Result.Naziv_Oj;
            result.Sifra_Oj = User.Result.Sifra_Oj;
            result.k_ins = User.Result.Ime + " " + User.Result.Prezime;
            result.k_upd = User.Result.Id;
            result.Sifra_Prijave = Model.Naziv_Prijave.Substring(0, 2) + User.Result.Ime.Substring(0, 2);
            result.Putanja_Fajl = await UploadFile(result.Naziv_Prijave, ListOfFile);
            if (result != null)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                await Task.Run(() => { _navigationManager.NavigateTo(""); });
                return true;
            }
            return false;
        }
        public async Task<bool> CreateCategory(KGB_CategoryViewModel Category)
        {
            KGB_Category result = _mapper.Map<KGB_Category>(Category);
            result.Sifra_Oj = User.Result.Sifra_Oj;
            result.k_ins = User.Result.Id;
            result.k_upd = User.Result.Id;
            _context.Add(result);
            // Check if savechanges > 0 to return valid value
            await _context.SaveChangesAsync();
            return await Task.FromResult(false);
        }
        public async Task<bool> CreateSubCategory(KGB_SubcategoryViewModel SubCategory)
        {
            KGB_Subcategory result = _mapper.Map<KGB_Subcategory>(SubCategory);
            result.k_ins = User.Result.Id;
            result.k_upd = User.Result.Id;
            _context.Add(result);
            //Check if savechanges > 0 to return valid value
            await _context.SaveChangesAsync();
            return await Task.FromResult(false);
        }
    }
}
