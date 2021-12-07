using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Worker
    {
        public Worker()
        {
            AutoServices = new HashSet<AutoService>();
        }

        public int Idworker { get; set; }
        public string NameWorker { get; set; }
        public int Idposition { get; set; }

        public virtual Position IdpositionNavigation { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<AutoService> AutoServices { get; set; }
    }
}
