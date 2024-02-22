// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Template;

/// <summary>
///     Interface for a template parser.
/// </summary>
public interface ITemplateParser
{
    /// <summary>
    ///     Renders the specified content using the provided view.
    /// </summary>
    /// <param name="content">The content to render.</param>
    /// <param name="view">The view used for rendering.</param>
    /// <returns>The rendered content.</returns>
    string Render(string content, object view);
}