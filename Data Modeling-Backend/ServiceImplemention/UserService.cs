using DataBaseContext;
using Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplemention
{
    public class UserService : IUserService
    {
        DBContext _contxt;
        ResponseModel _rresponsewrapper;
        private readonly AppSettings _appSettings;

        public UserService(DBContext context, IOptions<AppSettings> appSettings, ResponseModel rresponsewrapper)
        {
            _contxt = context;
            _appSettings = appSettings.Value;
            _rresponsewrapper = rresponsewrapper;
        }

        public ResponseModel Authenticate(LoginModel users)
        {
            try
            {
                var user = _contxt.User.FirstOrDefault(x => x.Email.ToLower() == users.Email.ToLower() && (x.Password == CreateHash(users.Password)));
                if (user != null)
                {
                    _rresponsewrapper.IsSuccess = true;
                    _rresponsewrapper.token = generateJwtToken(users);
                    _rresponsewrapper.Data = user;
                }
                else
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.token = "";
                    _rresponsewrapper.Message = "Invalid credentials. Please try again";
                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.IsSuccess = false;
                _rresponsewrapper.Data = ex;
            }
            return _rresponsewrapper;
        }
        public ResponseModel RegisterData(RegistrationModel users)
        {
            try
            {
                var user = _contxt.User.FirstOrDefault(x => x.Email.ToLower() == users.Email.ToLower());
                if (user == null)
                {
                    user = new User();
                    user.Email = users.Email;
                    user.Username = users.UserName;
                    user.Password = CreateHash(users.Password);
                    user.IsActive = true;
                    _contxt.User.Add(user);
                    _contxt.SaveChanges();
                    _rresponsewrapper.IsSuccess = true;
                    _rresponsewrapper.Message = "User SuccessFully Registered";
                }
                else
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.Message = "Already User Registered";

                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.IsSuccess = false;
                _rresponsewrapper.Data = ex;
            }
            return _rresponsewrapper;
        }


        public ResponseModel GetUserById(int UserId) 
        {
            try
            {
                if (UserId <= 0)
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.Message = "Invalid user ID!";
                }
                else
                {
                    var user = _contxt.User.FirstOrDefault(c => c.UserId == UserId);

                    if (user != null)
                    {
                        _rresponsewrapper.IsSuccess = true;
                        _rresponsewrapper.Data = user;
                    }
                    else
                    {
                        _rresponsewrapper.IsSuccess = false;
                        _rresponsewrapper.Message = "User not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                _rresponsewrapper.IsSuccess = false;
                _rresponsewrapper.Data = ex;
            }

            return _rresponsewrapper;
         
        }


       

        public ResponseModel DeleteUserById(int UserId) 
        {
            try
            {
                
                if (UserId <= 0)
                {
                    _rresponsewrapper.IsSuccess = false;
                    _rresponsewrapper.Message = "Invalid user ID!";
                }
                else 
                {
                    var user = _contxt.User.FirstOrDefault(a => a.UserId == UserId);
                    if (user != null)
                    {
                        user.IsActive = false;
                        _contxt.SaveChanges();
                        _rresponsewrapper.IsSuccess=true;
                        _rresponsewrapper.Data = user;
                        _rresponsewrapper.Message = "User Delete Success Fully";
                    }
                    else 
                    {
                        _rresponsewrapper.IsSuccess = false;
                        _rresponsewrapper.Message = "User Not Found";
                    }
                
                }
            
            }

            catch(Exception ex) 
            {
                _rresponsewrapper.IsSuccess=false;
                _rresponsewrapper.Data=ex;
            }
         
            return _rresponsewrapper;
        }

        private string generateJwtToken(LoginModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim("Email", user.Email.ToString()));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string CreateHash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return password;

            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }


    }
}
