using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KGB_Dev_.Pages
{
    partial class Create
    {
        [Inject]
        IWebHostEnvironment Environment { get; set; } = default!;
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        private List<KGB_Category> category;
        Dictionary<int, string> Category = new Dictionary<int, string>();
        public int value = 1;
        Dictionary<int, string> Subcategory = new Dictionary<int, string>();
        KGB_Knowledge Model = new KGB_Knowledge();
        private bool Clearing = false;
        string Location = @"Desktop";
        IList<IBrowserFile> files = new List<IBrowserFile>();

        protected override async Task OnInitializedAsync()
        {
            category = await IServices.GetCategory();
            foreach (var p in category)
            {
                Category.Add(p.Sifra_Kategorije, p.Naziv_Kategorije);
            }
            foreach (var k in category)
            {
                Subcategory.Add(k.Sifra_Potkategorije, k.Naziv_Potkategorije);
            }
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
            }
            //TODO upload the files to the server
        }
        private async Task CreateKGB(KGB_Knowledge Model)
        {
            var user = IServices.GetCurrentUser().Result;
           var path = Path.Combine(Location, user.Result.Naziv_Oj, Model.Naziv_Prijave);
           IServices.CheckFolder(path);
            foreach (var p in files)
            {
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                //var path = Path.Combine(Environment.ContentRootPath,
                //        Environment.EnvironmentName, "unsafe_uploads",
                //        trustedFileNameForFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await p.OpenReadStream().CopyToAsync(fs);
            }
            var result = IServices.CreateKGB(Model);
        }
        private async Task Clear(IBrowserFile file)
        {
            files.Remove(file);
        }

    }
}
