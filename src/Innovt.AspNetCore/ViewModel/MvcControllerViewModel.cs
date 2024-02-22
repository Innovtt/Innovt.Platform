// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.ViewModel;

/// <summary>
///     View model representing an MVC controller.
/// </summary>
public class MvcControllerViewModel : ViewModelBase
{
    /// <summary>
    ///     Gets or sets the area associated with the MVC controller.
    /// </summary>
    public string Area { get; set; }

    /// <summary>
    ///     Gets or sets the display name of the MVC controller.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    ///     Gets or sets the name of the MVC controller.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     A list to store MVC action view models associated with the controller
    /// </summary>
    private List<MvcActionViewModel> Actions { get; set; }

    /// <summary>
    ///     Adds a list of MVC action view models to the controller.
    /// </summary>
    /// <param name="actions">The list of MVC action view models to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if the actions list is null.</exception>
    public void AddActions(IList<MvcActionViewModel> actions)
    {
        ArgumentNullException.ThrowIfNull(actions);

        Actions ??= new List<MvcActionViewModel>();

        Actions.AddRange(actions);
    }
}