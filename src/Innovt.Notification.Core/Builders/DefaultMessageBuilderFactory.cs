// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using Innovt.Notification.Core.Template;

namespace Innovt.Notification.Core.Builders;

/// <summary>
///     Default implementation of the message builder factory interface.
/// </summary>
public class DefaultMessageBuilderFactory : IMessageBuilderFactory
{
    private readonly ITemplateParser templateParser;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DefaultMessageBuilderFactory" /> class.
    /// </summary>
    /// <param name="templateParser">The template parser.</param>
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