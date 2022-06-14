using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_Role")]
    public class KGB_Role
    {
        [Key]
        public int Id { get; set; }
        public int Sifra_Role { get; set; }
        public string Naziv_Role { get; set; }
    }
}
