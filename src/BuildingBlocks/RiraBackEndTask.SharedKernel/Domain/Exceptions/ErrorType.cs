using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Domain.Exceptions;
public enum ErrorType
{
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    BusinessRule = 4,
    Internal = 5
}