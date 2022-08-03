using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using KGB_Models;

namespace KGB_Dev_.Services
{
    public class Create : ICreateServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly NavigationManager _navigationManager;
        private readonly IMapper _mapper;
        private IDataRetrivingServices _dataRetrivingServices;
        public Task<KGB_User> User { get; set; }

        public Create(ApplicationDbContext? context, NavigationManager navigationManager, IMapper mapper, IDataRetrivingServices dataRetrivingServices)
        {
            _context = context;
            _navigationManager = navigationManager;
            _mapper = mapper;
            _dataRetrivingServices = dataRetrivingServices;
            User = _dataRetrivingServices.GetCurrentUser();
        }

        public async Task<bool> CreateKGB(KGB_KnowledgeViewModel Model, IList<IBrowserFile> ListOfFile, Dictionary<int, string?> OrgJed)
        {
            KGB_Knowledge result = _mapper.Map<KGB_Knowledge>(Model);
            _mapper.Map(User.Result, result);
            if (result != null)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                await ValidationForOrgJed(OrgJed, result);
                if (ListOfFile.Count >= 1)
                {
                    result.Putanja_Fajl = await _dataRetrivingServices.UploadFile(result.Id.ToString(), ListOfFile);
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
            _mapper.Map(User.Result, result);
            KGB_Category? Contains = _context.KGB_Category.Where(x => x.Naziv_Kategorije == result.Naziv_Kategorije && x.Sifra_Oj == result.Sifra_Oj).FirstOrDefault();
            if (Contains != null)
            {
                return await Task.FromResult(false);
            }
            _context.Add(result);
            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        public async Task<bool> CreateSubCategory(KGB_SubcategoryViewModel SubCategory)
        {
            KGB_Subcategory result = _mapper.Map<KGB_Subcategory>(SubCategory);
            _mapper.Map(User.Result, result);
            KGB_Subcategory? Contains = _context.KGB_Subcategory.Where(x => x.Naziv_Potkategorije == result.Naziv_Potkategorije && x.Fk_Kategorija == result.Fk_Kategorija).FirstOrDefault();
            if (Contains != null)
            {
                return await Task.FromResult(false);
            }
            _context.Add(result);
            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        public async Task<bool> EditKGBKnowledge(KGB_Knowledge KGB_Knowledge, IList<IBrowserFile> ListOfFile)
        {
            if (KGB_Knowledge != null)
            {
                try
                {
                    if (ListOfFile.Count >= 1)
                    {
                        KGB_Knowledge.Putanja_Fajl = await _dataRetrivingServices.UploadFile(KGB_Knowledge.Id.ToString(), ListOfFile);
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
                    if (!await ModelExist(KGB_Knowledge.Id))
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
                    if (!await ModelExist(KGB_Knowledge.Id))
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
        private async Task<bool> ModelExist(long id)
        {
            return _context.KGB_Knowledge.Any(e => e.Id == id);
        }
        public async Task ValidationForOrgJed(Dictionary<int, string?> OrgJed, KGB_Knowledge result)
        {
            if (OrgJed.Count < 1)
            {
                OrgJed.Add(result.Sifra_Oj, result.Naziv_Oj);
            }
            if (OrgJed.Count >= 1 && result.Visibility == false)
            {
                foreach (var item in OrgJed)
                {
                    KGB_OJKnowledge oJKnowledge = new KGB_OJKnowledge();
                    oJKnowledge.IdPrijave = result.Id;
                    oJKnowledge.Sifra_Oj = item.Key;
                    _context.KGB_OJKnowledge.Add(oJKnowledge);
                    _context.SaveChanges();
                }
            }
        }
    }
}
