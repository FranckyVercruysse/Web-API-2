using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpionshopWPFApp.Models
{
    class BestellingPOCO
    {

        public int B_id { get; set; }
        public short Klant_id { get; set; }
        public DateTime Datum { get; set; }

        public ICollection<BestellingDetailPOCO> Bestellingen { get; set; }
    }
}