// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

namespace Innovt.Data.DataModels;

/// <summary>
/// Represents the base interface for data model entities, providing change tracking capabilities.
/// </summary>
public interface IBaseDataModel
{
    /// <summary>
    /// Gets a value indicating whether the data model has pending changes.
    /// </summary>
    bool HasChanges { get; }

    /// <summary>
    /// Gets or sets a value indicating whether change tracking is enabled for the data model.
    /// </summary>
    bool EnableTrackingChanges { get; set; }
}