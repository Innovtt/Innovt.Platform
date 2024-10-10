using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Validation;

namespace Innovt.Core.Application.UseCases;

public abstract class DefaultUseCase<TRequest, TResult>(ILogger logger) : IUseCase<TRequest, TResult> where TRequest : IValidatableObject
{
    protected ILogger Logger { get; } = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        request.EnsureIsValid();
        
        return Execute(request, cancellationToken);
    }
    
    protected abstract Task<TResult> Execute(TRequest request, CancellationToken cancellationToken = default);
}