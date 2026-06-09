using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.SharedKernel.Application.CQRS;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}