using Grpc.Core;
using RiraBackEndTask.Modules.Persons.Application.Commands.CreatePerson;
using RiraBackEndTask.Modules.Persons.Application.Commands.DeletePerson;
using RiraBackEndTask.Modules.Persons.Application.Commands.UpdatePerson;
using RiraBackEndTask.Modules.Persons.Application.DTOs;
using RiraBackEndTask.Modules.Persons.Application.Queries.GetPersonById;
using RiraBackEndTask.Modules.Persons.Application.Queries.GetPersons;
using RiraBackEndTask.Modules.Persons.Presentation.Grpc.Protos;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;
using RiraBackEndTask.SharedKernel.Grpc;

namespace RiraBackEndTask.Modules.Persons.Presentation.Grpc;

public sealed class PersonGrpcService : PersonService.PersonServiceBase
{
    private readonly IMediator _mediator;

    public PersonGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<CreatePersonGrpcResponse> CreatePerson(
        CreatePersonGrpcRequest request,
        ServerCallContext context)
    {
        return GrpcSafeExecutor.ExecuteAsync(async () =>
        {
            var result = await _mediator.SendAsync(
                new CreatePersonCommand(
                    request.FirstName,
                    request.LastName,
                    request.NationalCode,
                    request.BirthDate),
                context.CancellationToken);

            return new CreatePersonGrpcResponse
            {
                Id = result.Id.ToString(),
                Message = result.Message
            };
        });
    }

    public override Task<PersonGrpcDto> GetPersonById(
        GetPersonByIdGrpcRequest request,
        ServerCallContext context)
    {
        return GrpcSafeExecutor.ExecuteAsync(async () =>
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new DomainException(
                    "شناسه معتبر نیست.",
                    ErrorType.Validation);

            var result = await _mediator.SendAsync(
                new GetPersonByIdQuery(id),
                context.CancellationToken);

            return ToGrpcDto(result);
        });
    }

    public override Task<GetPersonsGrpcResponse> GetPersons(
        GetPersonsGrpcRequest request,
        ServerCallContext context)
    {
        return GrpcSafeExecutor.ExecuteAsync(async () =>
        {
            var result = await _mediator.SendAsync(
                new GetPersonsQuery(),
                context.CancellationToken);

            var response = new GetPersonsGrpcResponse();

            response.Persons.AddRange(result.Select(ToGrpcDto));

            return response;
        });
    }

    public override Task<UpdatePersonGrpcResponse> UpdatePerson(
        UpdatePersonGrpcRequest request,
        ServerCallContext context)
    {
        return GrpcSafeExecutor.ExecuteAsync(async () =>
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new DomainException(
                    "شناسه معتبر نیست.",
                    ErrorType.Validation);

            var result = await _mediator.SendAsync(
                new UpdatePersonCommand(
                    id,
                    request.FirstName,
                    request.LastName,
                    request.NationalCode,
                    request.BirthDate),
                context.CancellationToken);

            return new UpdatePersonGrpcResponse
            {
                Message = result.Message
            };
        });
    }

    public override Task<DeletePersonGrpcResponse> DeletePerson(
        DeletePersonGrpcRequest request,
        ServerCallContext context)
    {
        return GrpcSafeExecutor.ExecuteAsync(async () =>
        {
            if (!Guid.TryParse(request.Id, out var id))
                throw new DomainException(
                    "شناسه معتبر نیست.",
                    ErrorType.Validation);

            var result = await _mediator.SendAsync(
                new DeletePersonCommand(id),
                context.CancellationToken);

            return new DeletePersonGrpcResponse
            {
                Message = result.Message
            };
        });
    }

    private static PersonGrpcDto ToGrpcDto(PersonDto person)
    {
        return new PersonGrpcDto
        {
            Id = person.Id.ToString(),
            FirstName = person.FirstName,
            LastName = person.LastName,
            NationalCode = person.NationalCode,
            BirthDate = person.BirthDate.ToString("yyyy-MM-dd")
        };
    }
}