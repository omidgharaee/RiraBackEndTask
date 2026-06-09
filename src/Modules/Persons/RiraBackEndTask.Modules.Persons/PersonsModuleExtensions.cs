using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.Modules.Persons.Infrastructure.Persistence;
using RiraBackEndTask.Modules.Persons.Infrastructure.Repositories;
using RiraBackEndTask.SharedKernel.Application.CQRS;


namespace RiraBackEndTask.Modules.Persons;

public static class PersonsModuleExtensions
{
    public static IServiceCollection AddPersonsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var applicationAssembly = typeof(PersonsModuleExtensions).Assembly;

        services.AddDbContext<PersonsDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("PersonsDatabase"));
        });

        services.AddScoped<IPersonRepository, PersonRepository>();

        services.AddCustomMediator(applicationAssembly);

        services.AddValidatorsFromAssemblies(
           new[] { applicationAssembly });

        return services;
    }
}