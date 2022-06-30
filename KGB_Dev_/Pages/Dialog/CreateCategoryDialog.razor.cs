using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.DataRetrieving;
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
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        [Inject]
        public ICreateServices ICreateServices { get; set; } = default!;
        [Inject]
        ISnackbar Snackbar { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {

        }
        private async Task CreateCategory(KGB_CategoryViewModel Category)
        {
            var result = await ICreateServices.CreateCategory(Category);
            if (!result)
            {
                Snackbar.Add($"Uspešno dodata potkategorija {Category.Naziv_Kategorije}", Severity.Success);
                MudDialog.Cancel();
                DialogService.Show<CategoryDialog>("", dialogOptions);
            }
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel()
        {
            MudDialog.Cancel();
            DialogService.Show<CategoryDialog>("", dialogOptions);
        }
    }
}
