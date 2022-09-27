// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

namespace Innovt.Data.DataModels;

public interface IBaseDataModel
{
    bool HasChanges { get; }
    bool EnableTrackingChanges { get; set; }
}