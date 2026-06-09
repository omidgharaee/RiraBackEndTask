using RiraBackEndTask.Modules.Persons.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Abstractions;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Person>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsByNationalCodeAsync(
        string nationalCode,
        Guid? exceptId,
        CancellationToken cancellationToken);

    Task AddAsync(Person person, CancellationToken cancellationToken);

    void Delete(Person person);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}