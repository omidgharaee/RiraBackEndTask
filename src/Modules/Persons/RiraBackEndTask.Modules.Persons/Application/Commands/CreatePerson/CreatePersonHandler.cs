using FluentValidation;
using Microsoft.Extensions.Logging;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.Modules.Persons.Domain.Entities;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.CreatePerson;

public sealed class CreatePersonHandler
    : IRequestHandler<CreatePersonCommand, CreatePersonResponse>
{
    private readonly IPersonRepository _repository;
    private readonly IValidator<CreatePersonCommand> _validator;
    private readonly ILogger<CreatePersonHandler> _logger;

    public CreatePersonHandler(
        IPersonRepository repository,
        IValidator<CreatePersonCommand> validator,
        ILogger<CreatePersonHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<CreatePersonResponse> HandleAsync(
        CreatePersonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);

            if (!validation.IsValid)
            {
                var errors = string.Join(" | ",
                    validation.Errors.Select(x => x.ErrorMessage));

                _logger.LogWarning(
                    "CreatePerson validation failed. NationalCode: {NationalCode}, Errors: {Errors}",
                    request.NationalCode,
                    errors);

                throw new DomainException(errors, ErrorType.Validation);
            }

            var exists = await _repository.ExistsByNationalCodeAsync(
                request.NationalCode,
                null,
                cancellationToken);

            if (exists)
            {
                _logger.LogWarning(
                    "CreatePerson conflict. NationalCode already exists: {NationalCode}",
                    request.NationalCode);

                throw new DomainException(
                    "این کد ملی قبلاً ثبت شده است.",
                    ErrorType.Conflict);
            }

            var person = Person.Create(
                request.FirstName,
                request.LastName,
                request.NationalCode,
                DateOnly.Parse(request.BirthDate));

            await _repository.AddAsync(person, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Person created successfully. PersonId: {PersonId}, NationalCode: {NationalCode}",
                person.Id,
                person.NationalCode);

            return new CreatePersonResponse(
                person.Id,
                "شخص با موفقیت ثبت شد.");
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while creating person. NationalCode: {NationalCode}",
                request.NationalCode);

            throw new DomainException(
                "خطای داخلی هنگام ثبت شخص رخ داد.",
                ErrorType.Internal);
        }
    }
}