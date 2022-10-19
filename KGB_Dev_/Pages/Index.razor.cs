using Microsoft.AspNetCore.Components;
using MudBlazor;
using KGB_Dev_.Pages.Dialog;
using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using KGB_Domain.KGB_SpecificDataType;
using KGB_Application.Interfaces;

namespace KGB_Dev_.Pages
{
    partial class Index
    {
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        [Inject]
        public IUserRepository IUserService { get; set; } = default!;
        private IEnumerable<KGB_KnowledgeViewModel?> ListOfKGB;
        private IEnumerable<KGB_KnowledgeViewModel?> BaseListOfKGB;
        private List<KGB_CategoryTypeModel> CategoryModel { get; set; }
        private List<KGB_SubCategoryTypeModel> SubCategoryModel = new List<KGB_SubCategoryTypeModel>();
        private string searchString1 = "";
        private KGB_User UserSifraOj;
        private KGB_TableFilter FilterModel = new KGB_TableFilter();
        private Dictionary<string, string?> FilterUsers = new Dictionary<string, string?>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        private bool HideFilter { get; set; } = true;
        DateRange DateIns = new DateRange(null, null);
        DateRange DateUpd = new DateRange(null, null);

        protected override async Task OnInitializedAsync()
        {
            UserSifraOj = await IUserService.GetCurrentUser();
            BaseListOfKGB = await IServices.GetListOfKnowledge(UserSifraOj.Sifra_Oj);
            ListOfKGB = BaseListOfKGB;
            CategoryModel = new List<KGB_CategoryTypeModel>();
            CategoryModel.Add(new KGB_CategoryTypeModel(0, "Izaberite kategoriju"));
            SubCategoryModel.Add(new KGB_SubCategoryTypeModel(0, "Izaberite potkategoriju"));
            var Kategorije = ListOfKGB.Select(x => new { x.Fk_Category, x.Naziv_Kategorije }).Distinct().ToList();
            var User = ListOfKGB.Select(x => new { x.k_ins, x.k_name }).Distinct().ToList();
            foreach (var item in Kategorije)
            {
                CategoryModel.Add(new KGB_CategoryTypeModel(item.Fk_Category, item.Naziv_Kategorije));
            }
            foreach (var Users in User)
            {
                FilterUsers.Add(Users.k_ins, Users.k_name);
            }
        }
        public async Task TableDetailsDialog(long IdPrijave)
        {
            DialogParameters parameteres = new DialogParameters();
            parameteres.Add("Sifra", IdPrijave);
            DialogService.Show<IndexDialog>("", parameteres, dialogOptions);
        }
        public void FilterDialog()
        {
            HideFilter = !HideFilter;
        }
        private bool SearchTable1(KGB_KnowledgeViewModel element) => SearchTable(element, searchString1);

        private bool SearchTable(KGB_KnowledgeViewModel element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Naziv_Prijave.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Opis_Prijave.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.k_name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Naziv_Prijave} {element.Opis_Prijave} {element.k_name}".Contains(searchString))
                return true;
            return false;
        }

        public async Task GetSubcategory(int Id)
        {
            var Potkategorije = ListOfKGB.Where(x => x.Fk_Category == Id).Select(x => new { x.Fk_Subcategory, x.Naziv_Potkategorije }).Distinct().ToList();
            if (Potkategorije.Count != 0)
            {
                SubCategoryModel = new List<KGB_SubCategoryTypeModel>();
                SubCategoryModel.Add(new KGB_SubCategoryTypeModel(0, "Izaberite potkategoriju"));
                foreach (var item in Potkategorije)
                {
                    SubCategoryModel.Add(new KGB_SubCategoryTypeModel(item.Fk_Subcategory, item.Naziv_Potkategorije));
                }
            }
            FilterModel.Fk_Category = Id;
            FilterModel.Fk_Subcategory = 0;
        }
        public async Task Filter(KGB_TableFilter Filter)
        {
            ListOfKGB = BaseListOfKGB;
            if (Filter.User != null && DateIns.Start != null && DateUpd.Start != null)
            {
                ListOfKGB = ListOfKGB.Where(x => x.k_upd == Filter.User &&
                x.d_ins.Date >= DateIns.Start && x.d_ins.Date <= DateIns.End &&
                x.d_upd.Date >= DateUpd.Start && x.d_upd.Date <= DateUpd.End).ToList();
            }
            else if (Filter.Fk_Category != 0 && Filter.Fk_Subcategory != 0)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category && x.Fk_Subcategory == Filter.Fk_Subcategory).ToList();
            }
            else if (Filter.Fk_Category != 0)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category).ToList();

            }
            else if (Filter.Fk_Category == 0 && Filter.Fk_Subcategory == 0 && Filter.User == null && DateIns.Start == null && DateUpd.Start == null) { }
            else
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category || x.Fk_Subcategory == Filter.Fk_Subcategory || x.k_upd == Filter.User ||
              (x.d_ins.Date >= DateIns.Start && x.d_ins.Date <= DateIns.End) || (x.d_upd.Date >= DateUpd.Start && x.d_upd.Date <= DateUpd.End)).ToList();
            }
        }
        public async Task CloseFilter()
        {
            HideFilter = !HideFilter;
        }
        public async Task ResetFilter()
        {
            ListOfKGB = BaseListOfKGB;
            FilterModel = new KGB_TableFilter();
            DateIns = new DateRange(null, null);
            DateUpd = new DateRange(null, null);
        }
    }
}
