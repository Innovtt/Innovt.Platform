// COMPANY: INNOVT TECNOLOGIA
// PROJECT: Innovt.Core
// DATE: 02-19-2019
// AUTHOR: michel

using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders
{
    public class DefaultMessageBuilderFactory: IMessageBuilderFactory
    {
        private readonly ITemplateParser templateParser;

        public DefaultMessageBuilderFactory(ITemplateParser templateParser)
        {
            this.templateParser = templateParser;
        }

        /// <summary>
        /// You can use your IOC Container to do It based in Names
        /// </summary>
        /// <param name="builderName"></param>
        /// <returns></returns>
        public IMessageBuilder Create(string builderName)
        {   
            return new DefaultMessageBuilder(templateParser);
        }
    }
}