using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class AutoServiceAutoPart
    {
        public int IdautoService { get; set; }
        public int IdautoPart { get; set; }

        public virtual AutoPart IdautoPartNavigation { get; set; }
        public virtual AutoService IdautoServiceNavigation { get; set; }
    }
}
