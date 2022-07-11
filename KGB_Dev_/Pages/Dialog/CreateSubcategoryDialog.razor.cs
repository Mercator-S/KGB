using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace KGB_Dev_.Pages.Dialog
{
    partial class CreateSubcategoryDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public KGB_CategoryViewModel Category { get; set; } = new KGB_CategoryViewModel();
        KGB_SubcategoryViewModel Model = new KGB_SubcategoryViewModel();
        public string ValidationMessage { get; set; }
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;
        private async Task CreateSubcategory(KGB_SubcategoryViewModel Model, KGB_CategoryViewModel Category)
        {
            Model.Fk_Kategorija = Category.Id;
            var result = await ICreateServices.CreateSubCategory(Model);
            if (!result)
            {
                Snackbar.Add($"Uspešno dodata kategorija {Model.Naziv_Potkategorije}", Severity.Success);
                MudDialog.Cancel();
                DialogService.Show<CategoryDialog>("", dialogOptions);
            }
            ValidationMessage = "Potkategorija sa ovim nazivom vec postoji!";

        }
        void Cancel()
        {
            MudDialog.Cancel();
            DialogService.Show<CategoryDialog>("", dialogOptions);
        }
    }
}

