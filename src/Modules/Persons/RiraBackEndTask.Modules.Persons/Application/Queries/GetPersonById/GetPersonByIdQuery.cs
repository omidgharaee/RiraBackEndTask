using RiraBackEndTask.Modules.Persons.Application.DTOs;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Queries.GetPersonById;
public sealed record GetPersonByIdQuery(Guid Id)
    : IRequest<PersonDto>;