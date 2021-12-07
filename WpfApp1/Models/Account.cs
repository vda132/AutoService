using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Account
    {
        public int Idworker { get; set; }
        public string LoginAccount { get; set; }
        public string PasswordAccount { get; set; }
        public virtual Worker IdworkerNavigation { get; set; }
    }
}
