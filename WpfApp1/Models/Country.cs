using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Country
    {
        public Country()
        {
            AutoConcerns = new HashSet<AutoConcern>();
            AutoParts = new HashSet<AutoPart>();
        }

        public int Idcountry { get; set; }
        public string NameCountry { get; set; }

        public virtual ICollection<AutoConcern> AutoConcerns { get; set; }
        public virtual ICollection<AutoPart> AutoParts { get; set; }
    }
}
