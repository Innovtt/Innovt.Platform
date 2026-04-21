// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

namespace Innovt.Cloud.AWS.Dynamo.ChangeTracking;

public interface IChangeTracker
{
    void Attach(object entity);

    EntityState GetState(object entity);
}
