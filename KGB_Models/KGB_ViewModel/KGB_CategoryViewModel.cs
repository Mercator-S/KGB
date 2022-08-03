using System.ComponentModel.DataAnnotations;

namespace KGB_Models.KGB_Model
{
    public class KGB_CategoryViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Unesite naziv kategorije!")]
        public string? Naziv_Kategorije { get; set; }
        public bool ShowSubcategory { get; set; } = false;

        public IList<KGB_SubcategoryViewModel> Subcategory { get; set; }

    }
}
