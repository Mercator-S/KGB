using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace KGB_Dev_.Data_Retrieving
{
    public class DataRetriving : IKgbServices
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
        public async Task<List<KGB_Knowledge>> GetListOfKnowledge()
        {
            var result = _context.KGB_Knowledge.OrderByDescending(x => x.Id).ToList();
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
        public async Task<bool> CreateKGB(KGB_Knowledge Model, IList<IBrowserFile> ListOfFile)
        {
            var User = GetCurrentUser().Result;
            Model.Sifra_Oj = User.Result.Sifra_Oj;
            Model.k_ins = User.Result.Ime + " " + User.Result.Prezime;
            Model.k_upd = User.Result.Id;
            Model.d_ins = DateTime.Now;
            Model.Sifra_Prijave = "Nepoznato";
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
        public async Task<List<KGB_Category>> GetCategory()
        {
            var result = _context.KGB_Category.ToList();
            return await Task.FromResult(result);
        }
        public void CheckFolder(string Path)
        {
            bool exist = Directory.Exists(Path);
            if (!exist)
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
            string Location = @"C:\KGB_Dev";
            var user = GetCurrentUser().Result;
            var path = Path.Combine(Location, user.Result.Naziv_Oj, NazivPrijave);
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
    }
}
