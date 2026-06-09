using Microsoft.EntityFrameworkCore;
using RiraBackEndTask.Modules.Persons.Domain.Entities;

namespace RiraBackEndTask.Modules.Persons.Infrastructure.Persistence;

public sealed class PersonsDbContext : DbContext
{
    public PersonsDbContext(DbContextOptions<PersonsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PersonsDbContext).Assembly);
    }
}
