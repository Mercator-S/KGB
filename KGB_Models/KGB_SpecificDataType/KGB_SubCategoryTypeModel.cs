using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGB_Domain.KGB_SpecificDataType
{
    public class KGB_SubCategoryTypeModel
    {
        public int SifraPotkategorije { get; set; }
        public string NazivPotkategorije { get; set; }
        public KGB_SubCategoryTypeModel(int sifraPotkategorije, string nazivPotkategorije)
        {
            SifraPotkategorije = sifraPotkategorije;
            NazivPotkategorije = nazivPotkategorije;
        }
    }
}
