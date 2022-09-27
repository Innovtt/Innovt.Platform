// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Builders;

public interface IMessageBuilderFactory
{
    IMessageBuilder Create(string builderName);
}