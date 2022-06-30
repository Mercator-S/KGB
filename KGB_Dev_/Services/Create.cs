using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.IO;
using KGB_Dev_.Data_Retrieving;

namespace KGB_Dev_.Services
{
    public class Create : DataRetriving,ICreateServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly UserManager<KGB_User> _UserManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;

        public Create(ApplicationDbContext? context, SignInManager<KGB_User> signInManager, UserManager<KGB_User> userManager, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager) : base(context, signInManager, userManager, authenticationStateProvider, navigationManager)
        {
            _context = context;
            _UserManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<bool> CreateCategory(KGB_CategoryViewModel Category)
        {
            var User =GetCurrentUser().Result;
            KGB_Category Cat = new KGB_Category();
            Cat.Naziv_Kategorije = Category.Naziv_Kategorije;
            Cat.Sifra_Oj = User.Result.Sifra_Oj;
            Cat.k_ins = User.Result.Id;
            Cat.k_upd = User.Result.Id;
            Cat.d_ins = DateTime.Now;
            _context.Add(Cat);
            // Check if savechanges > 0 to return valid value
            await _context.SaveChangesAsync();
            return await Task.FromResult(false);
        }
        public async Task<bool> CreateKGB(KGB_Knowledge Model, IList<IBrowserFile> ListOfFile)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var User = _UserManager.GetUserAsync(user);
            Model.Naziv_Oj = User.Result.Naziv_Oj;
            Model.Sifra_Oj = User.Result.Sifra_Oj;
            Model.k_ins = User.Result.Ime + " " + User.Result.Prezime;
            Model.k_upd = User.Result.Id;
            Model.d_ins = DateTime.Now;
            Model.Sifra_Prijave = Model.Naziv_Prijave.Substring(0, 2) + User.Result.Ime.Substring(0, 2);
            Model.Putanja_Fajl = await UploadFile(Model.Naziv_Prijave, ListOfFile);
            if (Model != null)
            {
                _context.Add(Model);
                await _context.SaveChangesAsync();
                await Task.Run(() => { _navigationManager.NavigateTo("/"); });
                return true;
            }
            return false;
        }
        public async Task<bool> CreateSubCategory(KGB_SubcategoryViewModel SubCategory)
        {
            KGB_Subcategory SubCat = new KGB_Subcategory();
            SubCat.Naziv_Potkategorije = SubCategory.Naziv_Potkategorije;
            SubCat.Fk_Kategorija = SubCategory.Fk_Kategorija;
            SubCat.d_ins = DateTime.Now;
            SubCat.k_ins = "1";
            SubCat.k_upd = "1";
            _context.Add(SubCat);
            //Check if savechanges > 0 to return valid value
            await _context.SaveChangesAsync();
            return await Task.FromResult(false);
        }
    }
}
