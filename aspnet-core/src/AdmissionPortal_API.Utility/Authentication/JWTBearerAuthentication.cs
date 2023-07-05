using AdmissionPortal_API.Domain.ViewModel.ModelInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdmissionPortal_API.Utility
{
    public class JWTBearerAuthentication
    {
        private IConfiguration _configuration;

        public JWTBearerAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// GenerateJSONWebToken
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GenerateJSONWebToken(string email, string userId, string userType, string loginType, string universityCollegeId, string user_Name, string headId, IConfiguration configuration,string Name=null,string Phone=null)
        {
            string userName = string.IsNullOrEmpty(user_Name) ? string.Empty : user_Name;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {

                new Claim(JwtRegisteredClaimNames.FamilyName,email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("UserID", userId),
                new Claim("UserType",userType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("loginType",loginType),
                new Claim("universityCollegeId",universityCollegeId),
                new Claim("user_Name",Convert.ToString(userName)),
                new Claim("headId",headId),
                new Claim("Name",Name??string.Empty),
                new Claim("Phone",Phone??string.Empty)
            };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                        configuration["Jwt:Issuer"],
                        claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: credentials);
            string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return generatedToken;
        }
    }
}



