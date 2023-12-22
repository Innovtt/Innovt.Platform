// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Commands.Decorators;

/// <summary>
///     Decorates a command handler to include retry logic in case of failures.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
public sealed class DatabaseRetryDecorator<TCommand> : BaseDatabaseRetryDecorator, ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> commandHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DatabaseRetryDecorator{TCommand}" /> class.
    /// </summary>
    /// <param name="commandHandler">The command handler to be decorated.</param>
    /// <param name="logger">The logger for capturing retry attempts.</param>
    /// <param name="retryCount">The number of retry attempts (default is 3).</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="commandHandler" /> or <paramref name="logger" /> is
    ///     null.
    /// </exception>
    public DatabaseRetryDecorator(ICommandHandler<TCommand> commandHandler, ILogger logger, int retryCount = 3) :
        base(logger, retryCount)
    {
        this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }

    /// <summary>
    ///     Handles the specified command with retry logic in case of failures.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    public void Handle(TCommand command)
    {
        CreatePolicy().Execute(() => commandHandler.Handle(command));
    }
}