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
        public string? SearchCategory;
        public string? SearchSubcategory;
        private List<KGB_Category> _categories;
        private List<KGB_Subcategory> _subCategory;
        private bool VisibilitySubCatBtn = true;
        private bool VisibilityCatBtn = true;
        private bool VisibilityCat = false;
        private List<string?> Category;
        private List<string?> Subcategory;

        protected override async Task OnInitializedAsync()
        {
            var User = IServices.GetCurrentUser().Result;
            _categories = await IServices.GetCategory(User.Result.Sifra_Oj);
            _subCategory = await IServices.GetSubcategory(_categories.Select(x => x.Id).First());
            Category = _categories.Select(x => x.Naziv_Kategorije).ToList();
            Subcategory = _subCategory.Select(x => x.Naziv_Potkategorije).ToList();
        }
        private async Task<IEnumerable<string?>> SearchCategories(string value)
        {
            await Task.Delay(5);
            if (string.IsNullOrEmpty(value))
            {
                VisibilityCatBtn = true;
                SearchCategory = null;
                return Category;
            }
            else if (Category.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
            {
                SearchCategory = value;
                VisibilityCatBtn = false;
                return Category.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            }
            VisibilityCatBtn = true;
            return Category.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        private async Task<IEnumerable<string?>> SearchSubCategories(string value)
        {
            await Task.Delay(5);
            if (string.IsNullOrEmpty(value))
            {
                VisibilitySubCatBtn = true;
                SearchSubcategory = null;
                return Subcategory;
            }
            else if (Subcategory.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
            {
                SearchSubcategory = value;
                VisibilitySubCatBtn = false;
                return Subcategory.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            }
            VisibilitySubCatBtn = true;
            return Subcategory.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        void AddCategory()
        {
            VisibilityCat = true;
            VisibilityCatBtn = true;
        }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
