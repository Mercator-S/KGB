using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_Subcategory")]
    public class KGB_Subcategory
    {
        [Key]
        public int Id { get; set; }
        public int Fk_Kategorija { get; set; }
        public string? Naziv_Potkategorije { get; set; }
        public DateTime d_upd { get; set; } = DateTime.Now;
        public DateTime d_ins { get; set; }
        public string? k_ins { get; set; }
        public string? k_upd { get; set; }
        public bool Active { get; set; } = true;

    }
}
