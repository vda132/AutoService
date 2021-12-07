using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class CarBrand
    {
        public CarBrand()
        {
            Models = new HashSet<Model>();
        }

        public int IdcarBrand { get; set; }
        public string NameCarBrand { get; set; }
        public int IdautoConcern { get; set; }

        public virtual AutoConcern IdautoConcernNavigation { get; set; }
        public virtual ICollection<Model> Models { get; set; }
    }
}
