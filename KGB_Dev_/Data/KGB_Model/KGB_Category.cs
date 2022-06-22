using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_Category")]
    public class KGB_Category
    {
        [Key]
        public int Id { get; set; }
        public int Sifra_Potkategorije { get; set; }
        public string? Naziv_Potkategorije { get; set; }
        public int Sifra_Kategorije { get; set; }
        public string? Naziv_Kategorije { get; set; }
        public DateTime d_upd { get; set; } = DateTime.Now;
        public DateTime d_ins { get; set; }
        public string k_ins { get; set; }
        public string k_upd { get; set; }
        public bool Active { get; set; } = true;

    }
}