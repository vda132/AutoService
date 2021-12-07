using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Car
    {
        public Car()
        {
            AutoServices = new HashSet<AutoService>();
        }

        public string StateNumber { get; set; }
        public int Idmodel { get; set; }
        public DateTime DataOfRelease { get; set; }
        public int Idclient { get; set; }

        public virtual Client IdclientNavigation { get; set; }
        public virtual Model IdmodelNavigation { get; set; }
        public virtual ICollection<AutoService> AutoServices { get; set; }
    }
}
