// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Innovt.Core.Validation;

public static class Extensions
{
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
                if (!validationResults.Any(r => r.ErrorMessage == item.ErrorMessage))
                    validationResults.Add(item);

        return validationResults.Distinct().ToList();
    }

    public static bool IsValid(this IValidatableObject obj, ValidationContext context = null)
    {
        if (obj == null)
            return false;

        var result = Validate(obj, context);

        return !result.Any();
    }

    public static void EnsureIsValid(this IValidatableObject obj, ValidationContext context = null)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var validationResults = Validate(obj, context);

        if (!validationResults.Any()) return;

        var errors = from e in validationResults
            select new ErrorMessage(e.ErrorMessage, string.Join(",", e.MemberNames));

        throw new BusinessException(errors.ToList());
    }

    public static void EnsureIsValid([NotNull] this ICommand command, ValidationContext context = null)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        EnsureIsValid((IValidatableObject)command, context);
    }

    public static void EnsureIsValid([NotNull] this ICommand command, string contextName)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        EnsureIsValid((IValidatableObject)command,
            new ValidationContext(command) { MemberName = contextName, DisplayName = contextName });
    }
}