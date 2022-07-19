using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using KGB_Dev_.Pages.Dialog;
using KGB_Dev_.Data.KGB_ViewModel;

namespace KGB_Dev_.Pages
{
    partial class Index
    {
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        private IEnumerable<KGB_Knowledge> ListOfKGB;
        private List<KGB_Category> category;
        private string searchString1 = "";
        private int UserSifraOj = 0;
        private KGB_TableFilter FilterModel = new KGB_TableFilter();
        private Dictionary<string, string> FilterUsers = new Dictionary<string, string>();
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private Dictionary<int, string?> DictionaryCategory = new Dictionary<int, string?>();
        private Dictionary<int, string?> DictionarySubcategory = new Dictionary<int, string?>();
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        [Parameter]
        public long IdPrijave { get; set; }
        private bool HideFilter { get; set; } = true;
        DateRange DateIns = new DateRange(null, null);
        DateRange DateUpd = new DateRange(null, null);

        protected override async Task OnInitializedAsync()
        {
            UserSifraOj = IServices.GetCurrentUser().Result.Sifra_Oj;
            FilterUsers = IServices.GetUsersFromOj(UserSifraOj).Result;
            category = await IServices.GetCategory();
            DictionaryCategory.Add(0, "Izaberite kategoriju");
            DictionarySubcategory.Add(0, "Izaberite potkategoriju");
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
                DictionaryCategory.Add(p.Id, p.Naziv_Kategorije);
            }
            ListOfKGB = await IServices.GetListOfKnowledge(UserSifraOj);
        }
        public async Task TableDetailsDialog()
        {
            var parameteres = new DialogParameters();
            parameteres.Add("Sifra", IdPrijave);
            DialogService.Show<IndexDialog>("", parameteres, dialogOptions);
        }
        public async void ShowTableDetailsDialog(long SifraPrijave)
        {
            IdPrijave = SifraPrijave;
            await TableDetailsDialog();
        }
        public void FilterDialog()
        {
            HideFilter = !HideFilter;
        }
        private bool SearchTable1(KGB_Knowledge element) => SearchTable(element, searchString1);

        private bool SearchTable(KGB_Knowledge element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Naziv_Prijave.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Opis_Prijave.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Putanja fajla trenutno sadrzi samo id prijave tako da ne moze da se pretrazuje, nadji neko resenje ako moze ako ne videti kako raditi.
            //if (element.Putanja_Fajl != "")
            //{
            //    if (element.Putanja_Fajl.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            //        return true;
            //}
            if (element.k_name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Naziv_Prijave} {element.Opis_Prijave} {element.k_name}".Contains(searchString))
                return true;
            return false;
        }

        public async Task GetSubcategory(int Id)
        {
            if (Id == 0)
            {
                DictionarySubcategory = new();
                DictionarySubcategory.Add(0, "Izaberite potkategoriju");
            }
            else
            {
                List<KGB_Subcategory> subcategory = IServices.GetSubcategory(Id).Result;
                if (subcategory.Count == 0)
                {
                    DictionarySubcategory = new();
                    DictionarySubcategory.Add(0, "Izaberite potkategoriju");
                }
                else
                {
                    DictionarySubcategory = new();
                    DictionarySubcategory.Add(0, "Izaberite potkategoriju");
                    foreach (var k in subcategory)
                    {
                        DictionarySubcategory.Add(k.Id, k.Naziv_Potkategorije);
                    }
                }
                FilterModel.Fk_Category = Id;
                FilterModel.Fk_Subcategory = 0;
            }
        }
        public async Task Filter(KGB_TableFilter Filter)
        {
            ListOfKGB = await IServices.GetListOfKnowledge(UserSifraOj);
            if (Filter.Fk_Category != 0 && Filter.Fk_Subcategory != 0 && Filter.User != null && DateIns.Start != null && DateUpd.Start != null)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category && x.Fk_Subcategory == Filter.Fk_Subcategory && x.k_upd == Filter.User &&
                (x.d_ins.Date >= DateIns.Start && x.d_ins.Date <= DateIns.End) && (x.d_upd.Date >= DateUpd.Start && x.d_upd.Date <= DateUpd.End)).ToList();
            }
            else if (Filter.Fk_Category != 0)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category).ToList();

            }
            else if (Filter.Fk_Category != 0 && Filter.Fk_Subcategory != 0)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Fk_Category == Filter.Fk_Category && x.Fk_Subcategory == Filter.Fk_Subcategory).ToList();
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
            ListOfKGB = await IServices.GetListOfKnowledge(UserSifraOj);
            FilterModel = new KGB_TableFilter();
            DateIns = new DateRange(null, null);
            DateUpd = new DateRange(null, null);
            HideFilter = !HideFilter;
        }
        public async Task ResetFilter()
        {
            //ListOfKGB = await IServices.GetListOfKnowledge(UserSifraOj);
            FilterModel = new KGB_TableFilter();
            DateIns = new DateRange(null, null);
            DateUpd = new DateRange(null, null);
        }
    }
}
