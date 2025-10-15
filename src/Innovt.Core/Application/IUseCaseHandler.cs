using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Core.Application;

public interface IUseCaseHandler
{
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest tRequest, CancellationToken cancellationToken = default)
        where TRequest : IValidatableObject;
}