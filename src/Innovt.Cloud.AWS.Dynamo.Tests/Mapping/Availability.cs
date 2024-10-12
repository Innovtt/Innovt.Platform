using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Domain.Core.Model;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents the availability of a cloud expert within the system, including the days and times they are available.
/// </summary>
public class Availability : Entity<Guid>, IValidatableObject
{
    /// <summary>
    ///     Initializes a new instance of the Availability class with a unique identifier.
    /// </summary>
    public Availability()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    ///     Gets or sets the TimeZoneId associated with the availability.
    /// </summary>
    public int TimeZoneId { get; set; }

    /// <summary>
    ///     Gets or sets the list of availability days for the cloud expert.
    /// </summary>
    public List<AvailabilityDay> Days { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether support is enabled for this availability.
    /// </summary>
    public bool IsSupportEnable { get; set; }

    /// <summary>
    ///     Gets or sets the identifier of the owner of this availability.
    /// </summary>
    public Guid OwnerId { get; set; }

    public int DayOfWeek { get; set; }

    /// <summary>
    ///     Performs validation on the Availability instance and returns validation results.
    /// </summary>
    /// <param name="validationContext">The context in which the validation is performed.</param>
    /// <returns>A collection of ValidationResult objects.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Custom validation logic would be added here
        return new List<ValidationResult>();
    }
}