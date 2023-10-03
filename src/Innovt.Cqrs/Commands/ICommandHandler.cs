// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using Innovt.Core.Cqrs.Commands;

namespace Innovt.Cqrs.Commands;
/// <summary>
/// Defines a synchronous command handler for a specific type of command.
/// </summary>
/// <typeparam name="T">The type of command to be handled.</typeparam>
public interface ICommandHandler<in T> where T : ICommand
{
    /// <summary>
    /// Handles the specified command synchronously.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    void Handle(T command);
}
/// <summary>
/// Defines a synchronous command handler for a specific type of command with a result.
/// </summary>
/// <typeparam name="T">The type of command to be handled.</typeparam>
/// <typeparam name="TResult">The type of result expected from handling the command.</typeparam>
public interface ICommandHandler<in T, out TResult> where T : ICommand
{
    /// <summary>
    /// Handles the specified command synchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <returns>The result of handling the command.</returns>
    TResult Handle(T command);
}