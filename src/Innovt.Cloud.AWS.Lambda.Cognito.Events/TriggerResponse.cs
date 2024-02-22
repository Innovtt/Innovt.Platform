// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Cognito.Events

using System.Runtime.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Cognito.Events;

/// <summary>
///     Represents a base class for trigger response objects.
/// </summary>
[DataContract]
public abstract class TriggerResponse
{
    /// This class can be used as a base for trigger response objects.
    /// You can add properties or methods specific to trigger responses in derived classes.
}