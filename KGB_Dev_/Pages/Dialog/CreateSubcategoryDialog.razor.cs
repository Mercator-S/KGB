using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace KGB_Dev_.Pages.Dialog
{
    partial class CreateSubcategoryDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        [Parameter]
        public KGB_CategoryViewModel Category { get; set; } = new KGB_CategoryViewModel();
        KGB_SubcategoryViewModel Model = new KGB_SubcategoryViewModel();
        public string ValidationMessage { get; set; }
        private async Task CreateSubcategory(KGB_SubcategoryViewModel Model, KGB_CategoryViewModel Category)
        {
            Model.Fk_Kategorija = Category.Id;
            var result = await ICreateServices.CreateSubCategory(Model);
            if (result)
            {
                Snackbar.Add($"Uspešno dodata kategorija {Model.Naziv_Potkategorije}", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            ValidationMessage = "Potkategorija sa ovim nazivom vec postoji!";
        }
        void Cancel() => MudDialog.Close(DialogResult.Ok(false));
    }
}

