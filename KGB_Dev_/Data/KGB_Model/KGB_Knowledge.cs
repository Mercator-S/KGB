using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_Knowledge")]
    public class KGB_Knowledge
    {
        [Key]
        public long Id { get; set; }
        public string? Naziv_Oj { get; set; }
        public int Sifra_Oj { get; set; }
        [Range(1, 100000, ErrorMessage = "Izaberite kategoriju prijave!")]
        public int Fk_Category { get; set; }
        [Range(1, 100000, ErrorMessage = "Izaberite potkategoriju prijave!")]
        public int Fk_Subcategory { get; set; }
        public string? Sifra_Prijave { get; set; }
        [Required(ErrorMessage = "Unesite naziv prijave!")]
        public string? Naziv_Prijave { get; set; }
        [Required(ErrorMessage = "Unesite opis prijave!")]
        public string? Opis_Prijave { get; set; }
        public string? Putanja_Fajl { get; set; }
        public bool Active { get; set; } = true;
        public DateTime d_upd { get; set; } = DateTime.Now;
        public string? k_ins { get; set; }
        public string? k_name { get; set; }
        public DateTime d_ins { get; set; }
        public string? k_upd { get; set; }
        public bool Visibility { get; set; } = false;
    }
}
