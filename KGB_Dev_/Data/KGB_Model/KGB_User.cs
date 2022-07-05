using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Dev_.Data.KGB_Model
{
    public class KGB_User : IdentityUser
    {
        public int Fk_Rola { get; set; }
        [Required(ErrorMessage = "Unesite ime!")]
        public string? Ime { get; set; }
        [Required(ErrorMessage = "Unesite email!")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Izaberite tip naloga!")]
        public string? Naziv_Role { get; set; }
        [Required(ErrorMessage = "Unesite prezime!")]
        public string? Prezime { get; set; }
        public int Sifra_Oj { get; set; }
        [Required(ErrorMessage = "Izaberite naziv organizacione jedinice!")]
        public string? Naziv_Oj { get; set; }
        public string? Lozinka { get; set; }
        public bool Active { get; set; } = true;
        public int K_Ins { get; set; }
        public string? D_Ins { get; set; }
        public int K_Upd { get; set; }
        public string? D_Upd { get; set; } = DateTime.Now.ToString();
    }
}
