using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
            Compatibilities = new HashSet<Compatibility>();
        }

        public int Idmodel { get; set; }
        public string NameModel { get; set; }
        public int IdcarBrand { get; set; }

        public virtual CarBrand IdcarBrandNavigation { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Compatibility> Compatibilities { get; set; }
    }
}
