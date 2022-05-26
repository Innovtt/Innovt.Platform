// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;

internal class UserDataModel : DataModelBase
{
    public UserDataModel()
    {
        EntityType = "User";
    }

    public string AuthId { get; set; }
    public string DomainId { get; set; }
    public List<RoleDataModel> Roles { get; set; }
    public DateTime CreatedAt { get; set; }

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