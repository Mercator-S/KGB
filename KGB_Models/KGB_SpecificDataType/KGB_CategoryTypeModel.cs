using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGB_Domain.KGB_SpecificDataType
{
    public class KGB_CategoryTypeModel
    {
        public int SifraKategorije { get; set; }
        public string NazivKategorije { get; set; }
        public KGB_CategoryTypeModel(int sifraKategorije, string nazivKategorije)
        {
            SifraKategorije = sifraKategorije;
            NazivKategorije = nazivKategorije;
        }
    }
}
