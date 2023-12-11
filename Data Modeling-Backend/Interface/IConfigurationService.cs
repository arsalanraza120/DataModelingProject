using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IConfigurationService 
    {
     
        public Task<ResponseModel> SaveConnStringParams(ConnectionDb Conn);
        public ResponseModel GetCredentialsByCredentialName();
        public ResponseModel RemoveConnectionById(int Id);
        public Task<ResponseModel> GetAllTablesMetaData(ConnectionDb Conn);
        public Task<ResponseModel> GetTableNames(ConnectionDb Conn);
        public ResponseModel GetCredentialById(int Id);
        public Task<ResponseModel> GetMetaDataTableByName(string tblName, ConnectionDb Conn);

    }
}




