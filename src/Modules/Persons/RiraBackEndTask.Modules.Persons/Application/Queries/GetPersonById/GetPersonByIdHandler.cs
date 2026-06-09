using Microsoft.Extensions.Logging;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.Modules.Persons.Application.DTOs;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;

namespace RiraBackEndTask.Modules.Persons.Application.Queries.GetPersonById;

public sealed class GetPersonByIdHandler
    : IRequestHandler<GetPersonByIdQuery, PersonDto>
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<GetPersonByIdHandler> _logger;

    public GetPersonByIdHandler(
        IPersonRepository repository,
        ILogger<GetPersonByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PersonDto> HandleAsync(
        GetPersonByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var person = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (person is null)
            {
                _logger.LogWarning(
                    "GetPersonById failed. Person not found. PersonId: {PersonId}",
                    request.Id);

                throw new DomainException(
                    "شخص مورد نظر پیدا نشد.",
                    ErrorType.NotFound);
            }

            _logger.LogInformation(
                "Person fetched successfully. PersonId: {PersonId}",
                request.Id);

            return new PersonDto(
                person.Id,
                person.FirstName,
                person.LastName,
                person.NationalCode,
                person.BirthDate);
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while fetching person by id. PersonId: {PersonId}",
                request.Id);

            throw new DomainException(
                "خطای داخلی هنگام دریافت اطلاعات شخص رخ داد.",
                ErrorType.Internal);
        }
    }
}