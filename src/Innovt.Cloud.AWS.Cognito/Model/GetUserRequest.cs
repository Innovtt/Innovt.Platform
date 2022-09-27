﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class GetUserRequest : RequestBase
{
    public GetUserRequest()
    {
        ExcludeExternalUser = true;
    }

    public GetUserRequest(string field, string value) : this()
    {
        Field = field;
        Value = value;
    }

    public string Field { get; set; }

    public string Value { get; set; }

    public bool ExcludeExternalUser { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Field.IsNullOrEmpty() && Value.IsNullOrEmpty())
            yield return new ValidationResult(Messages.FieldFilterIsRequired, new[] { nameof(Field) });

        if (Value.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ValueFieldIsRequired, new[] { nameof(Value) });
    }
}