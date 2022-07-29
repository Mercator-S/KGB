using System.ComponentModel.DataAnnotations;

namespace KGB_Models.KGB_Model
{
    public class KGB_OJKnowledge
    {
        [Key]
        public int Id { get; set; }
        public long IdPrijave { get; set; }
        public int Sifra_Oj { get; set; }
    }
}
