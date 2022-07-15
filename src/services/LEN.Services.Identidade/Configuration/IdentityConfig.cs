using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LEN.Services.Identidade.Data;
using LEN.Services.Identidade.Extensions;

namespace LEN.Services.Identidade.Configuration
{
    public static class IdentityConfigExtension
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<IdentidadeContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            service.AddDefaultIdentity<IdentityUser>()
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<IdentidadeContext>()
                   .AddDefaultTokenProviders();

            service.AddJwtConfig(configuration);

            return service;
        }

        public static void AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(appSettingsSection);

            var authSettings = appSettingsSection.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(authSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = authSettings.DominioValidade,
                    ValidIssuer = authSettings.Emissor
                };
            });
        }

        public static WebApplication UseAuthConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
