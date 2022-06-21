using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KGB_Dev_.Pages
{
    partial class Create
    {
        [Inject]
        public IKgbServices IServices { get; set; } = default!;
        private List<KGB_Category> category;
        Dictionary<int, string> Category = new Dictionary<int, string>();
        public int value = 1;
        Dictionary<int, string> Subcategory = new Dictionary<int, string>();
        KGB_Knowledge Model = new KGB_Knowledge();
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
           await IServices.CreateKGB(Model,files);
        }
        private async Task Clear(IBrowserFile file)
        {
            files.Remove(file);
        }

    }
}
