// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

namespace ConsoleAppTest.DataModels;

public class AuthProviderDataModel : BaseDataModel
{
    public AuthProviderDataModel()
    {
        EntityType = "AuthProvider";
    }

    public string Name { get; set; }

    public string? Domain { get; set; }

    public bool Enabled { get; set; }

    public static string BuildPk()
    {
        return $"AUTH_PROVIDER";
    }
}