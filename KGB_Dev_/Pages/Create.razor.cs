using KGB_Dev_.Interfaces;
using KGB_Dev_.Pages.Dialog;
using KGB_Domain.KGB_SpecificDataType;
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
        private List<KGB_Category> Category;
        private List<KGB_Subcategory> Subcategory;
        private List<KGB_Oj> organizacioneJedinice;
        private List<KGB_CategoryTypeModel> CategoryModel { get; set; }
        private List<KGB_SubCategoryTypeModel> SubCategoryModel = new List<KGB_SubCategoryTypeModel>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        KGB_KnowledgeViewModel Model = new KGB_KnowledgeViewModel();
        IList<IBrowserFile> files = new List<IBrowserFile>();
        List<KGB_OrgJed> OrganizacioneJedinice = new List<KGB_OrgJed>();
        List<KGB_OrgJed> OrgJed { get; set; }
        string OrgJedSearchValue;
        protected override async Task OnInitializedAsync()
        {
            OrgJed = new List<KGB_OrgJed>();
            Category = await IGetServices.GetCategory();
            Subcategory = await IGetServices.GetSubcategory();
            if (OrgJed.Count == 0)
            {
                organizacioneJedinice = await IGetServices.GetListOfOrgJed();
                foreach (KGB_Oj orgJed in organizacioneJedinice)
                {
                    OrganizacioneJedinice.Add(new KGB_OrgJed(orgJed.SifraOj, orgJed.NazivOj));
                }
            }
            CategoryModel = new List<KGB_CategoryTypeModel>();
            CategoryModel.Add(new KGB_CategoryTypeModel(0, "Izaberite kategoriju"));
            SubCategoryModel.Add(new KGB_SubCategoryTypeModel(0, "Izaberite potkategoriju"));
            foreach (var category in Category)
            {
                CategoryModel.Add(new KGB_CategoryTypeModel(category.Id, category.Naziv_Kategorije));
            }
        }

        private async Task CreateKGB(KGB_KnowledgeViewModel Model)
        {
            Model.Naziv_Kategorije = CategoryModel.Where(x => x.SifraKategorije == Model.Fk_Category).First().NazivKategorije;
            Model.Naziv_Potkategorije = SubCategoryModel.Where(x => x.SifraPotkategorije == Model.Fk_Subcategory).First().NazivPotkategorije;
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
            List<KGB_Subcategory> ListOfSubCategory = new List<KGB_Subcategory>(Subcategory.Where(x => x.Fk_Kategorija == Id).ToList());
            SubCategoryModel = new List<KGB_SubCategoryTypeModel>();
            SubCategoryModel.Add(new KGB_SubCategoryTypeModel(0, "Izaberite potkategoriju"));
            if (ListOfSubCategory.Count >= 1)
            {
                foreach (var subCat in ListOfSubCategory)
                {
                    SubCategoryModel.Add(new KGB_SubCategoryTypeModel(subCat.Id, subCat.Naziv_Potkategorije));
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
            return OrganizacioneJedinice.Select(x => x.NazivOj).Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
        public async Task GetListOfOrgJedString(string NazivOrgJed)
        {
            int SifraOrgJed = OrganizacioneJedinice.FirstOrDefault(x => x.NazivOj == NazivOrgJed).SifraOj;
            if (!OrgJed.Contains(new KGB_OrgJed(SifraOrgJed, NazivOrgJed)) && OrgJed.Count <= 10)
            {
                OrgJed.Add(new KGB_OrgJed(SifraOrgJed, NazivOrgJed));
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
        private async Task RemoveOrgJed(int Org) => OrgJed.Remove(OrgJed.Where(x => x.SifraOj == Org).FirstOrDefault());
    }
}
