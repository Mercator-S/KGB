using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace KGB_Dev_.Pages.Dialog
{
    partial class CategoryDialog
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        private IEnumerable<KGB_Category> _categories;
        private IEnumerable<KGB_Subcategory> _subCategory;
        private IList<KGB_CategoryViewModel> _CategoryViewModels;
        private IList<KGB_SubcategoryViewModel> _SubcategoryViewModels;
        DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true, Position = DialogPosition.Center, NoHeader = true };
        public bool ShowSubcategory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var User = IGetServices.GetCurrentUser().Result;
            _CategoryViewModels = new List<KGB_CategoryViewModel>();
            _SubcategoryViewModels = new List<KGB_SubcategoryViewModel>();
            _categories = await IGetServices.GetCategory(User.Result.Sifra_Oj);
            _subCategory = await IGetServices.GetSubcategory();
            foreach (var l in _subCategory)
            {
                _SubcategoryViewModels.Add(new KGB_SubcategoryViewModel { Id = l.Id, Naziv_Potkategorije = l.Naziv_Potkategorije, Fk_Kategorija = l.Fk_Kategorija });
            }
            foreach (var p in _categories)
            {
                _CategoryViewModels.Add(new KGB_CategoryViewModel { Id = p.Id, Naziv_Kategorije = p.Naziv_Kategorije, Subcategory = _SubcategoryViewModels.Where(x => x.Fk_Kategorija == p.Id).ToList() });
            }

        }
        private async void ShowBtnPress(int id)
        {
            KGB_CategoryViewModel kgb = _CategoryViewModels.First(x => x.Id == id);
            kgb.ShowSubcategory = !kgb.ShowSubcategory;
        }
        public async Task OpenDialogForSubcategory(KGB_CategoryViewModel model)
        {
            var parameter = new DialogParameters { ["Category"] = model };
            MudDialog.Cancel();
            DialogService.Show<CreateSubcategoryDialog>("", parameter, dialogOptions);
        }
        public async Task OpenDialogForCategory()
        {
            MudDialog.Cancel();
            DialogService.Show<CreateCategoryDialog>("", dialogOptions);
        }
        void Submit()
        {
            IGetServices.NavigationManager("/Create");
            MudDialog.Close(DialogResult.Ok(true));
        }
        void Cancel()
        {
            IGetServices.NavigationManager("/Create");
            MudDialog.Cancel();
        }
    }
}
