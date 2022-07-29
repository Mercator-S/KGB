using System.ComponentModel.DataAnnotations;

namespace KGB_Models.KGB_Model
{
    public class KGB_SubcategoryViewModel
    {
        [Key]
        public int Id { get; set; }
        public int Fk_Kategorija { get; set; }
        [Required(ErrorMessage = "Unesite naziv potkategorije!")]
        public string? Naziv_Potkategorije { get; set; }
    }
}
