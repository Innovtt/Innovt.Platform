using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Kinesis.Tests;

public class UserConfirmedEvent() : DomainEvent("UserConfirmed", "UserConfirmed");