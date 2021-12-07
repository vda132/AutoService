using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class AutoPart
    {
        public AutoPart()
        {
            AutoPartPrices = new HashSet<AutoPartPrice>();
            AutoServiceAutoParts = new HashSet<AutoServiceAutoPart>();
            Compatibilities = new HashSet<Compatibility>();
        }

        public int IdautoPart { get; set; }
        public string NameAutoPart { get; set; }
        public int Idcountry { get; set; }

        public virtual Country IdcountryNavigation { get; set; }
        public virtual ICollection<AutoPartPrice> AutoPartPrices { get; set; }
        public virtual ICollection<AutoServiceAutoPart> AutoServiceAutoParts { get; set; }
        public virtual ICollection<Compatibility> Compatibilities { get; set; }
    }
}
