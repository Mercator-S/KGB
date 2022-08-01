using KGB_Dev_.Interfaces;
using KGB_Models;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace KGB_Dev_.Data_Retrieving
{
    public class DataRetriving : IDataRetrivingServices
    {
        private readonly ApplicationDbContext? _context;
        private readonly UserManager<KGB_User> _UserManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        private KGB_User User { get; set; }

        public DataRetriving(ApplicationDbContext? context, UserManager<KGB_User> userManager, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _context = context;
            _UserManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
            User = GetCurrentUser().Result;
        }
        public async Task<List<KGB_Knowledge>> GetPublicListOfKnowledge()
        {
            return await Task.FromResult(_context.KGB_Knowledge.Where(x => x.Visibility == true && x.Active == true).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<List<KGB_Knowledge?>> GetListOfKnowledge(int OrgJed)
        {
            List<KGB_OJKnowledge> KGBoJKnowledge = _context.KGB_OJKnowledge.Where(x => x.Sifra_Oj == OrgJed).OrderByDescending(x => x.Id).ToList();
            if (KGBoJKnowledge.Count >= 1)
            {
                long MaxId = KGBoJKnowledge.Select(x => x.IdPrijave).Max();
                long MinId = KGBoJKnowledge.Select(x => x.IdPrijave).Min();
                List<KGB_Knowledge?> result = _context.KGB_Knowledge.Where(x => x.Id <= MaxId && x.Id >= MinId && x.Visibility == false && x.Active == true).ToList();
                return result;
            }
            return new List<KGB_Knowledge>();
            //return await Task.FromResult(_context.KGB_Knowledge.Where(x => KGBoJKnowledge && x.Visibility == false && x.Active == true).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<KGB_Knowledge> GetKnowledge(long id)
        {
            return await Task.FromResult(_context.KGB_Knowledge.FirstOrDefault(x => x.Id == id));
        }
        public async Task<KGB_User> GetCurrentUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return await Task.FromResult(_UserManager.GetUserAsync(authState.User).Result);
        }

        public void CheckFolder(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
        public async Task<List<string>> GetFile(string path)
        {
            bool exist = Directory.Exists(path);
            List<string> FileNames = new List<string>();
            if (exist)
            {
                string[] FileName = Directory.GetFiles(path);
                foreach (string name in FileName)
                {
                    FileNames.Add(Path.GetFileName(name));
                }
            }
            return await Task.FromResult(FileNames);
        }
        public async Task<string> UploadFile(string NazivPrijave, IList<IBrowserFile> ListOfFile)
        {
            string LocationDev = @"C:\KGB_Dev";
            //string Location = @"F:\KGB";
            string path = Path.Combine(LocationDev, User.Naziv_Oj, NazivPrijave);
            //var path = Path.Combine(Location,  User.Naziv_Oj, NazivPrijave);
            CheckFolder(path);
            string pathName = "";
            foreach (var p in ListOfFile.ToList())
            {
                await using FileStream fs = new(path + "\\\\" + p.Name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                pathName = await Task.FromResult(fs.Name.Remove((fs.Name.Length - p.Name.Length), p.Name.Length));
                await p.OpenReadStream().CopyToAsync(fs);
            }
            return pathName;
        }
        public async Task<List<KGB_Category>> GetCategory()
        {
            return await Task.FromResult(_context.KGB_Category.Where(x => x.Sifra_Oj == User.Sifra_Oj).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<List<KGB_Subcategory>> GetSubcategory()
        {
            return await Task.FromResult(_context.KGB_Subcategory.Where(x => x.Sifra_Oj == User.Sifra_Oj).OrderBy(x => x.Id).ToList());
        }
        public async Task<List<KGB_Subcategory>> GetSubcategory(int category_id)
        {
            return await Task.FromResult(_context.KGB_Subcategory.Where(x => x.Fk_Kategorija == category_id).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<Dictionary<string, string>> GetUsersFromOj(int SifraOj)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            List<KGB_User> result = await Task.FromResult(_context.KGB_Users.Where(x => x.Sifra_Oj == SifraOj).OrderBy(x => x.Ime).ToList());
            foreach (KGB_User k in result)
            {
                users.Add(k.Id, k.Ime + " " + k.Prezime);
            }
            return users;
        }
        public async Task<List<KGB_Oj>> GetListOfOrgJed()
        {
            return _context.KGB_OrgJed.ToList();
        }

        public async Task NavigationManager(string nav) => await Task.Run(() => { _navigationManager.NavigateTo(nav, forceLoad: true); });
    }
}
