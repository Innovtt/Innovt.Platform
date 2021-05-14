namespace Innovt.Authorization.Platform.Application.Commands
{
    public class AddPermissionCommand
    {
        public string Name { get; set; }

        public string Domain { get; set; }

        public string Resource { get; set; }

        public AddPermissionCommand(string loggedUserId)
        {

        }
    }
}
