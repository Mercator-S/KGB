using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGB_Domain.KGB_SpecificDataType
{
    public class KGB_OrgJed
    {
        public int SifraOj { get; set; }
        public string NazivOj { get; set; }
        public KGB_OrgJed(int sifraOj, string nazivOj)
        {
            SifraOj = sifraOj;
            NazivOj = nazivOj;
        }
    }
}
