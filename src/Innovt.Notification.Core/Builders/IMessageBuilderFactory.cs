// COMPANY: INNOVT TECNOLOGIA
// PROJECT: Innovt.Core
// DATE: 02-19-2019
// AUTHOR: michel

namespace Innovt.Notification.Core.Builders
{
    public interface IMessageBuilderFactory
    {
        IMessageBuilder Create(string builderName);
    }
}