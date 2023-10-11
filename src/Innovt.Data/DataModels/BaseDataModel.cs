// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Innovt.Data.DataModels;

/// <summary>
/// The base abstract class for data model classes that facilitate communication between domain models and data models.
/// </summary>
/// <typeparam name="TDomain">The type representing the domain model.</typeparam>
/// <typeparam name="TDataModel">The type representing the data model.</typeparam>
public abstract class BaseDataModel<TDomain, TDataModel> : INotifyPropertyChanged, IBaseDataModel
    where TDomain : class where TDataModel : class
{
    private bool enableTrackingChanges;
    private bool hasChanges;

    /// <summary>
    /// Gets or sets a value indicating whether change tracking is enabled.
    /// </summary>
    public bool EnableTrackingChanges
    {
        get => enableTrackingChanges;
        set
        {
            enableTrackingChanges = value;
            hasChanges = false;
        }
    }

    /// <summary>
    /// Gets a value indicating whether there are pending changes in the data model.
    /// </summary>
    public bool HasChanges => hasChanges;

    /// <summary>
    /// Event that is raised when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event and updates the change tracking if enabled.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed (auto-populated).</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (EnableTrackingChanges) hasChanges = true;
    }

    /// <summary>
    /// Sets the value of a property and raises the PropertyChanged event if the value changes.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    /// <param name="field">Reference to the property's backing field.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="propertyName">The name of the property that changed (auto-populated).</param>
    /// <returns>True if the property value changed; otherwise, false.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;

        OnPropertyChanged(propertyName);

        return true;
    }

    /// <summary>
    /// Converts a data model instance to a domain model instance.
    /// </summary>
    /// <param name="dataModel">The data model instance to convert.</param>
    /// <returns>The corresponding domain model instance.</returns>
    public abstract TDomain ParseToDomain(TDataModel dataModel);

    /// <summary>
    /// Converts a domain model instance to a data model instance.
    /// </summary>
    /// <param name="domainModel">The domain model instance to convert.</param>
    /// <returns>The corresponding data model instance.</returns>
    public abstract TDataModel ParseToDataModel(TDomain domainModel);

    /// <summary>
    /// Converts a list of data model instances to a list of domain model instances.
    /// </summary>
    /// <param name="dataModels">The list of data model instances to convert.</param>
    /// <returns>A list of corresponding domain model instances.</returns>
    public virtual List<TDomain> ParseToDomain(IList<TDataModel> dataModels)
    {
        return dataModels?.Select(ParseToDomain).ToList();
    }

    /// <summary>
    /// Converts a list of domain model instances to a list of data model instances.
    /// </summary>
    /// <param name="domainModels">The list of domain model instances to convert.</param>
    /// <returns>A list of corresponding data model instances.</returns>
    public virtual List<TDataModel> ParseToDataModel(IList<TDomain> domainModels)
    {
        return domainModels?.Select(ParseToDataModel).ToList();
    }
}