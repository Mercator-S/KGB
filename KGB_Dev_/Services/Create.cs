using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using KGB_Dev_.Data_Retrieving;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
            User = GetCurrentUser();
        }

        public async Task<bool> CreateKGB(KGB_KnowledgeViewModel Model, IList<IBrowserFile> ListOfFile)
        {
            KGB_Knowledge result = _mapper.Map<KGB_Knowledge>(Model);
            _mapper.Map(User.Result, result);
            if (result != null)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                if (ListOfFile.Count >= 1)
                {
                    result.Putanja_Fajl = await UploadFile(result.Id.ToString(), ListOfFile);
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                if (result.Visibility == true)
                {
                    await Task.Run(() => { _navigationManager.NavigateTo("PublicIndex"); });
                    return true;
                }
                await Task.Run(() => { _navigationManager.NavigateTo(""); });
                return true;
            }
            return false;
        }
        public async Task<bool> CreateCategory(KGB_CategoryViewModel Category)
        {
            KGB_Category result = _mapper.Map<KGB_Category>(Category);
            var Contains = _context.KGB_Category.Where(x => x.Naziv_Kategorije == Category.Naziv_Kategorije).FirstOrDefault();
            if (Contains != null)
            {
                return await Task.FromResult(true);
            }
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
            var Contains = _context.KGB_Subcategory.Where(x => x.Naziv_Potkategorije == SubCategory.Naziv_Potkategorije && x.Fk_Kategorija == SubCategory.Fk_Kategorija).FirstOrDefault();
            if (Contains != null)
            {
                return await Task.FromResult(true);
            }
            result.k_ins = User.Result.Id;
            result.k_upd = User.Result.Id;
            _context.Add(result);
            //Check if savechanges > 0 to return valid value
            await _context.SaveChangesAsync();
            return await Task.FromResult(false);
        }
        public async Task<bool> EditKGBKnowledge(KGB_Knowledge KGB_Knowledge, IList<IBrowserFile> ListOfFile)
        {
            if (KGB_Knowledge != null)
            {
                try
                {
                    if (ListOfFile.Count >= 1)
                    {
                        KGB_Knowledge.Putanja_Fajl = await UploadFile(KGB_Knowledge.Id.ToString(), ListOfFile);
                    }
                    KGB_Knowledge.d_upd = DateTime.Now;
                    _context.Update(KGB_Knowledge);
                    await _context.SaveChangesAsync();
                    if (KGB_Knowledge.Visibility == true)
                    {
                        await Task.Run(() => { _navigationManager.NavigateTo("PublicIndex", forceLoad: true); });
                        return await Task.FromResult(true);
                    }
                    await Task.Run(() => { _navigationManager.NavigateTo("", forceLoad: true); });
                    return await Task.FromResult(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExist(KGB_Knowledge.Id))
                    {
                        return await Task.FromResult(false);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return await Task.FromResult(false);
        }
        public async Task<bool> DeleteKGBKnowledge(KGB_Knowledge KGB_Knowledge)
        {
            if (KGB_Knowledge != null)
            {
                try
                {
                    KGB_Knowledge.d_upd = DateTime.Now;
                    KGB_Knowledge.k_upd = User.Result.Id;
                    KGB_Knowledge.Active = false;
                    _context.Update(KGB_Knowledge);
                    await _context.SaveChangesAsync();
                    if (KGB_Knowledge.Visibility == true)
                    {
                        await Task.Run(() => { _navigationManager.NavigateTo("PublicIndex", forceLoad: true); });
                        return await Task.FromResult(true);
                    }
                    await Task.Run(() => { _navigationManager.NavigateTo("", forceLoad: true); });
                    return await Task.FromResult(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExist(KGB_Knowledge.Id))
                    {
                        return await Task.FromResult(false);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return await Task.FromResult(false);
        }
        private bool ModelExist(long id)
        {
            return _context.KGB_Knowledge.Any(e => e.Id == id);
        }
    }
}
