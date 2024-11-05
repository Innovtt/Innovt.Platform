using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.Core.Application;

public class UseCaseHandler(IServiceProvider serviceProvider) : IUseCaseHandler
{
    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest tRequest, CancellationToken cancellationToken = default) where TRequest : IValidatableObject
    {       
        var handler = serviceProvider.GetService<IUseCase<TRequest, TResponse>>();

        if (handler == null)
            throw new InvalidOperationException($"Handler not found for {typeof(TRequest).Name}");
        
        return await handler.ExecuteAsync(tRequest, cancellationToken);
    }
}