using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.EventBridge.Tests;

public class UserConfirmedEvent() : DomainEvent("UserConfirmed", "UserConfirmed");