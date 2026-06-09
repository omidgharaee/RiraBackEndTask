using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Domain.Exceptions;

public sealed class DomainException : Exception
{
    public DomainException(
        string message,
        ErrorType errorType = ErrorType.BusinessRule)
        : base(message)
    {
        ErrorType = errorType;
    }

    public ErrorType ErrorType { get; }
}