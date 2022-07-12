using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.DataRetrieving;
using KGB_Dev_.Services;
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
        MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public long Sifra { get; set; }
        public KGB_Knowledge Model { get; set; }
        private List<KGB_Category> category;
        private List<KGB_Subcategory> subcategory;
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private Dictionary<int, string?> Subcategory = new Dictionary<int, string?>();
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        public DateTime? DatumUnosa { get; set; }
        public DateTime? DatumIzmene { get; set; }
        public string Korisnik { get; set; }
        List<string> FileNames = new List<string>();
        public string FilePath { get; set; }
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        IList<IBrowserFile> files = new List<IBrowserFile>();

        [Inject]
        IJSRuntime JS { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Model = IGetServices.GetKnowledge(Sifra).Result;
            DatumUnosa = Model.d_ins.Date;
            DatumIzmene = Model.d_upd.Date;
            FileNames = IGetServices.GetFile(Model.Putanja_Fajl).Result;
            FilePath = Model.Putanja_Fajl;
            Korisnik = Model.k_ins;
            category = await IGetServices.GetCategory();
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
            }
            subcategory = await IGetServices.GetSubcategory(Model.Fk_Category);
            foreach (var k in subcategory)
            {
                Subcategory.Add(k.Id, k.Naziv_Potkategorije);
            }
        }
        private async Task EditKGB(KGB_Knowledge Model)
        {
            var result = await ICreateServices.EditKGBKnowledge(Model, files);
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
            var dialogResult=await DialogService.Show<CategoryDialog>("", dialogOptions).GetReturnValueAsync<bool>();
            if (dialogResult == true)
            {
                category = await IGetServices.GetCategory();
                Category = new Dictionary<int, string?>();
                Subcategory = new Dictionary<int, string?>();
                foreach (var p in category)
                {
                    Category.Add(p.Id, p.Naziv_Kategorije);
                }
                subcategory = await IGetServices.GetSubcategory(Model.Fk_Category);
                foreach (var k in subcategory)
                {
                    Subcategory.Add(k.Id, k.Naziv_Potkategorije);
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
            using FileStream fs = File.OpenRead(path);
            using var streamRef = new DotNetStreamReference(stream: fs);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                if (FileNames.Contains(file.Name))
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
            string path = FilePath + fileName;
            FileNames.Remove(fileName);
            File.Delete(path);
        }

        void Cancel() => MudDialog.Cancel();
    }
}
