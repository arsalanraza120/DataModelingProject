using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;

namespace Data_Modeling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IUserService userService, IConfigurationService configurationService)
        {
            _userService = userService;
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
    }
}
