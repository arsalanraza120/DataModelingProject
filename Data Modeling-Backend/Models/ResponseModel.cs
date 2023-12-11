using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ResponseModel
    {
        public string? token { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public bool? IsSuccess { get; set; }
        public object Data { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }


}
