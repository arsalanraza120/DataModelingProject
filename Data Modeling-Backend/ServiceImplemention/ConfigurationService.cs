using DataBaseContext;
using Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Models;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SQLite;


namespace ServiceImplemention
{
    public class ConfigurationService : IConfigurationService
    {
        DBContext _contxt;
        ResponseModel _rresponsewrapper;
        private readonly AppSettings _appSettings;

        public ConfigurationService(DBContext context, IOptions<AppSettings> appSettings, ResponseModel rresponsewrapper)
        {
            _contxt = context;
            _appSettings = appSettings.Value;
            _rresponsewrapper = rresponsewrapper;
        }

        public enum DatabaseType
        {
            Oracle,
            SQLite,
            SQLServer,
            PostgreSQL,
            MySQL,
            MongoDb,
            MariaDB,
            IBM2
        }




        public async Task<string> VerifyConnection(string dbType, ConnectionDb Con)
        {
            string retVal = false.ToString();
            IDbConnection connection = null;

            try
            {
                string connectionString = await GetConnectionString(Con);
                if (Enum.TryParse(dbType, out DatabaseType selectedDbType))
                {
                    switch (selectedDbType)
                    {
                        case DatabaseType.Oracle:
                            connection = new OracleConnection(connectionString);
                            break;
                        case DatabaseType.SQLite:
                            connection = new SQLiteConnection(connectionString);
                            break;
                        case DatabaseType.SQLServer:
                            connection = new SqlConnection(connectionString);
                            break;
                        case DatabaseType.PostgreSQL:
                            connection = new NpgsqlConnection(connectionString);
                            break;
                        //case DatabaseType.MongoDb:
                        //    connection = new MongoConnection(connectionString);
                        default:

                            throw new NotSupportedException("Unsupported database type");
                    }

                    connection.Open();
                    retVal = true.ToString();
                }
                else
                {
                    throw new NotSupportedException("Invalid database type");
                }
            }
            catch (Exception exp)
            {
                throw;
            }
            finally
            {
                connection?.Dispose();
            }

            return retVal;
        }



        public ResponseModel GetCredentialsByCredentialName()
        {
            try
            {
                var credentialsGrouped = _contxt.ConfigCredentials.Where(x => x.IsActive == true)
                    .GroupBy(c => c.CredentialName)
                    .Select(g => new
                    {
                        CredentialName = g.Key,
                        Count = g.Count(),
                        Credentials = g.ToList()
                    })
                    .ToList();

                int totalCount = credentialsGrouped.Count;

                _rresponsewrapper.IsSuccess = true;
                _rresponsewrapper.Data = new
                {
                    TotalCount = totalCount,
                    GroupedCredentials = credentialsGrouped
                };
            }
            catch (Exception ex)
            {
                _rresponsewrapper.IsSuccess = false;
                _rresponsewrapper.Message = "An error occurred while fetching the grouped credentials.";
                _rresponsewrapper.Data = ex;
            }

            return _rresponsewrapper;
        }

        public string GenerateSQLServerConnectionTmp(ConnectionDb connectionDb)
        {
            if (connectionDb != null)
            {
                if (connectionDb.DbName != null && connectionDb.Host != null && connectionDb.Port != null && connectionDb.UserId != null && connectionDb.Password != null)
                {
                    string sqlServerTemp = "Server=@@@server,@@@port; Database=@@@dbname; User ID=@@@userid; Password=@@@password;";

                    string conString = sqlServerTemp
                            .Replace("@@@server", connectionDb.Host ?? "")
                            .Replace("@@@port", connectionDb.Port ?? "")
                            .Replace("@@@userid", connectionDb.UserId ?? "")
                            .Replace("@@@password", connectionDb.Password ?? "")
                            .Replace("@@@dbname", connectionDb.DbName ?? "");
                    return conString;
                }
            }
            return null;
        }

