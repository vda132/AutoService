using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class AutoService
    {
        public AutoService()
        {
            AutoServiceAutoParts = new HashSet<AutoServiceAutoPart>();
        }

        public int IdautoService { get; set; }
        public string StateNumber { get; set; }
        public int? Idworker { get; set; }
        public DateTime DateAutoService { get; set; }
        public int IdserviceType { get; set; }
        public decimal Price { get; set; }

        public string NameWorker => IdworkerNavigation == null ? "Данный сотрудник был уволен." : IdworkerNavigation.NameWorker;

        public virtual ServiceType IdserviceTypeNavigation { get; set; }
        public virtual Worker IdworkerNavigation { get; set; }
        public virtual Car StateNumberNavigation { get; set; }
        public virtual ICollection<AutoServiceAutoPart> AutoServiceAutoParts { get; set; }
    }
}
