namespace Innovt.Data.DataModels;

public interface IBaseDataModel
{
    bool HasChanges { get; }
    bool EnableTrackingChanges { get;  set; }
}