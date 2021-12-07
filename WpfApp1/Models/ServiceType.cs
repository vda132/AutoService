using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            AutoServices = new HashSet<AutoService>();
        }

        public int IdserviceType { get; set; }
        public string NameServiceType { get; set; }
        public decimal PriceServiceType { get; set; }

        public virtual ICollection<AutoService> AutoServices { get; set; }
    }
}
