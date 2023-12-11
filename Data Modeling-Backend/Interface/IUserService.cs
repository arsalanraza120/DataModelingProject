using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface  IUserService
    {
        public ResponseModel Authenticate(LoginModel users);
        public ResponseModel RegisterData(RegistrationModel users);
        public ResponseModel GetUserById(int userId);
        public ResponseModel DeleteUserById(int userId);
    }
}




