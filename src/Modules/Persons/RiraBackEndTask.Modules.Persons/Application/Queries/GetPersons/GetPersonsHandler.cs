using Microsoft.Extensions.Logging;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.Modules.Persons.Application.DTOs;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;

namespace RiraBackEndTask.Modules.Persons.Application.Queries.GetPersons;

public sealed class GetPersonsHandler
    : IRequestHandler<GetPersonsQuery, IReadOnlyList<PersonDto>>
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<GetPersonsHandler> _logger;

    public GetPersonsHandler(
        IPersonRepository repository,
        ILogger<GetPersonsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<PersonDto>> HandleAsync(
        GetPersonsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var persons = await _repository.GetAllAsync(
                cancellationToken);

            var result = persons
                .Select(x => new PersonDto(
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.NationalCode,
                    x.BirthDate))
                .ToList();

            _logger.LogInformation(
                "Persons fetched successfully. Count: {Count}",
                result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while fetching persons list.");

            throw new DomainException(
                "خطای داخلی هنگام دریافت لیست اشخاص رخ داد.",
                ErrorType.Internal);
        }
    }
}