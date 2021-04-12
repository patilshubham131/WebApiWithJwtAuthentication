using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApi_JWT_Auth.Models
{
    public class TokenManager
    {

        private static string Secret = "fhsjafjlaj324f/dfhadfh435sdjfSAJ6DHFj8dshfj7JDHFLAK==";

        public static string GenerateToken(string sUserName)
        {
            byte[] Key = Convert.FromBase64String(Secret);

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Key);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, sUserName) }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }

        public static ClaimsPrincipal GetPrincipal(string sTocken)
        {
            try
            {
                JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
                JwtSecurityToken Token = (JwtSecurityToken)Handler.ReadToken(sTocken);

                if (Token == null)
                    return null;
                byte[] Key = Convert.FromBase64String(Secret);

                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };

                SecurityToken securityToken;

                ClaimsPrincipal claimsPrincipal = Handler.ValidateToken(sTocken, tokenValidationParameters, out securityToken);

                return claimsPrincipal;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool ValidateToken(string sTocken)
        {
            string sUserName = string.Empty;

            ClaimsPrincipal claimsPrincipal = GetPrincipal(sTocken);
            if (claimsPrincipal == null)
            {
                return false;
            }

            ClaimsIdentity claimsIdentity = null;
            try
            {
                claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            }
            catch (NullReferenceException ex)
            {
                return false;
            }

            Claim claim = claimsIdentity.FindFirst(ClaimTypes.Name);
            sUserName = claim.Value;

            return sUserName == null ? false : true;

        }


    }
}