using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KGB_Dev_.Pages.Dialog
{
    partial class CategoryDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        [Inject]
        public IKgbServices IServices { get; set; } = default!;
        private IEnumerable<KGB_Category> _categories;
        private IEnumerable<KGB_Subcategory> _subCategory;
        private IList<KGB_CategoryViewModel> _CategoryViewModels;
        private IList<KGB_SubcategoryViewModel> _SubcategoryViewModels;
        bool Form = false;
        KGB_SubcategoryViewModel SubcategoryModel = new KGB_SubcategoryViewModel();
        public bool ShowSubcategory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var User = IServices.GetCurrentUser().Result;
            _CategoryViewModels = new List<KGB_CategoryViewModel>();
            _SubcategoryViewModels = new List<KGB_SubcategoryViewModel>();
            _categories = await IServices.GetCategory(User.Result.Sifra_Oj);
            _subCategory = await IServices.GetSubcategory();
            foreach (var l in _subCategory)
            {
                _SubcategoryViewModels.Add(new KGB_SubcategoryViewModel { Id = l.Id, Naziv_Potkategorije = l.Naziv_Potkategorije, Fk_Kategorija = l.Fk_Kategorija });
            }
            foreach (var p in _categories)
            {
                _CategoryViewModels.Add(new KGB_CategoryViewModel { Id = p.Id, Naziv_Kategorije = p.Naziv_Kategorije, Subcategory = _SubcategoryViewModels.Where(x => x.Fk_Kategorija == p.Id).ToList() }) ;
            }
            
        }
        private async void ShowBtnPress(int id)
        {
            KGB_CategoryViewModel kgb = _CategoryViewModels.First(x => x.Id == id);
            kgb.ShowSubcategory = !kgb.ShowSubcategory;
        }
        private async Task CreateSubcategory(KGB_SubcategoryViewModel Model)
        {
            //Implement in Iservices method from create subcategory
            await IServices.GetCategory(1);
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
        void ShowForm() => Form = true;

    }
}
