// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request for retrieving user information based on a specified field and value.
/// </summary>
public class GetUserRequest : RequestBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GetUserRequest" /> class with default settings.
    /// </summary>
    public GetUserRequest()
    {
        ExcludeExternalUser = true;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetUserRequest" /> class with specified field and value.
    /// </summary>
    /// <param name="field">The field to filter user retrieval.</param>
    /// <param name="value">The value to match in the specified field.</param>
    public GetUserRequest(string field, string value) : this()
    {
        Field = field;
        Value = value;
    }

    /// <summary>
    ///     Gets or sets the field to filter user retrieval.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    ///     Gets or sets the value to match in the specified field.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to exclude external users in the retrieval.
    /// </summary>
    public bool ExcludeExternalUser { get; set; }

    /// <summary>
    ///     Validates the <see cref="GetUserRequest" /> instance.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An <see cref="IEnumerable{ValidationResult}" /> containing validation errors, if any.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Field.IsNullOrEmpty() && Value.IsNullOrEmpty())
            yield return new ValidationResult(Messages.FieldFilterIsRequired, new[] { nameof(Field) });

        if (Value.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ValueFieldIsRequired, new[] { nameof(Value) });
    }
}