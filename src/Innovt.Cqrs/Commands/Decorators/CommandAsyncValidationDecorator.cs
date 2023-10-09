// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Commands.Decorators;
/// <summary>
/// Decorates an asynchronous command handler to include validation before handling the command.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
public sealed class CommandAsyncValidationDecorator<TCommand> : IAsyncCommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAsyncValidationDecorator{TCommand}"/> class.
    /// </summary>
    /// <param name="commandHandler">The asynchronous command handler to be decorated.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commandHandler"/> is null.</exception>
    public CommandAsyncValidationDecorator(IAsyncCommandHandler<TCommand> commandHandler)
    {
        asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }
    /// <summary>
    /// Handles the specified command asynchronously after ensuring its validity.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the command.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is null.</exception>
    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        command.EnsureIsValid();

        await asyncCommandHandler.Handle(command, cancellationToken).ConfigureAwait(false);
    }
}