// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;

namespace Innovt.Cqrs.Commands.Decorators;

/// <summary>
/// Decorates an asynchronous command handler to include retry logic in case of failures.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
public sealed class DatabaseAsyncRetryDecorator<TCommand> : BaseDatabaseRetryDecorator,
    IAsyncCommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;


    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseAsyncRetryDecorator{TCommand}"/> class.
    /// </summary>
    /// <param name="commandHandler">The asynchronous command handler to be decorated.</param>
    /// <param name="logger">The logger for capturing retry attempts.</param>
    /// <param name="retryCount">The number of retry attempts (default is 3).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commandHandler"/> or <paramref name="logger"/> is null.</exception>
    public DatabaseAsyncRetryDecorator(IAsyncCommandHandler<TCommand> commandHandler, ILogger logger,
        int retryCount = 3) : base(logger, retryCount)
    {
        asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }

    /// <summary>
    /// Handles the specified command asynchronously with retry logic in case of failures.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous handling of the command.</returns>
    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        await CreateAsyncPolicy()
            .ExecuteAsync(async () => await asyncCommandHandler.Handle(command, cancellationToken));
    }
}