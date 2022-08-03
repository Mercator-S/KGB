using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGB_Models.KGB_Model
{
    [Table("KGB_Oj")]
    public class KGB_Oj
    {
        [Key]
        public int Id { get; set; }
        public int SifraOj { get; set; }
        public string? NazivOj { get; set; }
        public bool Active { get; set; }
        public DateTime DUpd { get; set; }
        public int KIns { get; set; }
        public DateTime DIns { get; set; }
        public int KUpd { get; set; }
    }
}
