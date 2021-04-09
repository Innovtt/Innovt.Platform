using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Innovt.Core.Validation
{
    public static class Extensions
    {
        public static IEnumerable<ValidationResult> Validate(this IEnumerable<IValidatableObject> array,
            ValidationContext context = null)
        {
            var validationResult = new List<ValidationResult>();

            if (array == null)
                return validationResult;

            context ??= new ValidationContext(array);

            foreach (var obj in array) validationResult.AddRange(obj.Validate(context));

            return validationResult;
        }

        public static bool IsValid(this IValidatableObject obj, ValidationContext context = null)
        {
            if (obj == null)
                return false;

            if (context == null)
                context = new ValidationContext(obj);

            var result = obj.Validate(context);

            return !result.Any();
        }

        public static void EnsureIsValid(this IValidatableObject obj, ValidationContext context = null)
        {
            if (obj == null)
                return;

            if (context == null)
                context = new ValidationContext(obj);

            var result = obj.Validate(context);

            var validationResults = result as ValidationResult[] ?? result.ToArray();

            if (validationResults.Any())
            {
                var errors = from e in validationResults
                             select new ErrorMessage(e.ErrorMessage, string.Join(",", e.MemberNames));

                throw new BusinessException(errors.ToList());
            }
        }

        public static void EnsureIsValid(this ICommand obj, ValidationContext context = null)
        {
            EnsureIsValid((IValidatableObject)obj, context);
        }

        public static IEnumerable<ValidationResult> YieldFromCollection(this IEnumerable<ValidationResult> items)
        {
            foreach (var item in items) yield return item;
        }
    }
}