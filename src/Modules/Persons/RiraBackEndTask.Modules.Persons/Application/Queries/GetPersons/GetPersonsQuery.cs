using RiraBackEndTask.Modules.Persons.Application.DTOs;
using RiraBackEndTask.SharedKernel.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.Queries.GetPersons;
public sealed record GetPersonsQuery()
    : IRequest<IReadOnlyList<PersonDto>>;