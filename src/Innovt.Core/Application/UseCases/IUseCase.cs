using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Core.Application.UseCases;

public interface IUseCase<in TRequest, TResult> where TRequest : IValidatableObject
{
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}