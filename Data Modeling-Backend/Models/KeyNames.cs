using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public static class KeyNames
    {
        #region AreaNames
        public const string area_oracle = "oracle";
        public const string area_sqlserver = "sqlserver";
        
        #endregion



        #region Oracle
        public static string oracle_Con_Host = "Oracle.Con.Host";
        public static string oracle_Con_Port = "Oracle.Con.Port";
        public static string oracle_Con_ServiceName = "Oracle.Con.ServiceName";
        public static string oracle_Con_UserId = "Oracle.Con.UserId";
        public static string oracle_Con_Password = "Oracle.Con.Password";
        public static string oracle_Con_InstanceName = "Oracle.Con.InstanceName";
        public static string oracle_Con_Template = "Oracle.Con.Template";

        #endregion

        #region SQLSERVER

        public static string sqlserver_Con_Host = "Sqlserver.Con.Host";
        public static string sqlserver_Con_Port = "Sqlserver.Con.Port";
        public static string sqlserver_Con_UserId = "Sqlserver.Con.UserId";
        public static string sqlserver_Con_Password = "Sqlserver.Con.Password";
        public static string sqlserver_Con_DbName = "Sqlserver.Con.DbName";
        #endregion

    }
}
