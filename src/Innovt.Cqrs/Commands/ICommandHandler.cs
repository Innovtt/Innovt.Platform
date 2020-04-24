using Innovt.Core.Cqrs.Commands;

namespace Innovt.Cqrs.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        void Execute(T command);
    }

    public interface ICommandHandler<in T, out TResult> where T : ICommand
    {
        TResult Execute(T command);
    }
}
