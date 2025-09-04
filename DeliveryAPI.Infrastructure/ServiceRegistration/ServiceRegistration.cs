using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CloudinaryDotNet;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace DeliveryAPI.Infrastructure.ServiceRegistration;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<IRazerRenderViewService, RazerRenderViewService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddAuthorization();
        services.AddAuth(configuration);
        services.AddRedis(configuration);
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<IUserIdentityService, UserIdentityService>();
        services.AddCloudinary(configuration);
        services.AddScoped<ICloudinaryService, CloudinaryService>();
    }

    private static void AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudinaryAccount = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:ApiKey"],configuration["Cloudinary:ApiSecret"]);
        var cloudinary = new Cloudinary(cloudinaryAccount);
        services.AddSingleton(cloudinary);
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(opt =>
        {
            var config = new ConfigurationOptions
            {
                EndPoints = {configuration["Redis:EndPoints"]!},
                User = configuration["Redis:User"],
                Password = configuration["Redis:Password"],
                AbortOnConnectFail = Convert.ToBoolean(configuration["Redis:AbortOnConnectFail"]),
                Ssl = Convert.ToBoolean(configuration["Redis:Ssl"])
            };
            return ConnectionMultiplexer.Connect(config);
        });
    }
    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opt  =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                
                LifetimeValidator = (_,expires,token,_) => token is not null && expires > DateTime.UtcNow,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
            };
            opt.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                     IRedisService redisService = context.HttpContext.RequestServices.GetService<IRedisService>()!;
                     
                     var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                     if (string.IsNullOrEmpty(jti))
                     {
                         context.Fail("Invalid token: no jti");
                         return Task.CompletedTask;
                     }
                     string key = $"bl_accessToken:{jti}";
                     if(redisService.ExistsAsync(key).Result)
                         context.Fail("Token already exists");
                     
                     return Task.CompletedTask;
                }
            };
        });
    }
}