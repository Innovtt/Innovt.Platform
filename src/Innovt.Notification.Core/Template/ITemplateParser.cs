// COMPANY: INNOVT TECNOLOGIA
// PROJECT: Innovt.Enterprise.Notification.Platform
// DATE: 02-18-2019
// AUTHOR: michel

namespace Innovt.Notification.Core.Template
{
    public interface ITemplateParser
    {
        string Render(string content, object view);
    }
}