namespace ConsoleAppTest.DataModels
{
    public class AuthProviderDataModel : BaseDataModel
    {
        public string Name { get; set; }

        public string? Domain { get; set; }

        public bool Enabled { get; set; }

        public AuthProviderDataModel()
        {
            EntityType = "AuthProvider";
        }

        public static string BuildPk()
        {
            return $"AUTH_PROVIDER";
        }
    }
}