        public string GenerateOracleConnectionTmp(ConnectionDb connectionDb)
        {
            if (connectionDb != null)
            {
                string Oracletemp = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=@@@host)(PORT=@@@port))(CONNECT_DATA=(SERVICE_NAME=@@@servicename)(INSTANCE_NAME=@@@instancename))); User ID = @@@userid; Password = @@@password;";
                if (connectionDb.Host != null || connectionDb.Port != null || connectionDb.ServiceName != null || connectionDb.InstanceName != null || connectionDb.UserId != null || connectionDb.Password != null)
                {
                    string conString = Oracletemp.Replace("@@@host", connectionDb.Host)
                        .Replace("@@@port", connectionDb.Port)
                        .Replace("@@@servicename", connectionDb.ServiceName)
                        .Replace("@@@instancename", connectionDb.InstanceName)
                        .Replace("@@@userid", connectionDb.UserId)
                        .Replace("@@@password", connectionDb.Password)
                        .ToString();

                    return conString;
                }

            }

            return null;
        }

        public async Task<string> GetConnectionString(ConnectionDb conObj)
        {
            string host = conObj.Host;
            string port = conObj.Port;
            string database = conObj.DbName;
            string userid = conObj.UserId;
            string password = conObj.Password;
            string servicename = conObj.ServiceName;
            string instancename = conObj.InstanceName;

            //SQLSERVER
            if (DatabaseType.SQLServer.ToString() == conObj.dbType)
            {
                string sqlServerTemp = "Server=@@@server,@@@port; Database=@@@database; User ID=@@@userid; Password=@@@password;";
                if (host != null && database != null && userid != null && password != null && port != null)
                {
                    string conString = sqlServerTemp
                        .Replace("@@@server", host)
                        .Replace("@@@database", database)
                        .Replace("@@@userid", userid)
                        .Replace("@@@password", password)
                        .Replace("@@@port", port);
                    return conString;
                }


            }
            //SQLLITE
            else if (DatabaseType.SQLite.ToString() == conObj.dbType)
            {
                string sqlitetemp = "Data Source=@database.db; Version=3; Password=@password;";
                if (database != null && password != null)
                {
                    string conString = sqlitetemp
                        .Replace("@database", database)
                        .Replace("@password", password);
                    return conString;
                }
            }
            //ORACLE
            else if (DatabaseType.Oracle.ToString() == conObj.dbType)
            {
                string Oracletemp = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=@@@host)(PORT=@@@port))(CONNECT_DATA=(SERVICE_NAME=@@@servicename)(INSTANCE_NAME=@@@instancename))); User ID = @@@userid; Password = @@@password;";
                if (host != null && port != null && servicename != null && instancename != null && userid != null && password != null)
                {
                    string conString = Oracletemp.Replace("@@@host", host)
                        .Replace("@@@port", port)
                        .Replace("@@@servicename", servicename)
                        .Replace("@@@instancename", instancename)
                        .Replace("@@@userid", userid)
                        .Replace("@@@password", password)
                        .ToString();

                    return conString;
                }
            }
            //PostgreSQL
            else if (DatabaseType.PostgreSQL.ToString() == conObj.dbType)
            {
                string postgresTemp = "Host=@host; Port=@port; Database=@database; User Id=@userid; Password=@password;";
                if (host != null && port != null && database != null && userid != null && password != null)
                {
                    string conString = postgresTemp
                        .Replace("@host", host)
                        .Replace("@port", port)
                        .Replace("@database", database)
                        .Replace("@userid", userid)
                        .Replace("@password", password);
                    return conString;
                }
            }
            //MySQL
            else if (DatabaseType.MySQL.ToString() == conObj.dbType)
            {
                string mySQLTemp = "Server=@@@server; Port=@@@port; Database=@@@database; User=@@@user; Password=@@@password;";
                if (host != null && port != null && database != null && userid != null && password != null)
                {
                    string conString = mySQLTemp.Replace("@@@server", host)
                        .Replace("@@@port", port)
                        .Replace("@@@database", database)
                        .Replace("@@@user", userid)
                        .Replace("@@@password", password)
                        .ToString();

                    return conString;
                }
            }
            //MangoDB
            else if (DatabaseType.MongoDb.ToString() == conObj.dbType)
            {
                string mongoDBTemp = "mongodb://@@@username:@@@password@@@host/@@@database";

                if (userid != null && password != null && host != null && database != null)
                {
                    string conString = mongoDBTemp.Replace("@@@username", userid)
                        .Replace("@@@password", password)
                        .Replace("@@@host", host)
                        .Replace("@@@database", database)
                        .ToString();

                    return conString;
                }
            }
            //MariaDB
            else if (DatabaseType.MariaDB.ToString() == conObj.dbType)
            {
                string mariaDBTemp = "Server=@@@server; Port=@@@port; Database=@@@database; User=@@@user; Password=@@@password;";

                if (host != null && port != null && database != null && userid != null && password != null)
                {
                    string conString = mariaDBTemp.Replace("@@@server", host)
                        .Replace("@@@port", port)
                        .Replace("@@@database", database)
                        .Replace("@@@user", userid)
                        .Replace("@@@password", password)
                        .ToString();

                    return conString;
                }
            }
            //IBM2
            else if (DatabaseType.IBM2.ToString() == conObj.dbType)
            {

                string IBMTemp = "Server=@@@server; Database=@@@database; UID=@@@user; PWD=@@@password;";

                if (host != null && database != null && userid != null && password != null)
                {
                    string conString = IBMTemp.Replace("@@@server", host)
                        .Replace("@@@database", database)
                        .Replace("@@@user", userid)
                        .Replace("@@@password", password)
                        .ToString();

                    return conString;
                }
            }

            return null;
        }

        public async Task<ResponseModel> SaveConnStringParams(ConnectionDb Conn)
        {
            try
            {
                if (await VerifyConnection(Conn.dbType, Conn) == true.ToString())
                {
                    #region SQLSERVERSAVE   
                    if (DatabaseType.SQLServer.ToString() == Conn.dbType)
                    {

                        var existingCredentials = _contxt.ConfigCredentials.Where(c => c.CredentialName == Conn.CredentialName).FirstOrDefault();


                        if (existingCredentials == null)
                        {
                            existingCredentials = new ConfigCredential { CredentialName = Conn.CredentialName, CreatedDate = DateTime.Now };
                            _contxt.ConfigCredentials.Add(existingCredentials);

                            _rresponsewrapper.Message = "Connection Save Successfully";
                            _rresponsewrapper.IsSuccess = true;
                        }
                        else
                        {
                            _rresponsewrapper.Message = "Credentials with this name already exist in the database " + Conn.CredentialName;
                            _rresponsewrapper.IsSuccess = true;
                        }


                        existingCredentials.Host = Conn.Host;
                        existingCredentials.Port = Conn.Port;
                        existingCredentials.UserId = Conn.UserId;
                        existingCredentials.Password = Conn.Password;
                        existingCredentials.DbName = Conn.DbName;
                        existingCredentials.DbType = Conn.dbType;
                        existingCredentials.ModifiedDate = DateTime.Now;
                        existingCredentials.IsActive = true;
                    }

                    #endregion

                    #region OracleSAVE   
                    if (DatabaseType.Oracle.ToString() == Conn.dbType)
                    {
                        var existingCredentials = _contxt.ConfigCredentials.Where(c => c.CredentialName == Conn.CredentialName).FirstOrDefault();

                        if (existingCredentials == null)
                        {
                            existingCredentials = new ConfigCredential { CredentialName = Conn.CredentialName, CreatedDate = DateTime.Now };
                            _contxt.ConfigCredentials.Add(existingCredentials);

                            _rresponsewrapper.Message = "Connection Save Successfully";
                            _rresponsewrapper.IsSuccess = true;
                        }
                        else
                        {
                            _rresponsewrapper.Message = "Credentials with this name already exist in the database" + Conn.CredentialName;
                            _rresponsewrapper.IsSuccess = true;
                        }


                        existingCredentials.Host = Conn.Host;
                        existingCredentials.Port = Conn.Port;
                        existingCredentials.UserId = Conn.UserId;
                        existingCredentials.Password = Conn.Password;
                        existingCredentials.DbName = Conn.DbName;
                        existingCredentials.DbType = Conn.dbType;
                        existingCredentials.InstanceName = Conn.InstanceName;
                        existingCredentials.InstanceName = Conn.InstanceName;
                        existingCredentials.ModifiedDate = DateTime.Now;
                        existingCredentials.IsActive = true;
                    }
                    #endregion

                    _contxt.SaveChanges();
                }
                else
                {
                    _rresponsewrapper.Message = "Connection: Access Denied!";
                    _rresponsewrapper.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.Message = "Error in saving connection Please Check.... ";
                _rresponsewrapper.IsSuccess = false;
            }
            return _rresponsewrapper;
        }

        public Dictionary<string, DatabaseModel> GetMultipleTablesMetadata(List<string> tableNames, string Conn)
        {
            Dictionary<string, DatabaseModel> tablesMetadata = new Dictionary<string, DatabaseModel>();

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                connection.Open();

                foreach (string tableName in tableNames)
                {
                    DatabaseModel databaseModel = new DatabaseModel
                    {
                        TableName = tableName,
                        Tables = new Dictionary<string, TableModel>()
                    };

                    TableModel tableModel = new TableModel
                    {
                        Columns = new List<TableColumnModel>(),
                        ForeignKeys = new List<ForeignKeyModel>(),
                        PrimaryKeys = new List<PrimaryKeyModel>()
                    };

              // Fetch column information
                string columnQuery = @"
                SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName";

                    using (SqlCommand columnCommand = new SqlCommand(columnQuery, connection))
                    {
                        columnCommand.Parameters.AddWithValue("@TableName", tableName);

                        using (SqlDataReader columnReader = columnCommand.ExecuteReader())
                        {
                            while (columnReader.Read())
                            {
                                string columnName = columnReader["COLUMN_NAME"].ToString();
                                string dataType = columnReader["DATA_TYPE"].ToString();
                                string size = columnReader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                                bool allowNull = columnReader["IS_NULLABLE"].ToString().ToLower() == "yes";

                                tableModel.Columns.Add(new TableColumnModel
                                {
                                    ColumnName = columnName,
                                    DataType = dataType,
                                    Size = size,
                                    AllowNull = allowNull,
                                });
                            }
                        }
                    }


                    // Fetch foreign key information
                    string foreignKeyQuery = @"
                SELECT
                    TC.CONSTRAINT_NAME,
                    KCU.COLUMN_NAME,
                    KCU.TABLE_NAME AS REFERENCED_TABLE,
                    RC.CONSTRAINT_NAME AS REFERENCED_CONSTRAINT
                FROM
                    INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                    INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE KCU ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME
                    LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ON TC.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME
                WHERE
                    TC.TABLE_NAME = @TableName
                    AND TC.CONSTRAINT_TYPE = 'FOREIGN KEY'";

                    using (SqlCommand foreignKeyCommand = new SqlCommand(foreignKeyQuery, connection))
                    {
                        foreignKeyCommand.Parameters.AddWithValue("@TableName", tableName);

                        using (SqlDataReader foreignKeyReader = foreignKeyCommand.ExecuteReader())
                        {
                            while (foreignKeyReader.Read())
                            {
                                string foreignKeyConstraintName = foreignKeyReader["CONSTRAINT_NAME"].ToString();
                                string foreignKeyColumnName = foreignKeyReader["COLUMN_NAME"].ToString();
                                string referencedTableName = foreignKeyReader["REFERENCED_TABLE"].ToString();
                                string referencedConstraintName = foreignKeyReader["REFERENCED_CONSTRAINT"].ToString();

                                tableModel.ForeignKeys.Add(new ForeignKeyModel
                                {
                                    ConstraintName = foreignKeyConstraintName,
                                    ForeignKeyColumnName = foreignKeyColumnName,
                                    ReferencedTableName = referencedTableName,
                                    ReferencedConstraintName = referencedConstraintName
                                });
                            }
                        }
                    }


                    // Fetch primary key information
                    string primaryKeyQuery = @"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON TC.CONSTRAINT_NAME = CCU.CONSTRAINT_NAME
                WHERE TC.TABLE_NAME = @TableName AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'";

                    using (SqlCommand primaryKeyCommand = new SqlCommand(primaryKeyQuery, connection))
                    {
                        primaryKeyCommand.Parameters.AddWithValue("@TableName", tableName);

                        using (SqlDataReader primaryKeyReader = primaryKeyCommand.ExecuteReader())
                        {
                            while (primaryKeyReader.Read())
                            {
                                string primaryColumnName = primaryKeyReader["COLUMN_NAME"].ToString();

                                tableModel.PrimaryKeys.Add(new PrimaryKeyModel
                                {
                                    ColumnName = primaryColumnName
                                });
                            }
                        }
                    }
                    databaseModel.Tables[tableName] = tableModel;


                    tablesMetadata[tableName] = databaseModel;
                }
            }

            return tablesMetadata;
        }

        public DatabaseModel GetMetaTableByName(string tableName, string Conn)
        {
            DatabaseModel databaseModel = new DatabaseModel
            {
                TableName = tableName,
                Tables = new Dictionary<string, TableModel>()
            };

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                connection.Open();

                TableModel tableModel = new TableModel
                {

                    Columns = new List<TableColumnModel>(),
                    ForeignKeys = new List<ForeignKeyModel>(),
                    PrimaryKeys = new List<PrimaryKeyModel>()
                };

                // Fetch column information
                string columnQuery = @"
            SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = @TableName";

                using (SqlCommand columnCommand = new SqlCommand(columnQuery, connection))
                {
                    columnCommand.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader columnReader = columnCommand.ExecuteReader())
                    {
                        while (columnReader.Read())
                        {
                            string columnName = columnReader["COLUMN_NAME"].ToString();
                            string dataType = columnReader["DATA_TYPE"].ToString();
                            string size = columnReader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                            bool allowNull = columnReader["IS_NULLABLE"].ToString().ToLower() == "yes";

                            tableModel.Columns.Add(new TableColumnModel
                            {
                                ColumnName = columnName,
                                DataType = dataType,
                                Size = size,
                                AllowNull = allowNull,

                            });
                        }
                    }
                }

                // Fetch foreign key information
                string foreignKeyQuery = @"
            SELECT
                TC.CONSTRAINT_NAME,
                KCU.COLUMN_NAME,
                KCU.TABLE_NAME AS REFERENCED_TABLE,
                RC.CONSTRAINT_NAME AS REFERENCED_CONSTRAINT
            FROM
                INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE KCU ON TC.CONSTRAINT_NAME = KCU.CONSTRAINT_NAME
                LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ON TC.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME
            WHERE
                TC.TABLE_NAME = @TableName
                AND TC.CONSTRAINT_TYPE = 'FOREIGN KEY'";

                using (SqlCommand foreignKeyCommand = new SqlCommand(foreignKeyQuery, connection))
                {
                    foreignKeyCommand.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader foreignKeyReader = foreignKeyCommand.ExecuteReader())
                    {
                        while (foreignKeyReader.Read())
                        {
                            string foreignKeyConstraintName = foreignKeyReader["CONSTRAINT_NAME"].ToString();
                            string foreignKeyColumnName = foreignKeyReader["COLUMN_NAME"].ToString();
                            string referencedTableName = foreignKeyReader["REFERENCED_TABLE"].ToString();
                            string referencedConstraintName = foreignKeyReader["REFERENCED_CONSTRAINT"].ToString();

                            tableModel.ForeignKeys.Add(new ForeignKeyModel
                            {
                                ConstraintName = foreignKeyConstraintName,
                                ForeignKeyColumnName = foreignKeyColumnName,
                                ReferencedTableName = referencedTableName,
                                ReferencedConstraintName = referencedConstraintName
                            });
                        }
                    }
                }

                // Fetch primary key information
                string primaryKeyQuery = @"
            SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
            JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU ON TC.CONSTRAINT_NAME = CCU.CONSTRAINT_NAME
            WHERE TC.TABLE_NAME = @TableName AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'";

                using (SqlCommand primaryKeyCommand = new SqlCommand(primaryKeyQuery, connection))
                {
                    primaryKeyCommand.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader primaryKeyReader = primaryKeyCommand.ExecuteReader())
                    {
                        while (primaryKeyReader.Read())
                        {
                            string primaryColumnName = primaryKeyReader["COLUMN_NAME"].ToString();

                            tableModel.PrimaryKeys.Add(new PrimaryKeyModel
                            {
                                ColumnName = primaryColumnName
                            });
                        }
                    }
                }

                databaseModel.Tables[tableName] = tableModel;
            }

            return databaseModel;
        }

        public ResponseModel GetCredentialById(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.Message = "Invalid  Id!";

                }
                else
                {
                    var configr = _contxt.ConfigCredentials.Where(x => x.IsActive == true && x.Id == Id).FirstOrDefault();
                    if (configr != null)
                    {
                        _rresponsewrapper.Data = configr;
                        _rresponsewrapper.IsSuccess = true;
                        _rresponsewrapper.Message = "Get Connection Success fully";

                    }
                    else
                    {
                        _rresponsewrapper.IsSuccess = false;
                        _rresponsewrapper.Message = "Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.Message = ex.Message;
                _rresponsewrapper.IsSuccess = false;

            }
            return _rresponsewrapper;
        }

        public ResponseModel RemoveConnectionById(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.Message = "Invalid  Id!";
                }
                else
                {
                    var configs = _contxt.ConfigCredentials.FirstOrDefault(a => a.Id == Id);
                    if (configs != null)
                    {
                        configs.IsActive = false;
                        _contxt.SaveChanges();
                        _rresponsewrapper.Data = configs;
                        _rresponsewrapper.IsSuccess = true;
                        _rresponsewrapper.Message = "Remove Connection Success fully";
                    }
                    else
                    {
                        _rresponsewrapper.IsSuccess = false;
                        _rresponsewrapper.Message = "Not Found";
                    }

                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.IsSuccess = false;
                _rresponsewrapper.Message = ex.Message;
            }
            return _rresponsewrapper;
        }

        public async Task<ResponseModel> GetTableNames(ConnectionDb Conn)
        {
            if (Conn.dbType == DatabaseType.SQLServer.ToString())
            {
                string SqlserverconString = GenerateSQLServerConnectionTmp(Conn);
                ResponseModel<List<string>> tableNamesResponse = GetTable(SqlserverconString);

                _rresponsewrapper.Data = tableNamesResponse;
                _rresponsewrapper.IsSuccess = true;

            }


            return _rresponsewrapper;
        }


        public ResponseModel<List<string>> GetTable(string connectionString)
        {
            ResponseModel<List<string>> responseWrapper = new ResponseModel<List<string>>();
            List<string> tableNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string tableNameQuery = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                    using (SqlCommand tableCommand = new SqlCommand(tableNameQuery, connection))
                    using (SqlDataReader tableReader = tableCommand.ExecuteReader())
                    {
                        if (tableReader.HasRows)
                        {
                            while (tableReader.Read())
                            {
                                string tableName = tableReader["TABLE_NAME"].ToString();
                                tableNames.Add(tableName);
                            }

                            responseWrapper.Data = tableNames;
                            responseWrapper.IsSuccess = true;
                            responseWrapper.Message = "Table names retrieved successfully.";
                        }
                        else
                        {
                            responseWrapper.IsSuccess = false;
                            responseWrapper.Message = "No tables found in the database.";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                responseWrapper.IsSuccess = false;
                responseWrapper.Message = $"SQL Exception: {ex.Message}";
            }
            catch (Exception ex)
            {
                responseWrapper.IsSuccess = false;
                responseWrapper.Message = $"Exception: {ex.Message}";
            }

            return responseWrapper;
        }


        public async Task<ResponseModel> GetMetaDataTableByName(string? tblName, ConnectionDb Conn)
        {
            if (Conn.dbType == DatabaseType.SQLServer.ToString())
            {
                string sqlserverCon = GenerateSQLServerConnectionTmp(Conn);
                DatabaseModel databaseModel = GetMetaTableByName(tblName, sqlserverCon);

                _rresponsewrapper.Data = databaseModel;
                _rresponsewrapper.IsSuccess = true;

            }

            return _rresponsewrapper;
        }


        public async Task<ResponseModel> GetMetaDataMultipleTableByName(List<string> tableNames, ConnectionDb Conn)
        {
            if (Conn.dbType == DatabaseType.SQLServer.ToString())
            {
                string sqlserverCon = GenerateSQLServerConnectionTmp(Conn);
                Dictionary<string, DatabaseModel> tablesMetadata = await Task.Run(() => GetMultipleTablesMetadata(tableNames, sqlserverCon));

                _rresponsewrapper.Data = tablesMetadata;
                _rresponsewrapper.IsSuccess = true;
            }

            return _rresponsewrapper;
        }



        public async Task<ResponseModel> GetAllTablesMetaData(ConnectionDb Conn)
        {
            if (Conn.dbType == DatabaseType.SQLServer.ToString())
            {
                string SqlserverconString = GenerateSQLServerConnectionTmp(Conn);
                //  DatabaseModel databaseModel = GetTableColumnDataTypes(SqlserverconString);

                _rresponsewrapper.Data = "";//databaseModel;
                _rresponsewrapper.IsSuccess = true;

            }
            else if (Conn.dbType == DatabaseType.Oracle.ToString())
            {
                string OracleConString = GenerateOracleConnectionTmp(Conn);
            }

            return _rresponsewrapper;
        }



        public async Task<ResponseModel> CreateTable(TableData tblData)
        {
            try
            {
                string connectionString = "server=172.17.1.11;database=Test;User ID=arsalan.raza;password=12345";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var tableName = tblData.TableName.Replace(" ", "_");

                    if (TableExists(connection, tableName))
                    {
                        AlterTable(connection, tableName, tblData.SelectedRows);
                    }
                    else
                    {
                        
                   var columns = tblData.SelectedRows.Select(column =>
                   $"{column.ColumnName} {column.DataType}" +
                   $"{(!string.IsNullOrEmpty(column.Size) && int.TryParse(column.Size, out int size) ? $"({size})" : "")} " +
                   $"{(column.AllowNull ? "NULL" : "NOT NULL")}");


                        var createTableQuery = $"CREATE TABLE {"["+tableName+"]"} ({string.Join(", ", columns)});";

                        using (SqlCommand createCommand = new SqlCommand(createTableQuery, connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }

                    }
                }

                var result = new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Table created or updated successfully",
                };

                return result;
            }
            catch (Exception ex)
            {
                var errorResult = new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Error creating or updating table: {ex.Message}",
                };

                return errorResult;
            }
        }

        private bool TableExists(SqlConnection connection, string tableName)
        {
            var checkTableQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
            {
                int count = (int)checkTableCommand.ExecuteScalar();
                return count > 0;
            }
        }


        private bool ColumnExists(SqlConnection connection, string tableName, string columnName)
        {
            
            var checkColumnQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' AND COLUMN_NAME = '{columnName}';";

            using (SqlCommand checkColumnCommand = new SqlCommand(checkColumnQuery, connection))
            {
                int count = (int)checkColumnCommand.ExecuteScalar();
                return count > 0;
            }
        }

        private void AlterTable(SqlConnection connection, string tableName, List<ColumnData> selectedRows)
        {
            
            var existingColumns = GetTableColumns(connection, tableName);

            foreach (var existingColumn in existingColumns)
            {
                if (!selectedRows.Any(selectedColumn => selectedColumn.ColumnName == existingColumn))
                {
                    RemoveColumn(connection, tableName, existingColumn);
                }
            }

            foreach (var column in selectedRows)
            {
                if (ColumnExists(connection, tableName, column.ColumnName))
                {
                    // If column exists, alter it
                    var alterColumnQuery = $"ALTER TABLE {tableName} ALTER COLUMN " +
                       $"{column.ColumnName} {column.DataType} " +
                       $"{(!string.IsNullOrEmpty(column.Size) && int.TryParse(column.Size, out int size) ? $"({size})" : "")} " +
                       $"{(column.AllowNull ? "NULL" : "NOT NULL")} ";
                    using (SqlCommand alterColumnCommand = new SqlCommand(alterColumnQuery, connection))
                    {
                        alterColumnCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    var addColumnQuery = $"ALTER TABLE {tableName} ADD " +
                        $"{column.ColumnName} {column.DataType} " +
                        $"{(!string.IsNullOrEmpty(column.Size) && int.TryParse(column.Size, out int size) ? $"({size})" : "")} " +
                        $"{(column.AllowNull ? "NULL" : "NOT NULL")} ";
                    using (SqlCommand addColumnCommand = new SqlCommand(addColumnQuery, connection))
                    {
                        addColumnCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private List<string> GetTableColumns(SqlConnection connection, string tableName)
        {
           
            var columns = new List<string>();

            using (SqlCommand command = new SqlCommand($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }

            return columns;
        }

        private void RemoveColumn(SqlConnection connection, string tableName, string columnName)
        {

            var dropColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {columnName};";
            using (SqlCommand dropColumnCommand = new SqlCommand(dropColumnQuery, connection))
            {
                dropColumnCommand.ExecuteNonQuery();
            }
        }



    }
}
