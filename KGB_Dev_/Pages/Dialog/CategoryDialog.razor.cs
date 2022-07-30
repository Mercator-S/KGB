using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace KGB_Dev_.Pages.Dialog
{
    partial class CategoryDialog
    {
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        private IEnumerable<KGB_Category> _categories;
        private IEnumerable<KGB_Subcategory> _subCategory;
        private IList<KGB_CategoryViewModel> _CategoryViewModels;
        private IList<KGB_SubcategoryViewModel> _SubcategoryViewModels;
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true, Position = DialogPosition.Center, NoHeader = true, DisableBackdropClick = true };

        protected override async Task OnInitializedAsync()
        {
            _CategoryViewModels = new List<KGB_CategoryViewModel>();
            _SubcategoryViewModels = new List<KGB_SubcategoryViewModel>();
            _categories = await IGetServices.GetCategory();
            _subCategory = await IGetServices.GetSubcategory();
            foreach (KGB_Subcategory SubCat in _subCategory)
            {
                _SubcategoryViewModels.Add(new KGB_SubcategoryViewModel { Id = SubCat.Id, Naziv_Potkategorije = SubCat.Naziv_Potkategorije, Fk_Kategorija = SubCat.Fk_Kategorija });
            }
            foreach (KGB_Category Cat in _categories)
            {
                _CategoryViewModels.Add(new KGB_CategoryViewModel { Id = Cat.Id, Naziv_Kategorije = Cat.Naziv_Kategorije, Subcategory = _SubcategoryViewModels.Where(x => x.Fk_Kategorija == Cat.Id).ToList() });
            }

        }
        private async void ShowBtnPress(int id)
        {
            KGB_CategoryViewModel kgb = _CategoryViewModels.First(x => x.Id == id);
            kgb.ShowSubcategory = !kgb.ShowSubcategory;
        }
        public async Task OpenDialogForSubcategory(KGB_CategoryViewModel model)
        {
            DialogParameters parameter = new DialogParameters { ["Category"] = model };
            bool result = await DialogService.Show<CreateSubcategoryDialog>("", parameter, dialogOptions).GetReturnValueAsync<bool>();
            if (result)
            {
                await OnInitializedAsync();
            }
        }
        public async Task OpenDialogForCategory()
        {
            bool result = await DialogService.Show<CreateCategoryDialog>("", dialogOptions).GetReturnValueAsync<bool>();
            if (result)
            {
                await OnInitializedAsync();
            }
        }
        public async Task Submit()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
