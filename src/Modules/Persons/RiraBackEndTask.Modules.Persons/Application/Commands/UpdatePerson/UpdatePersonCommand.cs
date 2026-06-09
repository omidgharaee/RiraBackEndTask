using RiraBackEndTask.SharedKernel.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.UpdatePerson;
public sealed record UpdatePersonCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    string BirthDate
) : IRequest<UpdatePersonResponse>;
