using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VezetaAPI.Extension
{
    public static class CustomJwtAuthExtention
    {
        public static void AddCustomJwtAuth(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = configuration["JWT:Issuer"],
                         ValidAudience = configuration["JWT:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                         ClockSkew = TimeSpan.Zero
                     };
                     
                 });

        }
    }
}
