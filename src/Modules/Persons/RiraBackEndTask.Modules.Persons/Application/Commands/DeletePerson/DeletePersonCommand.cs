using RiraBackEndTask.SharedKernel.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.DeletePerson;


public sealed record DeletePersonCommand(Guid Id)
    : IRequest<DeletePersonResponse>;
