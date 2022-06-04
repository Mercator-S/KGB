using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_Knowledge")]
    public class KGB_Knowledge
    {
        [Key]
        public long Id { get; set; }
        public int Sifra_Oj { get; set; }
        public int Fk_Category { get; set; }
        public int Fk_Subcategory { get; set; }
        public string? Sifra_Prijave { get; set; }
        public string? Naziv_Prijave { get; set; }
        public string? Opis_Prijave { get; set; }
        public string? Putanja_Fajl { get; set; }
        public bool Active { get; set; }
        public DateTime d_upd { get; set; }
        public int k_ins { get; set; }
        public DateTime d_ins { get; set; }
        public int k_upd { get; set; }
    }
}
