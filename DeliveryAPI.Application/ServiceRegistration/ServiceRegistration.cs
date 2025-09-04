using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryAPI.Application.ServiceRegistration;

public static class ServiceRegistration
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}