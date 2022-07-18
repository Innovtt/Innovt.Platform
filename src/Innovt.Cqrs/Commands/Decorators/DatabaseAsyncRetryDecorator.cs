// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cqrs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Commands;
using Innovt.Core.CrossCutting.Log;
using Innovt.Cqrs.Decorators;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cqrs.Commands.Decorators;

public sealed class DatabaseAsyncRetryDecorator<TCommand> : BaseDatabaseRetryDecorator,
    IAsyncCommandHandler<TCommand> where TCommand : ICommand
{
    private readonly IAsyncCommandHandler<TCommand> asyncCommandHandler;


    public DatabaseAsyncRetryDecorator(IAsyncCommandHandler<TCommand> commandHandler, ILogger logger,
        int retryCount = 3) : base(logger, retryCount)
    {
        asyncCommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }

    public async Task Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        await CreateAsyncPolicy()
            .ExecuteAsync(async () => await asyncCommandHandler.Handle(command, cancellationToken));
    }
}