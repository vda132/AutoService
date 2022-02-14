using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Exceptions
{
    class FieldException:Exception
    {
        public FieldException(string message) : base(message) { }
    }
}
