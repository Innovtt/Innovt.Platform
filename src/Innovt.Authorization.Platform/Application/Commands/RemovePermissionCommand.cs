namespace Innovt.Authorization.Platform.Application.Commands
{
    public class RemovePermissionCommand
    {
        public int Id { get; set; }

        public RemovePermissionCommand(int loggedUserId, int id)
        {
            Id = id;
        }
    }
}
