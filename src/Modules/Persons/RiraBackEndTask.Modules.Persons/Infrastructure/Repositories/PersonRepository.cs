using Microsoft.EntityFrameworkCore;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.Modules.Persons.Domain.Entities;
using RiraBackEndTask.Modules.Persons.Infrastructure.Persistence;

namespace RiraBackEndTask.Modules.Persons.Infrastructure.Repositories;
public sealed class PersonRepository : IPersonRepository
{
    private readonly PersonsDbContext _dbContext;

    public PersonRepository(PersonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Person?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return _dbContext.Persons
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<Person>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Persons
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByNationalCodeAsync(
        string nationalCode,
        Guid? exceptId,
        CancellationToken cancellationToken)
    {
        return _dbContext.Persons.AnyAsync(x =>
            x.NationalCode == nationalCode &&
            (!exceptId.HasValue || x.Id != exceptId.Value),
            cancellationToken);
    }

    public async Task AddAsync(
        Person person,
        CancellationToken cancellationToken)
    {
        await _dbContext.Persons.AddAsync(person, cancellationToken);
    }

    public void Delete(Person person)
    {
        _dbContext.Persons.Remove(person);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}