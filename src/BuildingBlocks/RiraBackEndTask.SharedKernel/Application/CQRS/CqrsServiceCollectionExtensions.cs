using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Application.CQRS;

public static class CqrsServiceCollectionExtensions
{
    public static IServiceCollection AddCustomMediator(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();

        var handlerInterfaceType = typeof(IRequestHandler<,>);

        var handlerTypes = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false })
            .Select(type => new
            {
                Implementation = type,
                Services = type.GetInterfaces()
                    .Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .ToList()
            })
            .Where(x => x.Services.Count != 0);

        foreach (var handler in handlerTypes)
        {
            foreach (var service in handler.Services)
            {
                services.AddScoped(service, handler.Implementation);
            }
        }

        return services;
    }
}
