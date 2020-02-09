using Dna;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudChat.Server
{
    public static class JwtTokenExtensions
    {
        public static string GenerateJwtToken(this ApplicationUser user)
        {
            //Set our tokens
            var claims = new[]
            {
                // Unique Id for this token
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString("N")),

                // The username using the identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),

                // Add user Id so that UserManager.GetUserAsync() can find the user based on Id 
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                                Framework.Construction.Configuration["Jwt:SecretKey"]));

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Generate Jwt Token
            var token = new JwtSecurityToken(
                issuer: Framework.Construction.Configuration["Jwt:Issuer"],
                audience: Framework.Construction.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
