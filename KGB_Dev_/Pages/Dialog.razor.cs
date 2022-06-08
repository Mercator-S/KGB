using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KGB_Dev_.Pages
{
    partial class Dialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Parameter] 
        public long Sifra { get; set; }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
