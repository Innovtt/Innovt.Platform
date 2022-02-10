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

namespace Innovt.Cqrs.Commands.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : BaseDatabaseRetryDecorator, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> commandHandler;


        public DatabaseRetryDecorator(ICommandHandler<TCommand> commandHandler, ILogger logger, int retryCount = 3) :
            base(logger, retryCount)
        {
            this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        public void Handle(TCommand command)
        {
            CreatePolicy().Execute(() => commandHandler.Handle(command));
        }
    }
}