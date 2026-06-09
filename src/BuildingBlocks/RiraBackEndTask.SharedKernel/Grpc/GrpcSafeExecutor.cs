using RiraBackEndTask.SharedKernel.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Grpc;

public static class GrpcSafeExecutor
{
    public static async Task<TResponse> ExecuteAsync<TResponse>(
        Func<Task<TResponse>> action)
    {
        try
        {
            return await action();
        }
        catch (DomainException ex)
        {
            throw ex.ToRpcException();
        }
    }
}