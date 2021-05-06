// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Notification.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders
{
    public class DefaultMessageBuilderFactory : IMessageBuilderFactory
    {
        private readonly ITemplateParser templateParser;

        public DefaultMessageBuilderFactory(ITemplateParser templateParser)
        {
            this.templateParser = templateParser;
        }

        /// <summary>
        ///     You can use your IOC Container to do It based in Names
        /// </summary>
        /// <param name="builderName"></param>
        /// <returns></returns>
        public IMessageBuilder Create(string builderName)
        {
            return new DefaultMessageBuilder(templateParser);
        }
    }
}