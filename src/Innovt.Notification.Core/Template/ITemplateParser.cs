// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Template;

public interface ITemplateParser
{
    string Render(string content, object view);
}