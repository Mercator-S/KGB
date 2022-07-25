﻿using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.Interfaces;
using KGB_Dev_.Pages.Dialog;
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
        protected override async Task OnInitializedAsync()
        {
            category = await IGetServices.GetCategory();
            organizacioneJedinice = await IGetServices.GetListOfOrgJed();
            Category = new();
            Subcategory = new();
            Category.Add(0, "Izaberite kategoriju");
            Subcategory.Add(0, "Izaberite potkategoriju");
            OrganizacioneJedinice.Add(0, "Izaberite organizacionu jedinicu");
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
            }
            foreach (KGB_Oj orgJed in organizacioneJedinice)
            {
                OrganizacioneJedinice.Add(orgJed.SifraOj, orgJed.NazivOj);
            }
        }

        private async Task CreateKGB(KGB_KnowledgeViewModel Model)
        {
            await ICreateServices.CreateKGB(Model, files,OrgJed);
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
            foreach (IBrowserFile file in e.GetMultipleFiles())
            {
                files.Add(file);
            }
        }
        public async Task GetListOfOrgJed(int SifraOrgJed)
        {
            if (SifraOrgJed >= 1 && !OrgJed.ContainsKey(SifraOrgJed))
            {
                OrgJed.Add(SifraOrgJed, OrganizacioneJedinice[SifraOrgJed]);
            }
        }
        private async Task RemoveUploadFile(IBrowserFile file) => files.Remove(file);
        private async Task RemoveOrgJed(int Org) => OrgJed.Remove(Org);
    }
}
