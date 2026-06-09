using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Application.DTOs;
public sealed record PersonDto(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalCode,
    DateOnly BirthDate);