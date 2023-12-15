using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TableData
    {
        public string TableName { get; set; }
        public List<ColumnData> SelectedRows { get; set; }
    }

    public class ColumnData
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool AllowNull { get; set; }
        public string? Size { get; set; }
    }
}
