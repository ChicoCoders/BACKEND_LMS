using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LMS.Helpers
{
    public class JWTService
    {
        private string secureKey = "this is a very secure key for me";
        private string secureKeyForgetPassword = "this is a very secure key for me reset password";
        public string Generate(string username,string role)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
           {
                new System.Security.Claims.Claim("username", username),
                new System.Security.Claims.Claim("role", role) // Add user role to claims
            };

            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(
                issuer: null,
                audience: null,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);

            

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        } 

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler=new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(secureKey);
            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);
            
            return (JwtSecurityToken)validatedToken;
                }
            catch
            {
                throw new Exception("Cant Validate token");
            }
        }

        public string GetUsername(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userIdClaim = Verify(token);
            return userIdClaim.Claims.ToList().FirstOrDefault(e => e.Type == "username").Value;   
        }
        public string GetUserType(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userTypeClaim = Verify(token);
            return userTypeClaim.Claims.ToList().FirstOrDefault(e => e.Type == "role").Value;
        }

        public string GenerateResetPasswordToken(string username, string role)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKeyForgetPassword));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
           {
                new System.Security.Claims.Claim("username", username),
                new System.Security.Claims.Claim("role", role) // Add user role to claims
            };

            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(
                issuer: null,
                audience: null,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);



            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken VerifyResetPasswordToken(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKeyForgetPassword);
            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch
            {
                throw new Exception("Cant Validate token");
            }
        }

        public string GetUsernameResetPasswordToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userIdClaim = VerifyResetPasswordToken(token);
            return userIdClaim.Claims.ToList().FirstOrDefault(e => e.Type == "username").Value;
        }
    }
}
