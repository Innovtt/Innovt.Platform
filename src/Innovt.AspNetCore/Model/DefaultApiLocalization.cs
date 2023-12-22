// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.Globalization;

namespace Innovt.AspNetCore.Model;

/// <summary>
/// Represents default localization settings for an API.
/// </summary>
public class DefaultApiLocalization
{
    /// <summary>
    /// Initializes a new instance of the DefaultApiLocalization class with default culture settings.
    /// </summary>
    public DefaultApiLocalization()
    {
        RequestCulture = new CultureInfo("pt-BR");
        AddSupportedCulture("pt-br").AddSupportedCulture("en-US");
    }

    /// <summary>
    /// Gets or sets the list of supported cultures for the API.
    /// </summary>
    public IList<CultureInfo> SupportedCultures { get; private set; }

    /// <summary>
    /// Gets or sets the type of the default localization resource.
    /// </summary>
    public Type DefaultLocalizeResource { get; set; }

    /// <summary>
    /// Gets or sets the request culture for the API.
    /// </summary>
    public CultureInfo RequestCulture { get; set; }

    /// <summary>
    /// Adds a supported culture with the specified name to the list of supported cultures.
    /// </summary>
    /// <param name="name">The name of the supported culture (e.g., "pt-br").</param>
    /// <returns>The updated DefaultApiLocalization instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided name is null.</exception>
    protected DefaultApiLocalization AddSupportedCulture(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        SupportedCultures ??= new List<CultureInfo>();

        SupportedCultures.Add(CultureInfo.GetCultureInfo(name));

        return this;
    }
}