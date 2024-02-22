// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Commands;

/// <summary>
///     Represents a command interface that can be validated.
/// </summary>
/// <remarks>
///     This interface is used to define a command, which represents an action or operation that can be executed.
///     Commands often include data or parameters needed to perform the action.
///     Implementing the <see cref="IValidatableObject" /> interface allows for command validation.
/// </remarks>
public interface ICommand : IValidatableObject
{
}