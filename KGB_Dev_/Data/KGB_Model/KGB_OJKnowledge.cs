using System.ComponentModel.DataAnnotations;

namespace KGB_Dev_.Data.KGB_Model
{
    public class KGB_OJKnowledge
    {
        [Key]
        public int Id { get; set; }
        public long IdPrijave { get; set; }
        public int Sifra_Oj { get; set; }
    }
}
