using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.Abstractions.Repositories.Curier;
using DeliveryAPI.Application.Abstractions.Repositories.Order;
using DeliveryAPI.Application.Abstractions.Repositories.OrderRequest;
using DeliveryAPI.Application.Abstractions.Repositories.UserProfiles;
using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPI.Application.Abstractions.UnitOfWork;
using DeliveryAPI.Persistence.Context;
using DeliveryAPI.Persistence.Implementations;
using DeliveryAPI.Persistence.Implementations.Repositories;
using DeliveryAPI.Persistence.Implementations.Services;
using DeliveryAPI.Persistence.Implementations.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryAPI.Persistence.ServiceRegistration;

public static class ServiceRegistration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("MSSQL")));
        
        services.AddServices();
        
        services.AddRepositories();
        
        services.AddUnitOfWork();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRequestService, OrderRequestService>();
        
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<ICourierProfileRepository, CurierProfileRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderRequestRepository, OrderRequestRepository>();

    }

    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}