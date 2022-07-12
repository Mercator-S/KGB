using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using KGB_Dev_.Pages.Dialog;

namespace KGB_Dev_.Pages
{
    partial class PublicIndex
    {
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        private IEnumerable<KGB_Knowledge> ListOfKGB;
        private string searchString1 = "";
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        [Parameter]
        public long IdPrijave { get; set; }
        private KGB_TableFilter FilterModel = new KGB_TableFilter();
        private bool HideFilter { get; set; } = true;
        private Dictionary<int, string?> DictionarySifraOj = new Dictionary<int, string?>();
        DateRange DateIns = new DateRange(null, null);
        DateRange DateUpd = new DateRange(null, null);
        protected override async Task OnInitializedAsync()
        {
            ListOfKGB = await IServices.GetPublicListOfKnowledge();
            DictionarySifraOj.Add(0, "Izaberite organizacionu jedinicu");
            foreach (var item in ListOfKGB)
            {
                if (!DictionarySifraOj.ContainsKey(item.Sifra_Oj))
                {
                    DictionarySifraOj.Add(item.Sifra_Oj, item.Naziv_Oj);
                }
            }
        }
        public async void Show(long SifraPrijave)
        {
            IdPrijave = SifraPrijave;
            await ShowKGB();
        }
        public async Task ShowKGB()
        {
            var parameteres = new DialogParameters();
            parameteres.Add("Sifra", IdPrijave);
            DialogService.Show<IndexDialog>("", parameteres, dialogOptions);
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
            if (element.Putanja_Fajl.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Naziv_Oj.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.k_name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Naziv_Prijave} {element.Opis_Prijave} {element.Putanja_Fajl} {element.Naziv_Oj} {element.k_name}".Contains(searchString))
                return true;
            return false;
        }
        public async Task FilterDialog()
        {
            HideFilter = !HideFilter;
        }
        public async Task Filter(KGB_TableFilter Filter, DateRange DateIns, DateRange DateUpd)
        {
            ListOfKGB = await IServices.GetPublicListOfKnowledge();
            if (Filter.SifraOj != 0 && DateIns.Start == null && DateUpd.Start == null)
            {
                ListOfKGB = ListOfKGB.Where(x => x.Sifra_Oj == Filter.SifraOj).ToList();
            }
            else if (Filter.SifraOj == 0 && DateIns.Start == null && DateUpd.Start == null) { }
            else
            {
                ListOfKGB = ListOfKGB.Where(x => x.Sifra_Oj == Filter.SifraOj ||
                (x.d_ins >= DateIns.Start && x.d_ins <= DateIns.End) || (x.d_upd >= DateUpd.Start && x.d_upd <= DateUpd.End)).ToList();
            }
        }
        public async Task CloseFilter()
        {
            ListOfKGB = await IServices.GetPublicListOfKnowledge();
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
