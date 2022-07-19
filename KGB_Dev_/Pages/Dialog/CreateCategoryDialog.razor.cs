using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KGB_Dev_.Pages.Dialog
{
    partial class CreateCategoryDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public KGB_CategoryViewModel Category { get; set; } = new KGB_CategoryViewModel();
        public string ValidationMessage { get; set; }
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        private async Task CreateCategory(KGB_CategoryViewModel Category)
        {
            var result = await ICreateServices.CreateCategory(Category);
            if (result)
            {
                Snackbar.Add($"Uspešno dodata potkategorija {Category.Naziv_Kategorije}", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            ValidationMessage = "Kategorija sa ovim nazivom vec postoji!";
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Close(DialogResult.Ok(false));
    }
}
