using KGB_Application.Interfaces;
using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Diagnostics;

namespace KGB_Dev_.Pages.Dialog
{
    partial class IndexDialog
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public long Sifra { get; set; }
        public KGB_Knowledge? Prijava { get; set; }
        public KGB_User? User { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public DateTime? DatumIzmene { get; set; }
        public string? Korisnik { get; set; }
        List<string> FileNames = new List<string>();
        public string? FilePath { get; set; }
        public int Lines { get; set; }
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        [Inject]
        public IUserRepository IUserService { get; set; } = default!;
        [Inject]
        IJSRuntime? JS { get; set; }
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };

        protected override async Task OnInitializedAsync()
        {
            User = await IUserService.GetCurrentUser();
            Prijava = IGetServices.GetKnowledge(Sifra).Result;
            DatumUnosa = Prijava.d_ins.Date;
            DatumIzmene = Prijava.d_upd.Date;
            FileNames = IGetServices.GetFile(Prijava.Putanja_Fajl).Result;
            FilePath = Prijava.Putanja_Fajl;
            Korisnik = Prijava.k_ins;
            Lines = CalculateLine(Prijava.Opis_Prijave).Result;
        }

        private async Task DownloadFile(string fileName)
        {
            string path = FilePath + fileName;
            using FileStream fs = File.OpenRead(path);
            using var streamRef = new DotNetStreamReference(stream: fs);
            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        public async void EditDialog(long SifraPrijave)
        {
            Sifra = SifraPrijave;
            await ShowEditDialog();
        }
        public async Task ShowEditDialog()
        {
            DialogParameters parameteres = new DialogParameters();
            parameteres.Add("Sifra", Sifra);
            DialogService.Show<EditDialog>("", parameteres, dialogOptions);
        }
        public async Task<int> CalculateLine(string Prijava)
        {
            int counterN = Prijava.Count(x => x == '\n');
            int result = (Prijava.Length - counterN) / 96;
            if (result == 0)
            {
                counterN += 1;
            }
            return result + counterN;
        }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
