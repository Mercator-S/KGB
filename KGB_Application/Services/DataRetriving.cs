using AutoMapper;
using KGB_Application.Interfaces;
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
        private readonly NavigationManager _navigationManager;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;
        private KGB_User User { get; set; }

        public DataRetriving(ApplicationDbContext? context, IMapper mapper, NavigationManager navigationManager, IUserRepository userRepository)
        {
            _context = context;
            _navigationManager = navigationManager;
            _userRepository = userRepository;
            User = _userRepository.GetCurrentUser().Result;
            _mapper = mapper;
        }
        public async Task<List<KGB_KnowledgeViewModel>> GetPublicListOfKnowledge()
        {
            List<KGB_Knowledge> result = _context.KGB_Knowledge.Where(x => x.Visibility == true && x.Active == true).OrderByDescending(x => x.Id).ToList();
            return _mapper.Map<List<KGB_Knowledge?>, List<KGB_KnowledgeViewModel>>(result);
        }
        public async Task<List<KGB_KnowledgeViewModel?>> GetListOfKnowledge(int OrgJed)
        {
            List<KGB_OJKnowledge> KGBoJKnowledge = _context.KGB_OJKnowledge.Where(x => x.Sifra_Oj == OrgJed).ToList();
            if (KGBoJKnowledge.Count >= 1)
            {
                long MaxId = KGBoJKnowledge.Select(x => x.IdPrijave).Max();
                long MinId = KGBoJKnowledge.Select(x => x.IdPrijave).Min();
                List<KGB_Knowledge?> result = _context.KGB_Knowledge.Where(x => x.Id <= MaxId && x.Id >= MinId && x.Visibility == false && x.Active == true).OrderByDescending(x => x.Id).ToList();
                return _mapper.Map<List<KGB_Knowledge?>, List<KGB_KnowledgeViewModel>>(result);
            }
            return new List<KGB_KnowledgeViewModel>();
        }
        public async Task<KGB_Knowledge> GetKnowledge(long id)
        {
            return await Task.FromResult(_context.KGB_Knowledge.FirstOrDefault(x => x.Id == id));
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
                for (int i = 0; i < FileName.Length; i++)
                {
                    FileNames.Add(Path.GetFileName(FileName[i]));
                }
            }
            return await Task.FromResult(FileNames);
        }
        public async Task<string> UploadFile(string NazivPrijave, IList<IBrowserFile> ListOfFile)
        {
            var Location = Directory.GetCurrentDirectory();
            Location = Location.Split(':')[0] + @"\KGB\";
            if (Location.Contains('D'))
            {
                Location = Location.Replace("D", "F:");
            }
            var path = Path.Combine(Location, User.Naziv_Oj, NazivPrijave);
            CheckFolder(path);
            string pathName = "";
            for (int i = 0; i < ListOfFile.ToList().Count; i++)
            {
                await using FileStream fs = new(path + "\\\\" + ListOfFile[i].Name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                pathName = await Task.FromResult(fs.Name.Remove((fs.Name.Length - ListOfFile[i].Name.Length), ListOfFile[i].Name.Length));
                await ListOfFile[i].OpenReadStream().CopyToAsync(fs);
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
            for (int i = 0; i < result.Count; i++)
            {
                users.Add(result[i].Id, result[i].Ime + " " + result[i].Prezime);
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
