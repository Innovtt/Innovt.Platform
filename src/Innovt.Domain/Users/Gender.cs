// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Users;

/// <summary>
/// Represents a gender domain model.
/// </summary>
public class Gender : DomainModel<Gender>
{
    public static Gender Male = new(1, 'M', "Masculino");
    public static Gender Female = new(2, 'F', "Feminino");

    /// <summary>
    /// Initializes a new instance of the <see cref="Gender"/> class.
    /// </summary>
    public Gender()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Gender"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier for the gender.</param>
    /// <param name="acronym">The acronym representing the gender.</param>
    /// <param name="description">The description of the gender.</param>
    public Gender(int id, char acronym, string description)
    {
        Id = id;
        Acronym = acronym;
        Description = description;
        AddModel(this);
    }

    /// <summary>
    /// Gets or sets the acronym representing the gender.
    /// </summary>
    public char Acronym { get; set; }

    /// <summary>
    /// Gets or sets the description of the gender.
    /// </summary>
    public string Description { get; set; }
}