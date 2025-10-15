using Innovt.Domain.Users;

namespace Innovt.Domain.Security;

public interface IContextUserService
{
    ContextUser? GetCurrentUser();
}