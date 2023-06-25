using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using AutoMapper.Extensions.ExpressionMapping;

namespace RithmicSoul.Admin.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddExpressionMapping();
        }, AppDomain.CurrentDomain.GetAssemblies());
        
        var registered = services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
        return services;
    }
}