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
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        private IEnumerable<KGB_Knowledge> ListOfKGB;
        private string searchString1 = "";
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        [Parameter]
        public long IdPrijave { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ListOfKGB = await IGetServices.GetPublicListOfKnowledge();
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
            if ($"{element.Naziv_Prijave} {element.Opis_Prijave} {element.k_ins}".Contains(searchString))
                return true;
            return false;
        }
    }
}
