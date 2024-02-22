// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

namespace Innovt.AspNetCore.Model;

/// <summary>
///     Represents default documentation information for an API.
/// </summary>
public class DefaultApiDocumentation
{
    /// <summary>
    ///     Initializes a new instance of the DefaultApiDocumentation class with the specified API details.
    /// </summary>
    /// <param name="apiTitle">The title of the API.</param>
    /// <param name="apiDescription">The description of the API.</param>
    /// <param name="apiVersion">The version of the API.</param>
    /// <param name="contactName">The name of the contact person for the API (optional).</param>
    /// <param name="contactEmail">The email of the contact person for the API (optional).</param>
    public DefaultApiDocumentation(string apiTitle, string apiDescription, string apiVersion,
        string? contactName = null, string? contactEmail = null)
    {
        ApiTitle = apiTitle;
        ApiDescription = apiDescription;
        ApiVersion = apiVersion;
        ContactName = contactName;
        ContactEmail = contactEmail;
    }

    /// <summary>
    ///     Gets or sets the title of the API.
    /// </summary>
    public string ApiTitle { get; set; }

    /// <summary>
    ///     Gets or sets the description of the API.
    /// </summary>
    public string ApiDescription { get; set; }

    /// <summary>
    ///     Gets or sets the version of the API.
    /// </summary>
    public string ApiVersion { get; set; }

    /// <summary>
    ///     Gets or sets the name of the contact person for the API (optional).
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    ///     Gets or sets the email of the contact person for the API (optional).
    /// </summary>
    public string? ContactEmail { get; set; }
}