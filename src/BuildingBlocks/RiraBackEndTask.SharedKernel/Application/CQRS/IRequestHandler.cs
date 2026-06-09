using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Application.CQRS;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken);
}
