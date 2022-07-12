using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KGB_Dev_.Pages.Dialog
{
    partial class FilterDialog
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        KGB_TableFilter Model = new KGB_TableFilter();
        private List<KGB_Category> category;
        private Dictionary<string, string?> Users = new Dictionary<string, string?>();
        private Dictionary<int, string?> DictionaryCategory = new Dictionary<int, string?>();
        private Dictionary<int, string?> DictionarySubcategory = new Dictionary<int, string?>();
        public bool Basic_Switch1 { get; set; }
        DateRange _dateRange = new DateRange(null, null);

        protected override async Task OnInitializedAsync()
        {
            category = await IGetServices.GetCategory();
            var UserOrg = category.Select(x => x.Sifra_Oj).FirstOrDefault();
            Users = IGetServices.GetUsersFromOj(UserOrg).Result;
            DictionaryCategory.Add(0, "Izaberite kategoriju");
            DictionarySubcategory.Add(0, "Izaberite potkategoriju");
            foreach (var p in category)
            {
                DictionaryCategory.Add(p.Id, p.Naziv_Kategorije);
            }
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
                List<KGB_Subcategory> subcategory =  IGetServices.GetSubcategory(Id).Result;
                if (subcategory.Count == 0)
                {
                    DictionarySubcategory = new();
                    DictionarySubcategory.Add(0, "Izaberite potkategoriju");
                }
                else
                {
                    foreach (var k in subcategory)
                    {
                        DictionarySubcategory.Add(k.Id, k.Naziv_Potkategorije);
                    }
                }
                Model.Fk_Category = Id;
                Model.Fk_Subcategory = 0;
            }
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
