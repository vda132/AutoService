using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class AutoPartPrice
    {
        public int IdautoPart { get; set; }
        public DateTime DateChange { get; set; }
        public decimal PriceWithoutRepair { get; set; }

        public virtual AutoPart IdautoPartNavigation { get; set; }
    }
}
