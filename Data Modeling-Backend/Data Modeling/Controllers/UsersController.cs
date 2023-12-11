using Interface;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Data_Modeling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) 
        {
             _userService = userService;
        }


        [HttpPost("authenticate")]
        public ResponseModel Authenticate(LoginModel model) 
        {
           var response = _userService.Authenticate(model);
           return response;
        }

        [HttpPost("RegisterData")]
        public async Task<ResponseModel> RegisterData(RegistrationModel model) 
        {
            var response = _userService.RegisterData(model);
            return response;
        }

        [HttpGet("GetUserById")]
        public ResponseModel GetUserById(int UserId) 
        {
            var response = _userService.GetUserById(UserId);
            return response;
        }

        [HttpDelete("DeleteUserById")]
        public ResponseModel DeleteUserById(int UserId) 
        {
          var response = _userService.DeleteUserById(UserId);
          return response;
        }

    }
}
