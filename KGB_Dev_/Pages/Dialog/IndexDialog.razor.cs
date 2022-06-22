using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace KGB_Dev_.Pages.Dialog
{
    partial class IndexDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public long Sifra { get; set; }
        public KGB_Knowledge Prijava { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public DateTime? DatumIzmene { get; set; }
        public string Korisnik { get; set; }
        List<string> FileNames = new List<string>();
        public string FilePath { get; set; }
        [Inject]
        public IKgbServices IServices { get; set; } = default!;
        [Inject]
        IJSRuntime JS { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Prijava = IServices.GetKnowledge(Sifra).Result;
            DatumUnosa = Prijava.d_ins.Date;
            DatumIzmene = Prijava.d_upd.Date;
            FileNames = IServices.GetFile(Prijava.Putanja_Fajl).Result;
            FilePath = Prijava.Putanja_Fajl;
            Korisnik = Prijava.k_ins;
        }

        private async Task DownloadFile(string fileName)
        {
            var fileStream = IServices.GetFileStream();
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
