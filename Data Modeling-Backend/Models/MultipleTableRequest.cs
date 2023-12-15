using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MultipleTableRequest
    {
        public List<string> TableNames { get; set; }
        public ConnectionDb Conn { get; set; }
    }
}
