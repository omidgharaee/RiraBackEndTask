using FluentValidation;
using Microsoft.Extensions.Logging;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.UpdatePerson;

public sealed class UpdatePersonHandler
    : IRequestHandler<UpdatePersonCommand, UpdatePersonResponse>
{
    private readonly IPersonRepository _repository;
    private readonly IValidator<UpdatePersonCommand> _validator;
    private readonly ILogger<UpdatePersonHandler> _logger;

    public UpdatePersonHandler(
        IPersonRepository repository,
        IValidator<UpdatePersonCommand> validator,
        ILogger<UpdatePersonHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<UpdatePersonResponse> HandleAsync(
        UpdatePersonCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validation = await _validator.ValidateAsync(
                request,
                cancellationToken);

            if (!validation.IsValid)
            {
                var errors = string.Join(
                    " | ",
                    validation.Errors.Select(x => x.ErrorMessage));

                _logger.LogWarning(
                    "UpdatePerson validation failed. PersonId: {PersonId}, Errors: {Errors}",
                    request.Id,
                    errors);

                throw new DomainException(
                    errors,
                    ErrorType.Validation);
            }

            var person = await _repository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (person is null)
            {
                _logger.LogWarning(
                    "UpdatePerson failed. Person not found. PersonId: {PersonId}",
                    request.Id);

                throw new DomainException(
                    "شخص مورد نظر پیدا نشد.",
                    ErrorType.NotFound);
            }

            var nationalCodeExists =
                await _repository.ExistsByNationalCodeAsync(
                    request.NationalCode,
                    request.Id,
                    cancellationToken);

            if (nationalCodeExists)
            {
                _logger.LogWarning(
                    "UpdatePerson conflict. NationalCode already exists. PersonId: {PersonId}, NationalCode: {NationalCode}",
                    request.Id,
                    request.NationalCode);

                throw new DomainException(
                    "این کد ملی برای شخص دیگری ثبت شده است.",
                    ErrorType.Conflict);
            }

            person.Update(
                request.FirstName,
                request.LastName,
                request.NationalCode,
                DateOnly.Parse(request.BirthDate));

            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Person updated successfully. PersonId: {PersonId}",
                request.Id);

            return new UpdatePersonResponse(
                "شخص با موفقیت ویرایش شد.");
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while updating person. PersonId: {PersonId}",
                request.Id);

            throw new DomainException(
                "خطای داخلی هنگام ویرایش شخص رخ داد.",
                ErrorType.Internal);
        }
    }
}