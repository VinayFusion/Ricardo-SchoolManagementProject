using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace SchoolManagementSystem.Common
{
    public class JWTClass
    {
        public string Create_JWT(Int64 _LoginID, string _UserRole)
        {
            string key = "EIPro_secretkey_B58PQ"; //Secret key which will be used later during validation    
            var issuer = "http://localhost:59367";  //normally this will be your site URL  

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("loginid", _LoginID.ToString()));

            // Add roles as multiple claims
            // foreach (var role in user.Roles)
            //{
            permClaims.Add(new Claim(ClaimTypes.Role, _UserRole));
            //}

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}