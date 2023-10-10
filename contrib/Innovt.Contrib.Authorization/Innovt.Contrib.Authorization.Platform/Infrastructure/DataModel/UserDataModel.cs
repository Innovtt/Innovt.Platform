// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using System.Collections.Generic;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;
/// <summary>
/// Represents a data model for a user.
/// </summary>
internal class UserDataModel : DataModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDataModel"/> class.
    /// </summary>
    public UserDataModel()
    {
        EntityType = "User";
    }
    /// <summary>
    /// Gets or sets the authentication ID of the user.
    /// </summary>
    public string AuthId { get; set; }
    /// <summary>
    /// Gets or sets the domain ID of the user.
    /// </summary>
    public string DomainId { get; set; }
    /// <summary>
    /// Gets or sets the roles associated with the user.
    /// </summary>
    public List<RoleDataModel> Roles { get; set; }
    /// <summary>
    /// Gets or sets the creation date and time of the user.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Converts a data model instance to an <see cref="AuthUser"/>.
    /// </summary>
    /// <param name="userDataModel">The data model to convert.</param>
    /// <returns>An <see cref="AuthUser"/> instance.</returns>
    public static AuthUser ToUser(UserDataModel userDataModel)
    {
        if (userDataModel is null)
            return null;

        var authUser = new AuthUser
        {
            Id = userDataModel.AuthId,
            DomainId = userDataModel.DomainId,
            CreatedAt = userDataModel.CreatedAt
        };

        if (userDataModel.Roles == null) return authUser;


        foreach (var role in userDataModel.Roles) authUser.AssignRole(RoleDataModel.ToDomain(role));

        return authUser;
    }
    /// <summary>
    /// Converts an <see cref="AuthUser"/> instance to a data model.
    /// </summary>
    /// <param name="user">The <see cref="AuthUser"/> instance to convert.</param>
    /// <returns>A data model instance.</returns>
    public static UserDataModel FromUser(AuthUser user)
    {
        if (user is null)
            return null;

        var dataModel = new UserDataModel
        {
            Id = $"U#{user.Id}",
            Sk = $"DID#{user.DomainId}",
            AuthId = user.Id,
            DomainId = user.DomainId,
            CreatedAt = user.CreatedAt.GetValueOrDefault().UtcDateTime
        };

        if (user.Roles == null) return dataModel;

        dataModel.Roles = new List<RoleDataModel>();

        foreach (var role in user.Roles) dataModel.Roles.Add(RoleDataModel.FromDomain(role));

        return dataModel;
    }
}