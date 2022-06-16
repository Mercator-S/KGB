using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using KGB_Dev_.Pages.Dialog;

namespace KGB_Dev_.Pages
{
    partial class Index
    {
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        private IEnumerable<KGB_Knowledge> ListOfKGB;
        private string searchString1 = "";
        DialogOptions maxWidth = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position=DialogPosition.Center };
        [Parameter]
        public long Sifra { get; set; }
        protected override async Task OnInitializedAsync()
        {
            ListOfKGB = await IServices.GetListOfKnowledge();
        }
        public async void Show(long a)
        {
            Sifra = a;
           await HandleValidSubmit();
        }
        public async Task HandleValidSubmit()
        {
            var parameteres = new DialogParameters { { "Sifra", Sifra } };
            DialogService.Show<IndexDialog>("", parameteres, maxWidth);
        }
    }
}
