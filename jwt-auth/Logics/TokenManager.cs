using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace jwt_auth.Logics
{
    public class TokenManager
    {
        // Can generate sha256 code from this link : https://codebeautify.org/sha256-hash-generator
        private static string Secret = "4b0ca75423ac3389b53b8c96867bab9aca6c09e5aacefba56884604b98a33f23";

        public static string GenerateToken(string username, string[] role)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, username),
                    // for claims identity, jwt can handle more than 1 role
                    new Claim(ClaimTypes.Role, role[0]),
                    new Claim(ClaimTypes.Role, role[1])
                }),

                // This is the user token expiry, you can configure the expiry to AddDays 9999 if there's no expiry
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature),
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static Tuple<string, string[]> ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
            {
                return null;
            }
                
            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            IEnumerable<Claim> roleClaim = identity.FindAll(ClaimTypes.Role);
            List<string> roles = new List<string>();

            foreach (Claim claim in roleClaim)
            {
                roles.Add(claim.Value);
            }

            return Tuple.Create(usernameClaim.Value, roles.ToArray());
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                {
                    return null;
                }

                byte[] key = Convert.FromBase64String(Secret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                // if the token is expired then it will throw the exception
                if (ex.GetType() == typeof(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException))
                {
                    throw ex;
                }

                throw ex;
            }
        }
    }
}