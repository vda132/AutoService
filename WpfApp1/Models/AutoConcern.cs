using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class AutoConcern
    {
        public AutoConcern()
        {
            CarBrands = new HashSet<CarBrand>();
        }

        public int IdautoConcern { get; set; }
        public string NameAutoConcern { get; set; }
        public int Idcountry { get; set; }

        public virtual Country IdcountryNavigation { get; set; }
        public virtual ICollection<CarBrand> CarBrands { get; set; }
    }
}
