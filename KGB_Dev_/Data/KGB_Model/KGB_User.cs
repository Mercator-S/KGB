using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    [Table("KGB_UsersDb")]
    public class KGB_User
    {
        [Key]
        public int Id { get; set; }
        public int Fk_Rola { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public int Sifra_Oj { get; set; }
        public string? Naziv_Oj { get; set; }
        public string? Lozinka { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; } = true;
        public int K_Ins { get; set; }
        public string? D_Ins { get; set; } = DateTime.Now.ToString();
        public int K_Upd { get; set; }
        public string? D_Upd { get; set; }
    }
}
