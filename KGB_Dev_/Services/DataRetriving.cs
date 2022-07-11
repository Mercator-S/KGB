using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
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

        public DataRetriving(ApplicationDbContext? context, SignInManager<KGB_User> signInManager, UserManager<KGB_User> userManager, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            _context = context;
            _UserManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }
        public async Task<List<KGB_Knowledge>> GetPublicListOfKnowledge()
        {
            return await Task.FromResult(_context.KGB_Knowledge.Where(x => x.Visibility == true).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<List<KGB_Knowledge>> GetListOfKnowledge(int OrgJed)
        {
            return await Task.FromResult(_context.KGB_Knowledge.Where(x => x.Sifra_Oj == OrgJed && x.Visibility == false).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<KGB_Knowledge> GetKnowledge(long id)
        {
            return await Task.FromResult(_context.KGB_Knowledge.Where(x => x.Id == id).FirstOrDefault());
        }
        public async Task<Task<KGB_User>> GetCurrentUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return _UserManager.GetUserAsync(authState.User);
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
            var user = GetCurrentUser().Result;
            var path = Path.Combine(LocationDev, user.Result.Naziv_Oj, NazivPrijave);
            // var path = Path.Combine(Location, user.Result.Naziv_Oj, NazivPrijave);
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
            var User = GetCurrentUser().Result;
            return await Task.FromResult(_context.KGB_Category.Where(x => x.Sifra_Oj == User.Result.Sifra_Oj).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<List<KGB_Subcategory>> GetSubcategory(int category_id)
        {
            return await Task.FromResult(_context.KGB_Subcategory.Where(x => x.Fk_Kategorija == category_id).OrderByDescending(x => x.Id).ToList());
        }
        public async Task<List<KGB_Subcategory>> GetSubcategory()
        {
            return await Task.FromResult(_context.KGB_Subcategory.OrderBy(x => x.Id).ToList());
        }
        public async Task<Dictionary<string, string>> GetUsersFromOj(int SifraOj)
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            var result = await Task.FromResult(_context.KGB_Users.Where(x => x.Sifra_Oj == SifraOj).OrderBy(x => x.Ime).ToList());
            foreach (var k in result)
            {
                users.Add(k.Id, k.Ime + " " + k.Prezime);
            }
            return users;
        }

        public async Task NavigationManager(string nav) => await Task.Run(() => { _navigationManager.NavigateTo(nav, forceLoad: true); });
    }
}
