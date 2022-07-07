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
        private Dictionary<int, string?> Category = new Dictionary<int, string?>();
        private List<KGB_Category> category;
        KGB_KnowledgeViewModel Model = new KGB_KnowledgeViewModel();
        private string searchString1 = "";
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        [Parameter]
        public long IdPrijave { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var User = IServices.GetCurrentUser().Result;
            category = await IServices.GetCategory(User.Result.Sifra_Oj);
            foreach (var p in category)
            {
                Category.Add(p.Id, p.Naziv_Kategorije);
            }
            ListOfKGB = await IServices.GetListOfKnowledge(User.Result.Sifra_Oj);
        }
        public async void Show(long SifraPrijave)
        {
            IdPrijave = SifraPrijave;
            await HandleValidSubmit();
        }
        public async Task HandleValidSubmit()
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
            if (element.k_ins.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
             if (element.Putanja_Fajl.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Naziv_Prijave} {element.Opis_Prijave} {element.k_ins} {element.Putanja_Fajl}".Contains(searchString))
                return true;
            return false;
        }
        //public async Task ChangeList(int Kategorija)
        //{
        //    string a = null;
        //    ListOfKGB = ListOfKGB.Where(x => x.Naziv_Prijave.Contains("")|| x.Opis_Prijave.Contains("")).ToList();
        //}
    }
}
