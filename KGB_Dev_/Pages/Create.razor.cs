﻿using KGB_Dev_.Interfaces;
using KGB_Dev_.Pages.Dialog;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace KGB_Dev_.Pages
{
    partial class Create
    {
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        private List<KGB_Category> category;
        private List<KGB_Subcategory> subcategory;
        private List<KGB_Oj?> organizacioneJedinice;
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private Dictionary<int, string?> Subcategory = new Dictionary<int, string?>();
        private Dictionary<int, string?> OrganizacioneJedinice = new Dictionary<int, string?>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        KGB_KnowledgeViewModel Model = new KGB_KnowledgeViewModel();
        IList<IBrowserFile> files = new List<IBrowserFile>();
        Dictionary<int, string?> OrgJed = new Dictionary<int, string?>();
        string OrgJedSearchValue;
        protected override async Task OnInitializedAsync()
        {
            category = await IGetServices.GetCategory();
            if (OrganizacioneJedinice.Count == 0)
            {
                organizacioneJedinice = await IGetServices.GetListOfOrgJed();
                foreach (KGB_Oj orgJed in organizacioneJedinice)
                {
                    OrganizacioneJedinice.Add(orgJed.SifraOj, orgJed.NazivOj);
                }
            }
            Category = new();
            Subcategory = new();
            Category.Add(0, "Izaberite kategoriju");
            Subcategory.Add(0, "Izaberite potkategoriju");
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
            }
        }

        private async Task CreateKGB(KGB_KnowledgeViewModel Model)
        {
            Model.Naziv_Kategorije = Category[Model.Fk_Category];
            Model.Naziv_Potkategorije = Subcategory[Model.Fk_Subcategory];
            await ICreateServices.CreateKGB(Model, files, OrgJed);
            Snackbar.Add($"Uspešno dodata KGB prijava pod nazivom {Model.Naziv_Prijave}", Severity.Success);
        }
        public async Task OpenCategoryDialog()
        {
            bool dialogResult = await DialogService.Show<CategoryDialog>("", dialogOptions).GetReturnValueAsync<bool>();
            if (dialogResult)
            {
                await OnInitializedAsync();
                await GetSubcategory(Model.Fk_Category);
            }
        }
        public async Task GetSubcategory(int Id)
        {
            if (Id == 0)
            {
                Subcategory = new();
                Subcategory.Add(0, "Izaberite potkategoriju");
                Model.Fk_Subcategory = 0;
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
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            if (e.FileCount > 10 || files.Count >= 10)
            {
                Snackbar.Add($"Nije moguće dodati više 10 file-ova", Severity.Error);
            }
            else
            {
                foreach (IBrowserFile file in e.GetMultipleFiles())
                {
                    if ((file.Size / 1024) <= 512000)
                    {
                        files.Add(file);
                    }
                    else
                    {
                        Snackbar.Add($"Nije moguće dodati file veći od 512 MB {file.Name}", Severity.Error);
                    }
                }
            }
        }
        private async Task<IEnumerable<string>> Search1(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new string[0];
            return (IEnumerable<string>)OrganizacioneJedinice.Values.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        public async Task GetListOfOrgJedString(string NazivOrgJed)
        {
            int SifraOrgJed = OrganizacioneJedinice.FirstOrDefault(x => x.Value == NazivOrgJed).Key;
            if (!OrgJed.ContainsValue(NazivOrgJed) && OrgJed.Count <= 10)
            {
                OrgJed.Add(SifraOrgJed, NazivOrgJed);
            }
            else if (OrgJed.Count >= 10)
            {
                Snackbar.Add($"Nije moguće dodati više od 10 organizacionih jedinica!", Severity.Error);
            }
            else
            {
                Snackbar.Add($"Organizaciona jedinica {NazivOrgJed} već dodata", Severity.Error);
            }
        }
        private async Task RemoveUploadFile(IBrowserFile file) => files.Remove(file);
        private async Task RemoveOrgJed(int Org) => OrgJed.Remove(Org);
    }
}
