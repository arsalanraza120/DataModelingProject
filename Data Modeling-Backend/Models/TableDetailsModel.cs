using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{

    #region OldCode
    //public class DatabaseModel
    //{
    //    public Dictionary<string, TableModel> Tables { get; set; }
    //}

    //public class TableColumnModel
    //{
    //    public string ColumnName { get; set; }
    //    public string DataType { get; set; }
    //    public string Size { get; set; }
    //    public bool AllowNull { get; set; }
    //}

    //public class ForeignKeyModel
    //{
    //    public string ConstraintName { get; set; }
    //    public string ColumnName { get; set; }
    //    public string ReferencedTable { get; set; }
    //    public string ReferencedConstraint { get; set; }
    //}

    //public class TableModel
    //{
    //    public string TableName { get; set; }
    //    public List<TableColumnModel> Columns { get; set; }
    //    public List<ForeignKeyModel> ForeignKeys { get; set; }
    //    public List<string> PrimaryKeys { get; set; }
    //}
    #endregion

    public class TableColumnModel
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string Size { get; set; }
        public bool AllowNull { get; set; }
    }

    public class ForeignKeyModel
    {
        public string ConstraintName { get; set; }
        public string ForeignKeyColumnName { get; set; }
        public string ReferencedTableName { get; set; }
        public string ReferencedConstraintName { get; set; }
    }

    public class PrimaryKeyModel
    {
        public string ColumnName { get; set; }
    }

    public class TableModel
    {
        public List<TableColumnModel> Columns { get; set; }
        public List<ForeignKeyModel> ForeignKeys { get; set; }
        public List<PrimaryKeyModel> PrimaryKeys { get; set; }
    }

    public class DatabaseModel
    {
        public string TableName { get; set; }
        public Dictionary<string, TableModel> Tables { get; set; }
        
    }




}
