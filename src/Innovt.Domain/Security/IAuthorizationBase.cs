using System;

namespace Innovt.Domain.Security;

public interface IAuthorizationBase
{
    Guid UserId { get; set; }
}