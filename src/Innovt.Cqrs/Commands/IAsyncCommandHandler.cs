using Innovt.Core.Cqrs.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cqrs.Commands
{
    public interface IAsyncCommandHandler<in T> where T : ICommand
    {
        Task Handle(T command, CancellationToken cancellationToken = default);
    }

    public interface IAsyncCommandHandler<in T, TResult> where T : ICommand
    {
        Task<TResult> Handle(T command, CancellationToken cancellationToken = default);
    }
}