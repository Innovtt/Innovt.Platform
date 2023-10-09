// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cqrs

using System;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Validation;

namespace Innovt.Cqrs.Commands.Decorators;
/// <summary>
/// Decorates a command handler to include validation before handling the command.
/// </summary>
/// <typeparam name="TCommand">The type of command to be handled.</typeparam>
public sealed class CommandValidationDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> commandHandler;
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandValidationDecorator{TCommand}"/> class.
    /// </summary>
    /// <param name="commandHandler">The command handler to be decorated.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commandHandler"/> is null.</exception>
    public CommandValidationDecorator(ICommandHandler<TCommand> commandHandler)
    {
        this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
    }
    /// <summary>
    /// Handles the specified command after ensuring its validity.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="command"/> is null.</exception>
    public void Handle(TCommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        command.EnsureIsValid();

        commandHandler.Handle(command);
    }
}