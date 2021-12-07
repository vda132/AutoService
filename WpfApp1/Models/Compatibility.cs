using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Compatibility
    {
        public int Idmodel { get; set; }
        public int IdautoPart { get; set; }

        public virtual AutoPart IdautoPartNavigation { get; set; }
        public virtual Model IdmodelNavigation { get; set; }
    }
}
