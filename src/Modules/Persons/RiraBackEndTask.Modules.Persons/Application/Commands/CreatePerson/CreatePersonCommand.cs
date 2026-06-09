using RiraBackEndTask.SharedKernel.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Commands.CreatePerson;

public sealed record CreatePersonCommand(
    string FirstName,
    string LastName,
    string NationalCode,
    string BirthDate
) : IRequest<CreatePersonResponse>;
