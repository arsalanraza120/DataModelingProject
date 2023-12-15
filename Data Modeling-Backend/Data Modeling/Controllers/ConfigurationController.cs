using Interface;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace Data_Modeling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
      
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpPost("SaveConnectionStringParams")]
        public Task<ResponseModel> SaveConnectionStringParams(ConnectionDb Conn)
        {
           
            var response = _configurationService.SaveConnStringParams(Conn);
            return response;
        }

        [HttpGet("GetCredentialsByCredentialName")]
        public ResponseModel GetCredentialsByCredentialName()
        {
            var response = _configurationService.GetCredentialsByCredentialName();
            return response;
        }

        [HttpDelete("RemoveConnectionById")]
        public ResponseModel RemoveConnectionById(int id)
        { 
           var response = _configurationService.RemoveConnectionById(id);
           return response;
        }


        [HttpGet("GetCredentialById")]
        public ResponseModel GetCredentialById(int id) 
        {
          var response = _configurationService.GetCredentialById(id);
          return response;
        }

        [HttpPost("GetAllTablesMetaData")]
        public Task<ResponseModel> GetAllTablesMetaData(ConnectionDb Conn)
        {
            var response = _configurationService.GetAllTablesMetaData(Conn);
            return response;
        }

        [HttpPost("GetTableNames")]
        public Task<ResponseModel> GetTableNames(ConnectionDb Conn) 
        {
            var response = _configurationService.GetTableNames(Conn);
            return response;
        }

        [HttpPost("GetMetaDataTableByName")]
        public Task<ResponseModel> GetMetaDataTableByName(string tblName,ConnectionDb Conn)
        {
            var response = _configurationService.GetMetaDataTableByName(tblName,Conn);
            return response;
        }

        [HttpPost("GetMetaDataMultipleTableByName")]
        public async Task<ResponseModel> GetMetaDataMultipleTableByName(MultipleTableRequest request)
        {
            var response = await _configurationService.GetMetaDataMultipleTableByName(request.TableNames, request.Conn);
            return response;
        }


        [HttpPost("CreateTable")]
        public Task<ResponseModel> CreateTable(TableData tblData) 
        {
            var response = _configurationService.CreateTable(tblData);
            return response;
        }
    }
}
