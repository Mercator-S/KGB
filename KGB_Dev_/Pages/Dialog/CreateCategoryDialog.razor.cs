using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KGB_Dev_.Pages.Dialog
{
    partial class CreateCategoryDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        [Parameter]
        public KGB_CategoryViewModel Category { get; set; } = new KGB_CategoryViewModel();
        public string ValidationMessage { get; set; }

        private async Task CreateCategory(KGB_CategoryViewModel Category)
        {
            bool result = await ICreateServices.CreateCategory(Category);
            if (result)
            {
                Snackbar.Add($"Uspešno dodata potkategorija {Category.Naziv_Kategorije}", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            ValidationMessage = "Kategorija sa ovim nazivom vec postoji!";
        }
        void Cancel() => MudDialog.Close(DialogResult.Ok(false));
    }
}
