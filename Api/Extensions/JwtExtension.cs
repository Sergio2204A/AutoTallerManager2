using System.Text;
using Api.Helpers;
using Api.Services.Auth;
using Domain.Entities.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class JwtExtension
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JWT>(configuration.GetSection("JWT"));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher<UserMember>, PasswordHasher<UserMember>>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                RoleClaimType            = "roles",
                ClockSkew                = TimeSpan.Zero,
                ValidIssuer              = configuration["JWT:Issuer"],
                ValidAudience            = configuration["JWT:Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
            };
        });

        // Authorization Policies for AutoTallerManager roles
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly",      p => p.RequireRole("Admin"));
            options.AddPolicy("TallerStaff",    p => p.RequireRole("Admin", "JefeTaller", "Mecanico", "Recepcionista"));
            options.AddPolicy("Almacen",        p => p.RequireRole("Admin", "JefeAlmacen", "JefeBodega"));
        });
    }
}
