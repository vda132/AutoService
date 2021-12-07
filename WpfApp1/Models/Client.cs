using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Client
    {
        public Client()
        {
            Cars = new HashSet<Car>();
        }

        public int Idclient { get; set; }
        public string NameClient { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
