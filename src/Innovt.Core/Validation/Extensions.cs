// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Exceptions;

namespace Innovt.Core.Validation;

/// <summary>
///     A static class containing extension methods for validating objects that implement the
///     <see cref="IValidatableObject" /> interface.
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Validates a collection of objects implementing the <see cref="IValidatableObject" /> interface and returns a
    ///     collection of <see cref="ValidationResult" /> objects.
    /// </summary>
    /// <param name="array">The collection of objects to validate.</param>
    /// <param name="context">An optional <see cref="ValidationContext" /> to specify validation context.</param>
    /// <returns>A collection of <see cref="ValidationResult" /> objects containing validation errors.</returns>
    public static IEnumerable<ValidationResult> Validate(this IEnumerable<IValidatableObject> array,
        ValidationContext context = null)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));

        var validationResult = new List<ValidationResult>();

        context ??= new ValidationContext(array);

        foreach (var obj in array) validationResult.AddRange(obj.Validate(context));

        return validationResult;
    }

    /// <summary>
    ///     Internal validation, validate all required fields and Custom Validation
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    internal static IList<ValidationResult> Validate(this IValidatableObject obj, ValidationContext context = null)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var validationResults = new List<ValidationResult>();

        context ??= new ValidationContext(obj);

        //first validate the object 
        Validator.TryValidateObject(obj, context, validationResults);

        var result = obj.Validate(context)?.ToList();

        if (result?.Any() == true)
            foreach (var item in result)
                if (validationResults.All(r => r.ErrorMessage != item.ErrorMessage))
                    validationResults.Add(item);

        return validationResults.Distinct().ToList();
    }

    /// <summary>
    ///     Checks whether an object implementing the <see cref="IValidatableObject" /> interface is valid.
    /// </summary>
    /// <param name="obj">The object to validate.</param>
    /// <param name="context">An optional <see cref="ValidationContext" /> to specify validation context.</param>
    /// <returns>True if the object is valid; otherwise, false.</returns>
    public static bool IsValid(this IValidatableObject obj, ValidationContext context = null)
    {
        if (obj == null)
            return false;

        var result = Validate(obj, context);

        return !result.Any();
    }

    /// <summary>
    ///     Ensures that an object implementing the <see cref="IValidatableObject" /> interface is valid; otherwise, throws a
    ///     <see cref="BusinessException" /> with validation errors.
    /// </summary>
    /// <param name="obj">The object to validate.</param>
    /// <param name="context">An optional <see cref="ValidationContext" /> to specify validation context.</param>
    public static void EnsureIsValid(this IValidatableObject obj, ValidationContext context = null)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var validationResults = Validate(obj, context);

        if (!validationResults.Any()) return;

        var errors = from e in validationResults
            select new ErrorMessage(e.ErrorMessage, string.Join(",", e.MemberNames));

        throw new BusinessException(errors.ToList());
    }

    /// <summary>
    ///     Ensures that a command object is valid by treating it as an <see cref="IValidatableObject" />; otherwise, throws a
    ///     <see cref="BusinessException" /> with validation errors.
    /// </summary>
    /// <param name="command">The command to validate.</param>
    /// <param name="context">An optional <see cref="ValidationContext" /> to specify validation context.</param>
    public static void EnsureIsValid([NotNull] this ICommand command, ValidationContext context = null)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        EnsureIsValid((IValidatableObject)command, context);
    }

    /// <summary>
    ///     Ensures that a command object is valid by treating it as an <see cref="IValidatableObject" />; otherwise, throws a
    ///     <see cref="BusinessException" /> with validation errors.
    /// </summary>
    /// <param name="command">The command to validate.</param>
    /// <param name="contextName">The name of the validation context.</param>
    public static void EnsureIsValid([NotNull] this ICommand command, string contextName)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        EnsureIsValid((IValidatableObject)command,
            new ValidationContext(command) { MemberName = contextName, DisplayName = contextName });
    }
}