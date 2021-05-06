// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Notification.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Notification.Core.Builders
{
    public interface IMessageBuilderFactory
    {
        IMessageBuilder Create(string builderName);
    }
}