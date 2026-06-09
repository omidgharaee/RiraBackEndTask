using Microsoft.Extensions.Logging;
using RiraBackEndTask.Modules.Persons.Application.Abstractions;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.DeletePerson;

public sealed class DeletePersonHandler
    : IRequestHandler<DeletePersonCommand, DeletePersonResponse>
{
    private readonly IPersonRepository _repository;
    private readonly ILogger<DeletePersonHandler> _logger;

    public DeletePersonHandler(
        IPersonRepository repository,
        ILogger<DeletePersonHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DeletePersonResponse> HandleAsync(
        DeletePersonCommand request,
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
                    "DeletePerson failed. Person not found. PersonId: {PersonId}",
                    request.Id);

                throw new DomainException(
                    "شخص مورد نظر پیدا نشد.",
                    ErrorType.NotFound);
            }

            _repository.Delete(person);

            await _repository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Person deleted successfully. PersonId: {PersonId}",
                request.Id);

            return new DeletePersonResponse(
                "شخص با موفقیت حذف شد.");
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while deleting person. PersonId: {PersonId}",
                request.Id);

            throw new DomainException(
                "خطای داخلی هنگام حذف شخص رخ داد.",
                ErrorType.Internal);
        }
    }
}