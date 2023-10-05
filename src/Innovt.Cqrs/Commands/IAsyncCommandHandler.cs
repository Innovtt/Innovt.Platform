// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Cqrs.Commands;
/// <summary>
/// Defines an asynchronous command handler for a specific type of command.
/// </summary>
/// <typeparam name="T">The type of command to be handled.</typeparam>
public interface IAsyncCommandHandler<in T> where T : ICommand
{
    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the command.</returns>
    Task Handle(T command, CancellationToken cancellationToken = default);
}
/// <summary>
/// Defines an asynchronous command handler for a specific type of command with a result.
/// </summary>
/// <typeparam name="T">The type of command to be handled.</typeparam>
/// <typeparam name="TResult">The type of result expected from handling the command.</typeparam>
public interface IAsyncCommandHandler<in T, TResult> where T : ICommand
{
    /// <summary>
    /// Handles the specified command asynchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the command and the result.</returns>
    Task<TResult> Handle(T command, CancellationToken cancellationToken = default);
}