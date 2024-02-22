// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.ViewModel;

/// <summary>
///     Base view model for creating or editing entities.
/// </summary>
public abstract class CreateOrEditViewModel : ViewModelBase
{
    /// <summary>
    ///     Constant representing the "Create" action.
    /// </summary>
    public const string CreateAction = "Create";

    /// <summary>
    ///     Constant representing the "Edit" action.
    /// </summary>
    public const string EditAction = "Edit";

    /// <summary>
    ///     Initializes a new instance of the CreateOrEditViewModel class.
    ///     Sets the default action to "Create".
    /// </summary>
    protected CreateOrEditViewModel()
    {
        Action = CreateAction;
    }

    /// <summary>
    ///     Gets or sets the action to be performed (Create or Edit).
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    ///     Gets or sets the identifier of the entity being created or edited.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the action is "Create".
    /// </summary>
    public bool IsCreate => Action == CreateAction;

    /// <summary>
    ///     Gets a value indicating whether the action is "Edit".
    /// </summary>
    public bool IsEdit => Action == EditAction;
}