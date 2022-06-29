namespace KGB_Dev_.Data.KGB_Model
{
    public class KGB_CategoryViewModel
    {
        public int Id { get; set; }
        public string? Naziv_Kategorije { get; set; }
        public bool ShowSubcategory { get; set; } = false;

        public IList<KGB_SubcategoryViewModel> Subcategory { get; set; }

    }
}
