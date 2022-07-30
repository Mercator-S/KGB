using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Diagnostics;

namespace KGB_Dev_.Pages.Dialog
{
    partial class EditDialog
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        IJSRuntime JS { get; set; }
        [Parameter]
        public long Sifra { get; set; }
        public KGB_Knowledge? Model { get; set; }
        private List<KGB_Category> category;
        private List<KGB_Subcategory> subcategory;
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private Dictionary<int, string?> Subcategory = new Dictionary<int, string?>();
        public int CountFiles { get; set; }
        public string? FilePath { get; set; }
        IList<IBrowserFile> files = new List<IBrowserFile>();
        List<string> FileNames = new List<string>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };

        protected override async Task OnInitializedAsync()
        {
            Model = await IGetServices.GetKnowledge(Sifra);
            FileNames = await IGetServices.GetFile(Model.Putanja_Fajl);
            subcategory = await IGetServices.GetSubcategory(Model.Fk_Category);
            category = await IGetServices.GetCategory();
            FilePath = Model.Putanja_Fajl;
            foreach (KGB_Category Cat in category)
            {
                Category.Add(Cat.Id, Cat.Naziv_Kategorije);
            }
            foreach (KGB_Subcategory SubCat in subcategory)
            {
                Subcategory.Add(SubCat.Id, SubCat.Naziv_Potkategorije);
            }
        }
        private async Task EditKGB(KGB_Knowledge EditModel)
        {
            bool result = await ICreateServices.EditKGBKnowledge(EditModel, files);
            if (result == true)
            {
                Cancel();
            }
            else
            {
                Snackbar.Add($"Greska prilikom izmene prijave!", Severity.Error);
            }
        }
        public async Task OpenCategoryDialog()
        {
            bool dialogResult = await DialogService.Show<CategoryDialog>("", dialogOptions).GetReturnValueAsync<bool>();
            if (dialogResult == true)
            {
                category = await IGetServices.GetCategory();
                subcategory = await IGetServices.GetSubcategory(Model.Fk_Category);
                Category = new Dictionary<int, string?>();
                Subcategory = new Dictionary<int, string?>();
                foreach (KGB_Category Cat in category)
                {
                    Category.Add(Cat.Id, Cat.Naziv_Kategorije);
                }
                foreach (KGB_Subcategory SubCat in subcategory)
                {
                    Subcategory.Add(SubCat.Id, SubCat.Naziv_Potkategorije);
                }
            }
        }
        public async Task GetSubcategory(int Id)
        {
            if (Id == 0)
            {
                Subcategory = new();
                Subcategory.Add(0, "Izaberite potkategoriju");
                Model.Fk_Subcategory = 0;
                Model.Fk_Category = Id;
            }
            else
            {
                subcategory = await IGetServices.GetSubcategory(Id);
                if (subcategory.Count == 0)
                {
                    Subcategory = new();
                    Subcategory.Add(0, "Izaberite potkategoriju");
                }
                else
                {
                    Subcategory = new();
                    Subcategory.Add(0, "Izaberite potkategoriju");
                    foreach (var k in subcategory)
                    {
                        Subcategory.Add(k.Id, k.Naziv_Potkategorije);
                    }
                }
                Model.Fk_Category = Id;
                Model.Fk_Subcategory = 0;
            }
        }
        private async Task DownloadFile(string fileName)
        {
            string path = FilePath + fileName;
            if (!File.Exists(path))
            {
                Snackbar.Add($"Ne mozete preuzeti dodati fajl", Severity.Error);
            }
            else
            {
                using FileStream fs = File.OpenRead(path);
                using var streamRef = new DotNetStreamReference(stream: fs);
                await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {

            foreach (IBrowserFile file in e.GetMultipleFiles())
            {
                if (FileNames.Contains(file.Name) || files.Contains(file))
                {
                    Snackbar.Add($"File sa nazivom {file.Name} vec postoji ", Severity.Error);
                }
                else
                {
                    files.Add(file);
                    FileNames.Add(file.Name);
                }
            }
        }
        private async Task DeleteFile(string fileName)
        {
            string[] Files = Directory.GetFiles(FilePath);
            FileNames.Remove(fileName);
            if (files.Count >= 1)
            {
                IBrowserFile? deleteItem = files.Where(x => x.Name == fileName).FirstOrDefault();
                files.Remove(deleteItem);
            }
            foreach (var item in Files)
            {
                if (item == FilePath + fileName)
                {
                    File.Delete(item);
                }
            }
        }
        private async Task DeleteKGB(KGB_Knowledge Model)
        {
            bool result = await ICreateServices.DeleteKGBKnowledge(Model);
            if (result == true)
            {
                Cancel();
            }
            else
            {
                Snackbar.Add($"Greska prilikom izmene prijave!", Severity.Error);
            }

        }
        void Cancel() => MudDialog.Cancel();
    }
}
