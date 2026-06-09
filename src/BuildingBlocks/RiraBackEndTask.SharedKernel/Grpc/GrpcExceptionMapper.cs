using Grpc.Core;
using RiraBackEndTask.SharedKernel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Grpc;
public static class GrpcExceptionMapper
{
    public static RpcException ToRpcException(this DomainException exception)
    {
        var statusCode = exception.ErrorType switch
        {
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Conflict => StatusCode.AlreadyExists,
            ErrorType.BusinessRule => StatusCode.FailedPrecondition,
            _ => StatusCode.Internal
        };

        return new RpcException(
            new Status(statusCode, exception.Message));
    }
}