using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Innovt.Domain.Users;

/// <summary>
/// Represents a user in the current context.
/// </summary>
/// <typeparam></typeparam>
public record ContextUser
{
    public string UserId { get; private set; } = null!;
    
    public string? Email { get; private set; }
    
    public string? Name { get; private set; }
    
    public ReadOnlyCollection<string>? Roles { get; set; }
    
    public ReadOnlyDictionary<string, string>? Claims { get; set; }
    
    
    public ContextUser SetRoles(IList<string>? roles)
    {
        if (roles is null || roles.Count == 0)
        {
            Roles = null;
            return this;
        }
        
        Roles = new ReadOnlyCollection<string>(roles);

        return this;
    }
    
    public ContextUser SetClaims(IDictionary<string, string>? claims)
    {
        if (claims is null || claims.Count == 0)
        {
            Claims = null;
            return this;
        }
        
        Claims = new ReadOnlyDictionary<string, string>(claims);
        return this;
    }

    public ContextUser SetId(string userId)
    {
        UserId = userId;
        return this;
    }
    public ContextUser SetName(string? name)
    {
        Name = name;
        return this;
    }
    
    public ContextUser SetEmail(string? email)
    {
        Email = email;
        return this;
    }
}