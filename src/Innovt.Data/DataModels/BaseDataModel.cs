// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Innovt.Data.DataModels;

public abstract class BaseDataModel<TDomain, TDataModel> : INotifyPropertyChanged, IBaseDataModel
    where TDomain : class where TDataModel : class
{
    private bool enableTrackingChanges;
    private bool hasChanges;

    public bool EnableTrackingChanges
    {
        get => enableTrackingChanges;
        set
        {
            enableTrackingChanges = value;
            hasChanges = false;
        }
    }

    public bool HasChanges => hasChanges;
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (EnableTrackingChanges)
        {
            hasChanges = true;
        }
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;

        OnPropertyChanged(propertyName);

        return true;
    }

    public abstract TDomain ParseToDomain(TDataModel dataModel);

    public abstract TDataModel ParseToDataModel(TDomain domainModel);

    public virtual List<TDomain> ParseToDomain(IList<TDataModel> dataModels)
    {
        return dataModels?.Select(ParseToDomain).ToList();
    }

    public virtual List<TDataModel> ParseToDataModel(IList<TDomain> domainModels)
    {
        return domainModels?.Select(ParseToDataModel).ToList();
    }
}