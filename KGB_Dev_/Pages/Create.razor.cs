using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using KGB_Dev_.Pages.Dialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace KGB_Dev_.Pages
{
    partial class Create
    {
        [Inject]
        public IKgbServices IServices { get; set; } = default!;
        private List<KGB_Category> category;
        private List<KGB_Subcategory> subcategory;
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private Dictionary<int, string?> Subcategory = new Dictionary<int, string?>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        KGB_Knowledge Model = new KGB_Knowledge();
        IList<IBrowserFile> files = new List<IBrowserFile>();
        protected override async Task OnInitializedAsync()
        {
            category = await IServices.GetCategory();
            subcategory = await IServices.GetSubcategory(category.Select(x => x.Id).First());
            Category.Add(0, "Izaberite kategoriju");
            Subcategory.Add(0, "Izaberite potkategoriju");
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
            }
           
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
            }
        }
        private async Task CreateKGB(KGB_Knowledge Model)
        {
            await IServices.CreateKGB(Model, files);
        }
        private async Task Clear(IBrowserFile file)
        {
            files.Remove(file);
        }
        public async void OpenDialog()
        {
            await HandleValidSubmit();
        }
        public async Task HandleValidSubmit()
        {
            DialogService.Show<CategoryDialog>("", dialogOptions);
        }
        public async Task GetSubcategory(int Id)
        {
            subcategory = await IServices.GetSubcategory(Id);
            if (subcategory.Count == 0)
            {
                Subcategory = new();
                Subcategory.Add(0, "Izaberite potkategoriju");
            }
            else
            {
                foreach (var k in subcategory)
                {
                    Subcategory.Add(k.Id, k.Naziv_Potkategorije);
                }
            }
            Model.Fk_Category = Id;

        }
    }
}
