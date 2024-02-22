// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.ViewModel;

/// <summary>
///     View model representing an MVC action.
/// </summary>
public class MvcActionViewModel : ViewModelBase
{
    /// <summary>
    ///     Gets or sets the display name of the MVC action.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    ///     Gets or sets the name of the MVC action.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the name of the associated controller.
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    ///     Gets or sets the HTTP method used by the MVC action.
    /// </summary>
    public string Method { get; set; }
}