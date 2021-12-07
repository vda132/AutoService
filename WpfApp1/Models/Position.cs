using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Position
    {
        public Position()
        {
            Workers = new HashSet<Worker>();
        }

        public int Idposition { get; set; }
        public string NamePosition { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
